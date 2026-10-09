// Build one planned issue in a single sandbox: implement, bring in main, check,
// review, check again if the reviewer changed anything, and publish exactly the
// commit that passed as the issue's own pull request. Nothing is merged into
// main here and no issue is closed: the PR says "Fixes #n", so the issue closes
// when the PR merges through the checks and review in docs/PROCESS.md.

import * as sandcastle from "@ai-hero/sandcastle";
import { docker } from "@ai-hero/sandcastle/sandboxes/docker";
import { commitsAhead, fetchMain, headOf } from "./branches.mts";
import { checkFileChanges, fenced, runCheck, tail } from "./check.mts";
import { BASE_BRANCH, CHECK_COMMENT_LINES, copyToWorktree, hooks, IMPLEMENTER_ITERATIONS, MODEL } from "./config.mts";
import { commentOnIssue, openPullRequest, type SandcastleIssue } from "./github.mts";
import { issuePromptArgs } from "./prompts.mts";
import { prBody, prTitle } from "./publish.mts";
import { containsSandboxSecret } from "./sandbox-env.mts";
import { publishedText } from "./scan.mts";
import { git, GitConfigChangedError, gitConfigChanged } from "./shell.mts";
import { parseVerdict } from "./verdict.mts";

// The parts of a sandbox buildIssue uses; tests pass a fake.
export type BuildSandbox = Pick<sandcastle.Sandbox, "run" | "exec" | "close" | "worktreePath">;

// What buildIssue needs from outside the pipeline; tests pass stubs.
export type BuildHost = {
  createSandbox(branch: string): Promise<BuildSandbox>;
  // Refresh main from origin and return its commit.
  fetchBase(): string;
  commitsAhead(worktreePath: string, base: string): number;
  head(worktreePath: string): string;
  // The files the branch changes that decide what the check runs.
  checkFileChanges(worktreePath: string, base: string): string[];
  // Whether the commits from base to commit, their diffs or messages, hold
  // one of the sandbox's secrets.
  leaksSecret(base: string, commit: string): boolean;
  commentOnIssue(issueNumber: number, body: string): void;
  // Push the commit to the branch on origin and open (or reuse) its pull request.
  publish(commit: string, branch: string, title: string, body: string): string;
  // Whether the clone's git config has changed since the run started.
  gitConfigChanged(): boolean;
  log(line: string): void;
};

// Push the checked commit by its id, so nothing committed after the check can
// ride along. Hooks are off (see git in shell.mts): the pre-push hook is a file
// in the worktree the agents edited, so the host can't run it. CI runs the full
// suite, the Docker-backed tests included. No force: a push that doesn't
// fast-forward origin's branch fails and is reported, rather than drop its work.
function publish(commit: string, branch: string, title: string, body: string): string {
  git(process.cwd(), "push", "--quiet", "origin", `${commit}:refs/heads/${branch}`);
  return openPullRequest(branch, title, body);
}

// Whether what the push would publish (see publishedText) holds a secret.
function leaksSecret(base: string, commit: string): boolean {
  return containsSandboxSecret(publishedText(process.cwd(), base, commit));
}

const liveHost: BuildHost = {
  createSandbox: (branch) =>
    sandcastle.createSandbox({ branch, baseBranch: BASE_BRANCH, sandbox: docker(), hooks, copyToWorktree }),
  fetchBase: fetchMain,
  commitsAhead,
  head: headOf,
  checkFileChanges,
  leaksSecret,
  commentOnIssue,
  publish,
  gitConfigChanged,
  log: console.log,
};

export type BuildOutcome =
  | "published"
  | "nothing-to-publish"
  | "implementer-unfinished"
  | "merge-conflict"
  | "check-failed"
  | "rejected"
  | "secret-in-commits"
  | "publish-failed";

export async function buildIssue(
  issue: SandcastleIssue,
  branch: string,
  host: BuildHost = liveHost,
): Promise<{ outcome: BuildOutcome; prUrl?: string }> {
  const log = (line: string) => host.log(`  #${issue.number} ${line}`);
  const stop = (outcome: BuildOutcome, comment: string) => {
    log(`stopped: ${outcome}`);
    host.commentOnIssue(issue.number, comment);
    return { outcome };
  };
  const notPushed = `\`${branch}\` wasn't pushed; the local branch keeps its commits.`;
  const promptArgs = issuePromptArgs(issue, branch);

  const sandbox = await host.createSandbox(branch);
  // Once an agent has changed .git/config, Sandcastle's own git (removing the
  // worktree in close()) would run under it too, so the sandbox is left as it
  // is and the error goes up to stop the run.
  let configChanged = false;
  try {
    // Implement. A run that throws or uses up its iterations without
    // signalling completion stops the issue for this round, whatever it
    // committed: unfinished work is never published.
    let finished: boolean;
    let failure = "it ran out of iterations unfinished";
    try {
      const implement = await sandbox.run({
        name: "implementer",
        agent: sandcastle.claudeCode(MODEL),
        maxIterations: IMPLEMENTER_ITERATIONS,
        promptFile: "./.sandcastle/implement-prompt.md",
        promptArgs,
      });
      finished = implement.completionSignal !== undefined;
    } catch (error) {
      finished = false;
      failure = `it failed: ${error}`;
    }
    if (!finished) return stop("implementer-unfinished", `Sandcastle stopped building this issue: the implementer ${failure}. ${notPushed}`);

    // Bring in main as it is now, so the check and the review see the branch
    // as it would merge, and the PR isn't behind main when it opens. The merge
    // runs in the sandbox, where the worktree's hooks are the agents' own.
    const base = host.fetchBase();
    const merge = await sandbox.exec(`git merge --no-edit ${base} 2>&1`);
    if (merge.exitCode !== 0) {
      await sandbox.exec("git merge --abort 2>&1");
      return stop(
        "merge-conflict",
        `Sandcastle stopped building this issue: \`${branch}\` doesn't merge cleanly with \`${BASE_BRANCH}\`. ${notPushed}\n\n` +
          fenced(tail(merge.stdout, CHECK_COMMENT_LINES)),
      );
    }

    // Check, review and publish whenever the branch holds work main doesn't,
    // not only when this run added commits: a re-run of a finished issue
    // makes none, and its earlier work still needs a PR.
    if (host.commitsAhead(sandbox.worktreePath, base) === 0) {
      log("nothing to publish");
      return { outcome: "nothing-to-publish" };
    }

    // Run the check and return the commit it passed on, or undefined (after
    // saying why on the issue) when it failed.
    const checkedCommit = async (when: string) => {
      const check = await runCheck(sandbox);
      log(`check ${when}: ${check.passed ? "passed" : "failed"}`);
      if (check.passed) return host.head(sandbox.worktreePath);
      host.commentOnIssue(
        issue.number,
        `Sandcastle stopped building this issue: \`.sandcastle/check.sh\` failed ${when}. ${notPushed}\n\n` +
          `The last ${CHECK_COMMENT_LINES} lines of its output:\n\n${fenced(tail(check.output, CHECK_COMMENT_LINES))}`,
      );
      return undefined;
    };

    let checked = await checkedCommit("after the implementer");
    if (!checked) return { outcome: "check-failed" };

    // Review. The reviewer may commit refinements, and must end with a
    // verdict; anything but an approval keeps the branch from being published.
    let verdict;
    try {
      const review = await sandbox.run({
        name: "reviewer",
        agent: sandcastle.claudeCode(MODEL),
        maxIterations: 1,
        promptFile: "./.sandcastle/review-prompt.md",
        promptArgs,
      });
      verdict = parseVerdict(review.stdout);
    } catch (error) {
      verdict = { approved: false, summary: `The reviewer failed: ${error}` };
    }
    log(`reviewer ${verdict.approved ? "approved" : "rejected"}`);
    if (!verdict.approved) {
      return stop("rejected", `Sandcastle's reviewer rejected this issue's change, so ${notPushed}\n\n${verdict.summary}`);
    }

    // Compare HEAD itself, not the commits the reviewer's run reports: an
    // amend, reset or rebase moves HEAD without adding a commit, and only a
    // commit that passed the check may be published.
    if (host.head(sandbox.worktreePath) !== checked) {
      checked = await checkedCommit("after the reviewer changed the branch");
      if (!checked) return { outcome: "check-failed" };
      if (host.commitsAhead(sandbox.worktreePath, base) === 0) {
        log("nothing to publish after the review");
        return { outcome: "nothing-to-publish" };
      }
    }

    // Publish while the worktree still exists; close() may remove it.
    try {
      // The push is public, and the sandbox holds the Claude token or API key.
      // The comment doesn't say where the secret is, since it's public too.
      if (host.leaksSecret(base, checked)) {
        return stop(
          "secret-in-commits",
          `Sandcastle didn't publish this issue: a commit on \`${branch}\` holds one of the sandbox's secrets, or something ` +
            `shaped like a Claude or GitHub token. ${notPushed} Find it with \`git log -p ${BASE_BRANCH}..${branch}\` before ` +
            "anything pushes the branch, and rotate the secret if it has left this machine.",
        );
      }
      const checkFiles = host.checkFileChanges(sandbox.worktreePath, base);
      if (checkFiles.length > 0) log(`changes the check's own files: ${checkFiles.join(", ")}`);
      const prUrl = host.publish(checked, branch, prTitle(issue), prBody(issue, verdict.summary, checkFiles));
      log(`published ${prUrl}`);
      return { outcome: "published", prUrl };
    } catch (error) {
      if (error instanceof GitConfigChangedError) throw error;
      return stop("publish-failed", `Sandcastle couldn't publish \`${branch}\`: ${error}`);
    }
  } catch (error) {
    if (error instanceof GitConfigChangedError) configChanged = true;
    throw error;
  } finally {
    // Another pipeline's agent may have changed the config after this
    // pipeline's last host git call, so ask again rather than rely on having
    // seen the error.
    if (!configChanged && !host.gitConfigChanged()) await sandbox.close();
  }
}
