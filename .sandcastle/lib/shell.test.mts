import assert from "node:assert/strict";
import { execFileSync } from "node:child_process";
import { mkdtempSync, rmSync } from "node:fs";
import { tmpdir } from "node:os";
import { join } from "node:path";
import { after, describe, it } from "node:test";
import { assertGitConfigUnchanged, configChanges, git, GitConfigChangedError, recordGitConfig } from "./shell.mts";

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
