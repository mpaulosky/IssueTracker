import assert from "node:assert/strict";
import { execFileSync } from "node:child_process";
import { mkdirSync, mkdtempSync, rmSync, writeFileSync } from "node:fs";
import { tmpdir } from "node:os";
import { join } from "node:path";
import { describe, it } from "node:test";
import { checkFileChanges } from "./check.mts";
import { addedLines, publishedText } from "./scan.mts";
import { protectHostGit } from "./shell.mts";

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
    assert.deepEqual(addedLines(patch), ["diff --git a/f b/f", "--- a/f", "+++ b/f", "new", "diff --cc g", "+++ b/g", "evil", "ours", "theirs"]);
  });

  it("keeps an added line that looks like a file header", () => {
    const patch = ["diff --git a/f b/f", "--- a/f", "+++ b/f", "@@ -0,0 +1 @@", "+++ sk-ant-api03-token", "diff --cc g", "+++ b/g", "@@@ -1 -1 +1 @@@", "+++ ghp_token"].join("\n");
    assert.deepEqual(addedLines(patch).filter((line) => line.includes("token")), ["++ sk-ant-api03-token", "+ ghp_token"]);
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

      // A user's color.ui=always mustn't blind the scan.
      git("config", "color.ui", "always");
      const text = publishedText(repo, base, git("rev-parse", "HEAD"));
      assert.match(text, /s3cr3t-in-a-merge/);
      assert.match(text, /line 2/);
      assert.match(text, /Merge main/);
      assert.doesNotMatch(text, /ghp_FAKE/);
    } finally {
      rmSync(repo, { recursive: true, force: true });
    }
  });

  it("includes the commit headers a push publishes: author and committer names and emails", () => {
    const repo = mkdtempSync(join(tmpdir(), "sandcastle-scan-"));
    try {
      const env = { ...process.env, GIT_AUTHOR_NAME: "t", GIT_AUTHOR_EMAIL: "t@t", GIT_COMMITTER_NAME: "t", GIT_COMMITTER_EMAIL: "t@t" };
      const git = (...args: string[]) => execFileSync("git", args, { cwd: repo, env, encoding: "utf8" }).trim();

      git("init", "--quiet", "-b", "main");
      git("commit", "--quiet", "--allow-empty", "-m", "base");
      const base = git("rev-parse", "HEAD");
      // The environment doesn't touch .git/config, so the config guard can't
      // see these.
      const commit = (message: string, extra: NodeJS.ProcessEnv) =>
        execFileSync("git", ["commit", "--quiet", "--allow-empty", "-m", message], { cwd: repo, env: { ...env, ...extra } });
      commit("One", { GIT_AUTHOR_NAME: "sk-ant-api03-in-the-author-name" });
      commit("Two", { GIT_COMMITTER_EMAIL: "ghp_in-the-committer-email@example.com" });

      const text = publishedText(repo, base, git("rev-parse", "HEAD"));
      assert.match(text, /sk-ant-api03-in-the-author-name/);
      assert.match(text, /ghp_in-the-committer-email/);
    } finally {
      rmSync(repo, { recursive: true, force: true });
    }
  });
});

// core.useReplaceRefs=false, which the host's git helper passes and
// protectHostGit puts in the environment, so a replace ref in the writable
// .git can't show the scan a harmless commit while the push sends the real one.
describe("replace refs", () => {
  it("makes the secret scan and the check-file warning read a commit a replace ref hides", () => {
    const repo = mkdtempSync(join(tmpdir(), "sandcastle-replace-test-"));
    try {
      const plain = { ...process.env, GIT_AUTHOR_NAME: "t", GIT_AUTHOR_EMAIL: "t@t", GIT_COMMITTER_NAME: "t", GIT_COMMITTER_EMAIL: "t@t" };
      const run = (env: NodeJS.ProcessEnv, ...args: string[]) => execFileSync("git", args, { cwd: repo, env, encoding: "utf8" }).trim();
      const write = (file: string, text: string) => {
        mkdirSync(join(repo, file, ".."), { recursive: true });
        writeFileSync(join(repo, file), text);
      };
      run(plain, "init", "--quiet", "-b", "main");
      write("readme.txt", "hello\n");
      run(plain, "add", ".");
      run(plain, "commit", "--quiet", "-m", "base");
      const base = run(plain, "rev-parse", "HEAD");

      // A harmless commit, then the real one, which the agents hide behind it.
      write("readme.txt", "hello, world\n");
      run(plain, "commit", "--quiet", "-am", "Harmless");
      const harmless = run(plain, "rev-parse", "HEAD");
      run(plain, "reset", "--quiet", "--hard", base);
      write("readme.txt", "token = sk-ant-api03-hidden-by-a-replace-ref\n");
      write(".sandcastle/check.sh", "exit 0\n");
      run(plain, "add", ".");
      run(plain, "commit", "--quiet", "-m", "Real");
      const real = run(plain, "rev-parse", "HEAD");
      run(plain, "replace", real, harmless);

      // The host's helpers, under protectHostGit, read the real commit.
      const saved = { ...process.env };
      protectHostGit(process.env);
      try {
        assert.match(publishedText(repo, base, real), /sk-ant-api03-hidden-by-a-replace-ref/);
        assert.deepEqual(checkFileChanges(repo, base), [".sandcastle/check.sh"]);
      } finally {
        for (const key of Object.keys(process.env)) if (!(key in saved)) delete process.env[key];
        Object.assign(process.env, saved);
      }

      // So does any other git under protectHostGit.
      const protectedEnv = { ...plain };
      protectHostGit(protectedEnv);
      assert.match(run(protectedEnv, "log", "-p", `${base}..${real}`), /sk-ant-api03-hidden-by-a-replace-ref/);

      // The control: a git without it reads the harmless commit.
      assert.doesNotMatch(run(plain, "log", "-p", `${base}..${real}`), /sk-ant-api03-hidden-by-a-replace-ref/);
    } finally {
      rmSync(repo, { recursive: true, force: true });
    }
  });

  it("reads past a replace ref through the host's git helper alone, without protectHostGit", () => {
    const repo = mkdtempSync(join(tmpdir(), "sandcastle-replace-test-"));
    try {
      const plain = { ...process.env, GIT_AUTHOR_NAME: "t", GIT_AUTHOR_EMAIL: "t@t", GIT_COMMITTER_NAME: "t", GIT_COMMITTER_EMAIL: "t@t" };
      const run = (...args: string[]) => execFileSync("git", args, { cwd: repo, env: plain, encoding: "utf8" }).trim();
      run("init", "--quiet", "-b", "main");
      run("commit", "--quiet", "--allow-empty", "-m", "base");
      const base = run("rev-parse", "HEAD");
      run("commit", "--quiet", "--allow-empty", "-m", "Harmless");
      const harmless = run("rev-parse", "HEAD");
      run("reset", "--quiet", "--hard", base);
      run("commit", "--quiet", "--allow-empty", "-m", "ghp_hidden-by-a-replace-ref");
      const real = run("rev-parse", "HEAD");
      run("replace", real, harmless);

      assert.ok(!process.env.GIT_CONFIG_COUNT, "the test environment already sets GIT_CONFIG_*, so it proves nothing");
      assert.match(publishedText(repo, base, real), /ghp_hidden-by-a-replace-ref/);
      // The control: plain git shows the harmless message.
      assert.doesNotMatch(run("log", `${base}..${real}`), /ghp_hidden-by-a-replace-ref/);
    } finally {
      rmSync(repo, { recursive: true, force: true });
    }
  });
});
