// Parallel Planner with Review: plan → build → review → pull request loop
//
//   Phase 1 (Plan):   The host reads the open Sandcastle issues with its own gh
//                     auth, keeping only issues and comments from the owner,
//                     members and collaborators, and holds back every issue
//                     that already has an open PR from this repository. A
//                     planner agent picks the ones that can be built in
//                     parallel; the host keeps each offered issue once
//                     (lib/plan.mts) and names its branch (lib/branches.mts).
//   Phase 2 (Build):  For each issue, in its own sandbox (lib/build.mts): the
//                     implementer works the issue until it signals completion,
//                     the branch merges origin/main, the host runs
//                     .sandcastle/check.sh, a reviewer refines the change and
//                     returns an approve/reject verdict, and the host checks
//                     again if the reviewer moved HEAD. The commit that passed
//                     is pushed and gets its own draft PR that fixes the issue;
//                     anything else gets a comment on the issue and isn't
//                     pushed. All pipelines run concurrently.
//
// Nothing is merged into main and no issue is closed here: each change reaches
// main through its PR and the checks in docs/PROCESS.md.
//
// The sandbox gets no GitHub token. Agents read the issue from their prompt,
// and every GitHub write (comments, pushes, PRs) is made by the host, in code.
//
// The loop repeats up to MAX_ITERATIONS times. Nothing reaches main between
// rounds, so a later round only picks up issues the earlier picks didn't
// block: the planner sees the issues in review, and the ones that stopped, as
// open blockers. An issue that stopped isn't built again in the same run. The
// loop stops early when a round opens no pull request, and at once when an
// agent has changed the clone's git config.
//
// Usage:
//   pnpm run sandcastle

import { existsSync, readFileSync } from "node:fs";
import * as sandcastle from "@ai-hero/sandcastle";
import { docker } from "@ai-hero/sandcastle/sandboxes/docker";
import { fetchMain, prepareBranches, withoutOpenPullRequests } from "./lib/branches.mts";
import { buildIssue } from "./lib/build.mts";
import { MAX_ITERATIONS, MODEL } from "./lib/config.mts";
import { listSandcastleIssues, openPullRequestBranches, repoName } from "./lib/github.mts";
import { planSchema, readyPicks } from "./lib/plan.mts";
import { plannerPromptArgs } from "./lib/prompts.mts";
import { ENV_FILE, githubTokensIn } from "./lib/sandbox-env.mts";
import { GitConfigChangedError, protectHostGit, recordGitConfig } from "./lib/shell.mts";

// First, before anything runs git: hooks and fsmonitor off for every git on
// the host, Sandcastle's own included. See lib/shell.mts.
protectHostGit();

const envFile = ENV_FILE;
const leakedTokens = existsSync(envFile) ? githubTokensIn(readFileSync(envFile, "utf8")) : [];
if (leakedTokens.length > 0) {
  throw new Error(
    `${envFile} sets ${leakedTokens.join(" and ")}, which Sandcastle would pass into the sandbox. ` +
      "Remove it: the host uses its own gh auth, and agents must not reach GitHub.",
  );
}

// Before any sandbox starts: record the clone's git config, which the host
// refuses to run git past once an agent has changed it, and read the
// repository's name, which gh would otherwise take from the clone's remotes.
// See lib/shell.mts.
recordGitConfig();
const { owner, name } = repoName();
console.log(`Building Sandcastle issues in ${owner}/${name}.`);

// Issues that stopped this run without a pull request. Each already has a
// comment saying why, and building it again in the next round would most
// likely stop the same way, so it waits for the next run.
const stopped = new Set<number>();

for (let iteration = 1; iteration <= MAX_ITERATIONS; iteration++) {
  console.log(`\n=== Iteration ${iteration}/${MAX_ITERATIONS} ===\n`);

  // -------------------------------------------------------------------------
  // Phase 1: Plan
  // -------------------------------------------------------------------------
  const open = withoutOpenPullRequests(listSandcastleIssues(), openPullRequestBranches());
  const ready = open.ready.filter((issue) => !stopped.has(issue.number));
  const stoppedEarlier = open.ready.filter((issue) => stopped.has(issue.number));
  for (const issue of open.inReview) {
    console.log(`  ⏸ #${issue.number} is held back: its pull request is open.`);
  }
  for (const issue of stoppedEarlier) {
    console.log(`  ⏸ #${issue.number} is held back: it stopped earlier in this run.`);
  }
  // Neither kind is on main, so both still block the issues that depend on them.
  const heldBack = [...open.inReview, ...stoppedEarlier];
  if (ready.length === 0) {
    console.log("No open Sandcastle issues ready to build. Exiting.");
    break;
  }

  const plan = await sandcastle.run({
    sandbox: docker(),
    name: "planner",
    // Structured output requires maxIterations: 1.
    maxIterations: 1,
    agent: sandcastle.claudeCode(MODEL),
    promptFile: "./.sandcastle/plan-prompt.md",
    promptArgs: plannerPromptArgs(ready, heldBack),
    // Throws StructuredOutputError if the tag is missing, the JSON is
    // malformed, or validation fails, which aborts the loop.
    output: sandcastle.Output.object({ tag: "plan", schema: planSchema }),
  });

  const picks = readyPicks(plan.output.issues, ready);

  if (picks.length === 0) {
    console.log("No unblocked issues to work on. Exiting.");
    break;
  }

  // -------------------------------------------------------------------------
  // Phase 2: Build, review and publish
  // -------------------------------------------------------------------------
  fetchMain();
  const work = prepareBranches(picks);

  console.log(`Planning complete. ${work.length} issue(s) to build in parallel:`);
  for (const { issue, branch } of work) {
    console.log(`  #${issue.number}: ${issue.title} → ${branch}`);
  }

  // Promise.allSettled means one failing pipeline doesn't cancel the others.
  const settled = await Promise.allSettled(work.map(({ issue, branch }) => buildIssue(issue, branch)));

  // An agent changed the clone's git config: stop the whole run, with the
  // warning last, rather than carry on as if one issue had failed.
  const configChanged = settled.find(
    (outcome): outcome is PromiseRejectedResult => outcome.status === "rejected" && outcome.reason instanceof GitConfigChangedError,
  );

  const published: string[] = [];
  for (const [i, outcome] of settled.entries()) {
    const { issue, branch } = work[i]!;
    if (outcome.status === "rejected") {
      console.error(`  ✗ #${issue.number} (${branch}) failed: ${outcome.reason}`);
      stopped.add(issue.number);
    } else if (outcome.value.prUrl) {
      published.push(`  #${issue.number} (${branch}) → ${outcome.value.prUrl}`);
    } else {
      stopped.add(issue.number);
    }
  }

  if (configChanged) throw configChanged.reason;

  console.log(`\nRound complete. ${published.length} pull request(s):`);
  for (const line of published) console.log(line);

  if (published.length === 0) {
    // Nothing reached a PR, so the next plan would pick the same issues and
    // repeat the same round. Stop and let a person look at the issue comments.
    console.log("No pull requests opened this round. Stopping.");
    break;
  }
}

console.log("\nAll done.");
