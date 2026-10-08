import assert from "node:assert/strict";
import { execFileSync } from "node:child_process";
import { describe, it } from "node:test";
import { branchFor, isIssueBranch, parseHeads, prepareBranches, slugFor, withoutOpenPullRequests } from "./branches.mts";

const issue = (number: number, title: string, labels: string[] = ["Sandcastle"]) => ({ number, title, labels });

// The branch standard, from the script the pre-push hook and CI share.
const passesBranchStandard = (branch: string) => {
  try {
    execFileSync("bash", ["scripts/check-branch-name.sh", branch], { stdio: "ignore" });
    return true;
  } catch {
    return false;
  }
};

describe("slugFor", () => {
  it("drops the conventional-commit prefix, lower-cases and joins words with hyphens", () => {
    assert.equal(
      slugFor("feat(sandcastle): Hold back issues whose blockers haven't landed"),
      "hold-back-issues-whose-blockers-havent-landed",
    );
  });

  it("drops prefixes without a scope or with a breaking-change mark", () => {
    assert.equal(slugFor("fix: Stop the crash"), "stop-the-crash");
    assert.equal(slugFor("refactor(Domain)!: Rename Result"), "rename-result");
  });

  it("drops curly apostrophes too", () => {
    assert.equal(slugFor("Don’t reuse the cache"), "dont-reuse-the-cache");
  });

  it("joins every run of other characters into one hyphen and trims the ends", () => {
    assert.equal(slugFor("  Add  C# / .NET 10 support!  "), "add-c-net-10-support");
  });

  it("cuts a long title at a hyphen within 50 characters", () => {
    const slug = slugFor("Make the planner hold back every issue whose blockers have not landed on main yet");
    assert.equal(slug, "make-the-planner-hold-back-every-issue-whose");
    assert.ok(slug.length <= 50);
  });

  it("falls back to 'issue' when the title has no letters or digits", () => {
    assert.equal(slugFor("feat: ???"), "issue");
  });
});

describe("isIssueBranch", () => {
  it("matches the issue's feature, fix and hotfix branches, not another issue's", () => {
    assert.ok(isIssueBranch("feature/4-add-search", 4));
    assert.ok(isIssueBranch("fix/4-stop-the-crash", 4));
    assert.ok(isIssueBranch("hotfix/4-stop-the-crash", 4));
    assert.ok(!isIssueBranch("feature/42-add-search", 4));
    assert.ok(!isIssueBranch("fix/42-stop-the-crash", 4));
    assert.ok(!isIssueBranch("chore/4-add-search", 4));
  });
});

describe("branchFor", () => {
  it("names a bug's branch fix/{n}-{slug}", () => {
    assert.equal(branchFor(issue(7, "fix: Stop the crash", ["Sandcastle", "bug"]), []), "fix/7-stop-the-crash");
  });

  it("names any other issue's branch feature/{n}-{slug}", () => {
    assert.equal(branchFor(issue(8, "feat: Add search"), []), "feature/8-add-search");
  });

  it("reuses the issue's existing branch after its title or labels change", () => {
    const existing = ["feature/80-other-work", "feature/8-add-search"];
    assert.equal(branchFor(issue(8, "feat: Add full-text search", ["Sandcastle", "bug"]), existing), "feature/8-add-search");
  });

  it("reuses an existing fix/ or hotfix/ branch", () => {
    assert.equal(branchFor(issue(9, "Add search"), ["fix/9-stop-the-crash"]), "fix/9-stop-the-crash");
    assert.equal(branchFor(issue(9, "Add search", ["bug"]), ["hotfix/9-urgent"]), "hotfix/9-urgent");
  });

  it("only names branches that pass the branch standard", () => {
    for (const title of ["feat: Add search", "fix: Stop the crash!", "???", "Don’t reuse the cache"]) {
      for (const labels of [["Sandcastle"], ["Sandcastle", "bug"]]) {
        const branch = branchFor(issue(12, title, labels), []);
        assert.ok(passesBranchStandard(branch), `${branch} fails scripts/check-branch-name.sh`);
      }
    }
  });
});

describe("withoutOpenPullRequests", () => {
  it("holds back issues with an open PR from one of their branches", () => {
    const issues = [issue(1, "One"), issue(2, "Two"), issue(3, "Three")];
    const { ready, inReview } = withoutOpenPullRequests(issues, ["fix/2-two", "feature/30-other", "chore/tidy"]);
    assert.deepEqual(ready.map((i) => i.number), [1, 3]);
    assert.deepEqual(inReview.map((i) => i.number), [2]);
  });
});

describe("parseHeads", () => {
  it("strips refs/heads/ from ls-remote output", () => {
    assert.deepEqual(parseHeads("abc\trefs/heads/feature/1-a\ndef\trefs/heads/fix/2-b\n"), ["feature/1-a", "fix/2-b"]);
  });
});

describe("prepareBranches", () => {
  // A clone with the given branches, recording what was fetched and moved.
  const clone = (remote: string[], local: string[]) => {
    const calls = { fetched: [] as string[], fastForwarded: [] as string[] };
    return {
      calls,
      git: {
        remoteIssueBranches: () => remote,
        localIssueBranches: () => local,
        fetch: (branch: string) => calls.fetched.push(branch),
        fastForward: (branch: string) => calls.fastForwarded.push(branch),
      },
    };
  };

  it("fetches only the branches that already exist on origin", () => {
    const { git, calls } = clone(["feature/1-add-search"], []);
    const work = prepareBranches([issue(1, "Add search"), issue(2, "Stop the crash", ["bug"])], git);
    assert.deepEqual(work.map((w) => w.branch), ["feature/1-add-search", "fix/2-stop-the-crash"]);
    assert.deepEqual(calls.fetched, ["feature/1-add-search"]);
    assert.deepEqual(calls.fastForwarded, []);
  });

  it("reuses a local branch a failed build left unpushed", () => {
    const { git, calls } = clone([], ["feature/3-old-title"]);
    const work = prepareBranches([issue(3, "New title")], git);
    assert.deepEqual(work.map((w) => w.branch), ["feature/3-old-title"]);
    assert.deepEqual(calls.fetched, []);
  });

  it("fast-forwards a local branch that origin also has", () => {
    const { git, calls } = clone(["feature/4-add-search"], ["feature/4-add-search"]);
    prepareBranches([issue(4, "Add search")], git);
    assert.deepEqual(calls.fetched, ["feature/4-add-search"]);
    assert.deepEqual(calls.fastForwarded, ["feature/4-add-search"]);
  });
});
