import assert from "node:assert/strict";
import { describe, it } from "node:test";
import { buildIssue, type BuildHost, type BuildSandbox } from "./build.mts";

const issue = { number: 7, title: "Add search", body: "Search issues.", labels: ["Sandcastle"], comments: [] };
const branch = "feature/7-add-search";

type RunResult = { completionSignal?: string; commits?: { sha: string }[]; stdout?: string; head?: string } | Error;

// A pipeline with scripted agent runs, merge and check results, recording what
// the host was asked to do. `head` in a run's result is where that run leaves
// HEAD; it starts at c1.
function pipeline(options: {
  implementer?: RunResult;
  reviewer?: RunResult;
  checks?: boolean[];
  ahead?: number[];
  mergeFails?: boolean;
  publishError?: Error;
}) {
  const checks = [...(options.checks ?? [true, true])];
  const ahead = [...(options.ahead ?? [1, 1])];
  let head = "c1";
  const calls = {
    runs: [] as string[],
    commands: [] as string[],
    fetches: 0,
    comments: [] as string[],
    published: [] as { commit: string; title: string; body: string }[],
    closed: false,
  };
  const results: Record<string, RunResult> = {
    implementer: options.implementer ?? { completionSignal: "<promise>COMPLETE</promise>", commits: [{ sha: "c1" }] },
    reviewer: options.reviewer ?? { stdout: '<verdict>{"approved": true, "summary": "Clean."}</verdict>', commits: [] },
  };

  const sandbox = {
    worktreePath: "/tmp/worktree",
    run: async (opts: { name?: string }) => {
      calls.runs.push(opts.name!);
      const result = results[opts.name!]!;
      if (result instanceof Error) throw result;
      if (result.head) head = result.head;
      return { iterations: [], stdout: "", commits: [], ...result };
    },
    exec: async (command: string) => {
      calls.commands.push(command);
      if (command.startsWith(".sandcastle/check.sh")) {
        const passed = checks.shift();
        if (passed === undefined) throw new Error("check ran more often than scripted");
        return { stdout: passed ? "ok" : "error CS1002", stderr: "", exitCode: passed ? 0 : 1 };
      }
      if (command.startsWith("git merge --no-edit") && options.mergeFails) {
        return { stdout: "CONFLICT (content): Merge conflict in src/UI/Program.cs", stderr: "", exitCode: 1 };
      }
      return { stdout: "", stderr: "", exitCode: 0 };
    },
    close: async () => {
      calls.closed = true;
      return {};
    },
  } as unknown as BuildSandbox;

  const host: BuildHost = {
    createSandbox: async () => sandbox,
    fetchBase: () => {
      calls.fetches++;
    },
    commitsAhead: () => {
      const count = ahead.shift();
      if (count === undefined) throw new Error("commitsAhead ran more often than scripted");
      return count;
    },
    head: () => head,
    commentOnIssue: (_, body) => calls.comments.push(body),
    publish: (commit, _, title, body) => {
      if (options.publishError) throw options.publishError;
      calls.published.push({ commit, title, body });
      return "https://github.com/o/r/pull/1";
    },
    log: () => {},
  };

  return { run: () => buildIssue(issue, branch, host), calls, checksLeft: checks };
}

describe("buildIssue", () => {
  it("publishes an approved, checked branch as a PR that fixes the issue", async () => {
    const { run, calls } = pipeline({});
    const result = await run();
    assert.deepEqual(result, { outcome: "published", prUrl: "https://github.com/o/r/pull/1" });
    assert.deepEqual(calls.runs, ["implementer", "reviewer"]);
    assert.equal(calls.published[0]!.commit, "c1");
    assert.equal(calls.published[0]!.title, "feat: Add search");
    assert.match(calls.published[0]!.body, /Fixes #7/);
    assert.deepEqual(calls.comments, []);
    assert.ok(calls.closed);
  });

  it("merges a freshly fetched origin/main before the check", async () => {
    const { run, calls } = pipeline({});
    await run();
    assert.equal(calls.fetches, 1);
    const merge = calls.commands.findIndex((command) => command.startsWith("git merge --no-edit origin/main"));
    const check = calls.commands.findIndex((command) => command.startsWith(".sandcastle/check.sh"));
    assert.ok(merge !== -1 && merge < check, calls.commands.join("\n"));
  });

  it("stops on a merge conflict with main, aborts the merge and says why", async () => {
    const { run, calls } = pipeline({ mergeFails: true });
    assert.equal((await run()).outcome, "merge-conflict");
    assert.ok(calls.commands.includes("git merge --abort 2>&1"));
    assert.deepEqual(calls.runs, ["implementer"]);
    assert.match(calls.comments[0]!, /Merge conflict in src\/UI\/Program\.cs/);
    assert.deepEqual(calls.published, []);
  });

  it("doesn't publish when the reviewer rejects, and says why on the issue", async () => {
    const { run, calls } = pipeline({
      reviewer: { stdout: '<verdict>{"approved": false, "summary": "No test for an empty query."}</verdict>' },
    });
    assert.equal((await run()).outcome, "rejected");
    assert.deepEqual(calls.published, []);
    assert.match(calls.comments[0]!, /No test for an empty query/);
  });

  it("treats a review without a verdict as a rejection", async () => {
    const { run, calls } = pipeline({ reviewer: { stdout: "<promise>COMPLETE</promise>" } });
    assert.equal((await run()).outcome, "rejected");
    assert.deepEqual(calls.published, []);
  });

  it("treats a reviewer that throws as a rejection", async () => {
    const { run, calls } = pipeline({ reviewer: new Error("sandbox crashed") });
    assert.equal((await run()).outcome, "rejected");
    assert.match(calls.comments[0]!, /sandbox crashed/);
  });

  it("stops when the implementer doesn't finish, even if it committed", async () => {
    const { run, calls } = pipeline({ implementer: { commits: [{ sha: "c1" }] } });
    assert.equal((await run()).outcome, "implementer-unfinished");
    assert.deepEqual(calls.runs, ["implementer"]);
    assert.match(calls.comments[0]!, /ran out of iterations/);
    assert.deepEqual(calls.published, []);
  });

  it("stops when the implementer throws", async () => {
    const { run, calls } = pipeline({ implementer: new Error("network down") });
    assert.equal((await run()).outcome, "implementer-unfinished");
    assert.match(calls.comments[0]!, /network down/);
  });

  it("stops when the check fails after the implementer", async () => {
    const { run, calls } = pipeline({ checks: [false] });
    assert.equal((await run()).outcome, "check-failed");
    assert.deepEqual(calls.runs, ["implementer"]);
    assert.match(calls.comments[0]!, /error CS1002/);
  });

  it("checks again when the reviewer moves HEAD, and stops when that fails", async () => {
    const { run, calls } = pipeline({
      reviewer: { stdout: '<verdict>{"approved": true, "summary": "Tidied."}</verdict>', commits: [{ sha: "c2" }], head: "c2" },
      checks: [true, false],
    });
    assert.equal((await run()).outcome, "check-failed");
    assert.deepEqual(calls.published, []);
  });

  it("checks again when the reviewer amends without reporting a commit, and publishes the new HEAD", async () => {
    const { run, calls, checksLeft } = pipeline({
      reviewer: { stdout: '<verdict>{"approved": true, "summary": "Amended."}</verdict>', commits: [], head: "c1-amended" },
      checks: [true, true],
    });
    assert.equal((await run()).outcome, "published");
    assert.equal(checksLeft.length, 0);
    assert.equal(calls.published[0]!.commit, "c1-amended");
  });

  it("publishes nothing when the reviewer resets the branch to main", async () => {
    const { run, calls } = pipeline({
      reviewer: { stdout: '<verdict>{"approved": true, "summary": "Reset."}</verdict>', head: "main" },
      ahead: [1, 0],
    });
    assert.equal((await run()).outcome, "nothing-to-publish");
    assert.deepEqual(calls.published, []);
  });

  it("doesn't check again when the reviewer leaves HEAD alone", async () => {
    const { run, checksLeft } = pipeline({ checks: [true, true] });
    assert.equal((await run()).outcome, "published");
    assert.equal(checksLeft.length, 1);
  });

  it("publishes earlier work when a re-run adds no commits", async () => {
    const { run, calls } = pipeline({ implementer: { completionSignal: "<promise>COMPLETE</promise>", commits: [] } });
    assert.equal((await run()).outcome, "published");
    assert.equal(calls.published.length, 1);
  });

  it("publishes nothing when the branch has no work main lacks", async () => {
    const { run, calls } = pipeline({ ahead: [0] });
    assert.equal((await run()).outcome, "nothing-to-publish");
    assert.deepEqual(calls.runs, ["implementer"]);
    assert.deepEqual(calls.comments, []);
  });

  it("reports a failed push on the issue", async () => {
    const { run, calls } = pipeline({ publishError: new Error("rejected: non-fast-forward") });
    assert.equal((await run()).outcome, "publish-failed");
    assert.match(calls.comments[0]!, /non-fast-forward/);
    assert.ok(calls.closed);
  });
});
