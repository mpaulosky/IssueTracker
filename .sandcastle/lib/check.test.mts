import assert from "node:assert/strict";
import { describe, it } from "node:test";
import { execFileSync } from "node:child_process";
import { mkdirSync, mkdtempSync, rmSync, writeFileSync } from "node:fs";
import { tmpdir } from "node:os";
import { join } from "node:path";
import { checkFileChanges, fenced, runCheck, tail } from "./check.mts";

// These tests make their own repositories. A git hook (pre-push runs these
// tests) can export GIT_DIR and friends, which would point git at this clone.
for (const key of ["GIT_DIR", "GIT_WORK_TREE", "GIT_INDEX_FILE", "GIT_PREFIX", "GIT_COMMON_DIR"]) delete process.env[key];

// A sandbox whose exec answers each command from a table.
const sandboxWith = (results: Record<string, { stdout: string; exitCode: number }>) => ({
  exec: async (command: string) => {
    const result = results[command];
    if (!result) throw new Error(`unexpected command: ${command}`);
    return { ...result, stderr: "" };
  },
});

describe("runCheck", () => {
  it("passes a green check over a clean worktree, without colour codes", async () => {
    const result = await runCheck(
      sandboxWith({
        ".sandcastle/check.sh 2>&1": { stdout: "\u001b[32mok\u001b[0m\n", exitCode: 0 },
        "git status --porcelain 2>&1": { stdout: "", exitCode: 0 },
      }),
    );
    assert.deepEqual(result, { passed: true, output: "ok\n" });
  });

  it("fails a red check", async () => {
    const result = await runCheck(sandboxWith({ ".sandcastle/check.sh 2>&1": { stdout: "error CS1002", exitCode: 1 } }));
    assert.equal(result.passed, false);
    assert.equal(result.output, "error CS1002");
  });

  it("fails a green check over uncommitted changes", async () => {
    const result = await runCheck(
      sandboxWith({
        ".sandcastle/check.sh 2>&1": { stdout: "ok", exitCode: 0 },
        "git status --porcelain 2>&1": { stdout: " M src/UI/Program.cs\n", exitCode: 0 },
      }),
    );
    assert.equal(result.passed, false);
    assert.match(result.output, /uncommitted changes/);
  });
});

describe("tail", () => {
  it("keeps the last lines", () => {
    assert.equal(tail("a\nb\nc\n", 2), "b\nc");
  });
});

describe("fenced", () => {
  it("uses a fence longer than any backtick run inside", () => {
    assert.equal(fenced("x ```` y"), "`````text\nx ```` y\n`````");
    assert.equal(fenced("plain"), "```text\nplain\n```");
  });
});

describe("checkFileChanges", () => {
  it("names each change that decides what the check runs, and nothing else", () => {
    const repo = mkdtempSync(join(tmpdir(), "sandcastle-check-files-"));
    try {
      const env = { ...process.env, GIT_AUTHOR_NAME: "t", GIT_AUTHOR_EMAIL: "t@t", GIT_COMMITTER_NAME: "t", GIT_COMMITTER_EMAIL: "t@t" };
      const git = (...args: string[]) => execFileSync("git", args, { cwd: repo, env, encoding: "utf8" }).trim();
      const write = (file: string, text: string) => {
        mkdirSync(join(repo, file, ".."), { recursive: true });
        writeFileSync(join(repo, file), text);
      };
      git("init", "--quiet");
      write("tests/A/A.csproj", "<Project>\n</Project>\n");
      write("tests/B/B.csproj", "<Project>\n</Project>\n");
      write("tests/C/C.csproj", "<Project>\n</Project>\n");
      write("tests/D/D.csproj", "<Project>\n</Project>\n");
      write("tests/D/FooTests.cs", "class FooTests {}\n");
      write("tests/F/F.csproj", "<Project>\n<PropertyGroup>\n<IsTestProject>true</IsTestProject>\n</PropertyGroup>\n</Project>\n");
      write("src/E/E.csproj", "<Project>\n</Project>\n");
      write("tests/Directory.Build.props", "<Project>\n</Project>\n");
      write(".sandcastle/check.sh", "echo check\n");
      git("add", ".");
      git("commit", "--quiet", "-m", "base");
      const base = git("rev-parse", "HEAD");

      write("tests/A/A.csproj", '<Project>\n<PackageReference Include="Testcontainers" />\n</Project>\n');
      write("tests/B/B.csproj", "<Project>\n<IsTestProject>false</IsTestProject>\n</Project>\n");
      rmSync(join(repo, "tests/C"), { recursive: true });
      git("mv", "tests/D/FooTests.cs", "tests/D/FooTests.cs.bak");
      // Commented out on lines of their own: no line the pattern matches changes.
      write("tests/F/F.csproj", "<Project>\n<PropertyGroup>\n<!--\n<IsTestProject>true</IsTestProject>\n-->\n</PropertyGroup>\n</Project>\n");
      write("Directory.Build.props", "<Project>\n</Project>\n");
      // Nested Directory.Build files: a new one, and a whitespace-only edit.
      write("src/X/Directory.Build.targets", "<Project>\n</Project>\n");
      write("tests/Directory.Build.props", "<Project>\n\n</Project>\n");
      write("src/E/E.csproj", '<Project>\n<PackageReference Include="Radzen.Blazor" />\n</Project>\n');
      write(".sandcastle/check.sh", "exit 0\n");
      git("add", "-A");
      git("commit", "--quiet", "-m", "branch");

      const expected = [
        ".sandcastle/check.sh",
        "Directory.Build.props",
        "src/X/Directory.Build.targets",
        "tests/A/A.csproj",
        "tests/B/B.csproj",
        "tests/C/C.csproj",
        "tests/D/FooTests.cs",
        "tests/Directory.Build.props",
        "tests/F/F.csproj",
      ];
      assert.deepEqual(checkFileChanges(repo, base), expected);

      // The same under a host's GIT_GLOB_PATHSPECS=1, where a "*" without
      // explicit glob magic stops at "/".
      const saved = process.env.GIT_GLOB_PATHSPECS;
      process.env.GIT_GLOB_PATHSPECS = "1";
      try {
        assert.deepEqual(checkFileChanges(repo, base), expected);
      } finally {
        if (saved === undefined) delete process.env.GIT_GLOB_PATHSPECS;
        else process.env.GIT_GLOB_PATHSPECS = saved;
      }
    } finally {
      rmSync(repo, { recursive: true, force: true });
    }
  });
});
