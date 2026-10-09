import assert from "node:assert/strict";
import { execFileSync } from "node:child_process";
import { mkdtempSync, rmSync, writeFileSync } from "node:fs";
import { tmpdir } from "node:os";
import { join } from "node:path";
import { describe, it } from "node:test";
import { addedLines, publishedText } from "./scan.mts";

// These tests make their own repository. A git hook (pre-push runs these
// tests) can export GIT_DIR and friends, which would point git at this clone.
for (const key of ["GIT_DIR", "GIT_WORK_TREE", "GIT_INDEX_FILE", "GIT_PREFIX", "GIT_COMMON_DIR"]) delete process.env[key];

describe("addedLines", () => {
  it("keeps added lines from plain and combined diffs, not headers or removed lines", () => {
    const patch = [
      "diff --git a/f b/f",
      "--- a/f",
      "+++ b/f",
      "@@ -1 +1 @@",
      "-old",
      "+new",
      "diff --cc g",
      "+++ b/g",
      "@@@ -1,1 -1,1 +1,1 @@@",
      "++evil",
      " +ours",
      "+ theirs",
      "- gone",
    ].join("\n");
    assert.deepEqual(addedLines(patch), ["new", "evil", "ours", "theirs"]);
  });
});

describe("publishedText", () => {
  it("includes what a merge commit adds, and not the context around an edit", () => {
    const repo = mkdtempSync(join(tmpdir(), "sandcastle-scan-"));
    try {
      const env = { ...process.env, GIT_AUTHOR_NAME: "t", GIT_AUTHOR_EMAIL: "t@t", GIT_COMMITTER_NAME: "t", GIT_COMMITTER_EMAIL: "t@t" };
      const git = (...args: string[]) => execFileSync("git", args, { cwd: repo, env, encoding: "utf8" }).trim();
      const write = (file: string, text: string) => writeFileSync(join(repo, file), text);

      git("init", "--quiet", "-b", "main");
      write("fixture.txt", "token = ghp_FAKEFAKEFAKEFAKEFAKEFAKE\nline two\n");
      write("other.txt", "one\n");
      git("add", ".");
      git("commit", "--quiet", "-m", "base");
      const base = git("rev-parse", "HEAD");

      // The branch edits the line next to main's fake token.
      git("checkout", "--quiet", "-b", "issue");
      write("fixture.txt", "token = ghp_FAKEFAKEFAKEFAKEFAKEFAKE\nline 2\n");
      git("commit", "--quiet", "-am", "Edit beside the fixture");

      // Main moves on, and the branch's merge of it carries content of its own.
      git("checkout", "--quiet", "main");
      write("other.txt", "two\n");
      git("commit", "--quiet", "-am", "Main moves");
      git("checkout", "--quiet", "issue");
      git("merge", "--quiet", "--no-commit", "main");
      write("other.txt", "two\nleaked = s3cr3t-in-a-merge\n");
      git("add", ".");
      git("commit", "--quiet", "-m", "Merge main");

      const text = publishedText(repo, base, git("rev-parse", "HEAD"));
      assert.match(text, /s3cr3t-in-a-merge/);
      assert.match(text, /line 2/);
      assert.match(text, /Merge main/);
      assert.doesNotMatch(text, /ghp_FAKE/);
    } finally {
      rmSync(repo, { recursive: true, force: true });
    }
  });
});
