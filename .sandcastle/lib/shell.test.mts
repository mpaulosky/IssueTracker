import assert from "node:assert/strict";
import { execFileSync } from "node:child_process";
import { chmodSync, existsSync, mkdirSync, mkdtempSync, rmSync, writeFileSync } from "node:fs";
import { tmpdir } from "node:os";
import { join } from "node:path";
import { after, describe, it } from "node:test";
import { assertGitConfigUnchanged, configChanges, git, GitConfigChangedError, protectHostGit, recordGitConfig } from "./shell.mts";

// These tests make their own repositories. A git hook (pre-push runs these
// tests) can export GIT_DIR and friends, which would point git at this clone.
for (const key of ["GIT_DIR", "GIT_WORK_TREE", "GIT_INDEX_FILE", "GIT_PREFIX", "GIT_COMMON_DIR"]) delete process.env[key];

describe("configChanges", () => {
  it("lists removed and added lines", () => {
    assert.deepEqual(configChanges("core.bare=false\nremote.origin.url=a\n", "core.bare=false\nremote.origin.url=b\n"), [
      "- remote.origin.url=a",
      "+ remote.origin.url=b",
    ]);
  });

  it("finds nothing when the config is the same", () => {
    assert.deepEqual(configChanges("a=1\nb=2\n", "a=1\nb=2\n"), []);
  });
});

describe("the git config guard", () => {
  const repo = mkdtempSync(join(tmpdir(), "sandcastle-shell-test-"));
  const original = process.cwd();
  after(() => {
    process.chdir(original);
    rmSync(repo, { recursive: true, force: true });
  });

  it("refuses host git once an agent has written a command into the clone's config", () => {
    execFileSync("git", ["init", "--quiet", repo]);
    process.chdir(repo);
    recordGitConfig();
    assert.doesNotThrow(() => assertGitConfigUnchanged());
    assert.equal(git(repo, "rev-parse", "--is-inside-work-tree"), "true");

    execFileSync("git", ["config", "core.fsmonitor", "touch pwned"], { cwd: repo });
    assert.throws(() => git(repo, "status"), (error: unknown) => {
      assert.ok(error instanceof GitConfigChangedError);
      assert.match(String(error), /\+ core\.fsmonitor=touch pwned/);
      return true;
    });
  });
});

describe("protectHostGit", () => {
  it("appends after GIT_CONFIG_* entries already in the environment", () => {
    const env: NodeJS.ProcessEnv = { GIT_CONFIG_COUNT: "1", GIT_CONFIG_KEY_0: "user.name", GIT_CONFIG_VALUE_0: "x" };
    protectHostGit(env);
    assert.equal(env.GIT_CONFIG_COUNT, "3");
    assert.equal(env.GIT_CONFIG_KEY_0, "user.name");
    assert.equal(env.GIT_CONFIG_KEY_1, "core.hooksPath");
    assert.equal(env.GIT_CONFIG_VALUE_1, "/dev/null");
    assert.equal(env.GIT_CONFIG_KEY_2, "core.fsmonitor");
  });

  it("keeps a branch's post-checkout hook from running when any git, not just the host's helper, adds a worktree", () => {
    const repo = mkdtempSync(join(tmpdir(), "sandcastle-hooks-test-"));
    try {
      const run = (env: NodeJS.ProcessEnv, ...args: string[]) => execFileSync("git", args, { cwd: repo, env, stdio: "ignore" });
      const plain = { ...process.env, GIT_AUTHOR_NAME: "t", GIT_AUTHOR_EMAIL: "t@t", GIT_COMMITTER_NAME: "t", GIT_COMMITTER_EMAIL: "t@t" };
      run(plain, "init", "--quiet");
      mkdirSync(join(repo, "hooks"));
      writeFileSync(join(repo, "hooks", "post-checkout"), `#!/bin/sh\ntouch "${join(repo, "pwned")}"\n`);
      chmodSync(join(repo, "hooks", "post-checkout"), 0o755);
      run(plain, "config", "core.hooksPath", "hooks");
      run(plain, "add", ".");
      run(plain, "commit", "--quiet", "-m", "hooks");

      const protectedEnv = { ...plain };
      protectHostGit(protectedEnv);
      run(protectedEnv, "worktree", "add", "--quiet", join(repo, "wt-protected"), "-b", "a");
      assert.ok(!existsSync(join(repo, "pwned")), "the hook ran despite protectHostGit");

      // The control: without it, the same command runs the hook.
      run(plain, "worktree", "add", "--quiet", join(repo, "wt-plain"), "-b", "b");
      assert.ok(existsSync(join(repo, "pwned")), "the control hook didn't run, so the test proves nothing");
    } finally {
      rmSync(repo, { recursive: true, force: true });
    }
  });
});
