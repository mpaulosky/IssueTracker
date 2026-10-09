// The host's check of an issue branch: .sandcastle/check.sh, run inside the
// sandbox. Its exit code decides whether the branch can be published, never
// what an agent says about it.
//
// The check runs the branch's own copy of the files that define it, and an
// agent can change those: drop a step, or mark a failing test project as
// needing Docker so it's skipped. The host can't stop that without refusing
// legitimate changes to them, so it names every such change in the PR, for
// the person who reviews it. CI runs everything regardless.

import type { Sandbox } from "@ai-hero/sandcastle";
import { git } from "./shell.mts";

// The files that decide what the check runs and skips.
export const CHECK_FILES = [
  ".sandcastle/check.sh",
  ".sandcastle/needs-docker.sh",
  ".github/scripts/discover_tests.py",
  "package.json",
] as const;

// MSBuild lines that decide whether a project is a test project and whether
// check.sh runs it: needs-docker.sh skips a project with RequiresDocker or a
// Docker-backed package (directly or through a tests/ project it references),
// and discover_tests.py drops one whose IsTestProject isn't true.
export const CHECK_MSBUILD_PATTERN = "RequiresDocker|IsTestProject|Testcontainers|Aspire\\.Hosting\\.Testing|Playwright|ProjectReference";

// The files the branch changes, since it left `base`, that decide what the
// check runs: CHECK_FILES, MSBuild files where a CHECK_MSBUILD_PATTERN line
// was added or removed, and files deleted or renamed away under tests/ (with
// renames off, a rename is a deletion and an addition). `base` is a commit
// id, not origin/main, which the agents could move.
export function checkFileChanges(worktreePath: string, base: string): string[] {
  const range = `${base}...HEAD`;
  const names = (...args: string[]) =>
    git(worktreePath, "diff", "--no-ext-diff", "--no-textconv", "--name-only", ...args).split("\n").filter(Boolean);
  return [
    ...new Set([
      ...names(range, "--", ...CHECK_FILES),
      ...names("-G", CHECK_MSBUILD_PATTERN, range, "--", "*.csproj", "*.props", "*.targets"),
      ...names("--no-renames", "--diff-filter=D", range, "--", "tests/"),
    ]),
  ].sort();
}

export type CheckRun = { passed: boolean; output: string };

// The check prints in colour, which only gets in the way of an issue comment.
const ansiEscape = /\u001b\[[0-9;]*[A-Za-z]/g;

// Run the check with stderr folded into stdout, so the output keeps the order
// it was printed in. A passing check over a dirty worktree still fails: only
// commits are pushed, so uncommitted edits would be checked but never published.
export async function runCheck(sandbox: Pick<Sandbox, "exec">): Promise<CheckRun> {
  const { stdout, exitCode } = await sandbox.exec(".sandcastle/check.sh 2>&1");
  const output = stdout.replace(ansiEscape, "");
  if (exitCode !== 0) return { passed: false, output };

  const status = await sandbox.exec("git status --porcelain 2>&1");
  if (status.exitCode !== 0) {
    return { passed: false, output: `${output}\ngit status failed, so the worktree can't be shown to be clean:\n${status.stdout}` };
  }
  if (status.stdout.trim()) {
    return { passed: false, output: `${output}\nThe check passed, but the worktree has uncommitted changes:\n${status.stdout}` };
  }
  return { passed: true, output };
}

// The last `lines` lines of the output.
export function tail(output: string, lines: number): string {
  return output.replace(/\n$/, "").split("\n").slice(-lines).join("\n");
}

// Text quoted in a fence longer than any run of backticks inside it.
export function fenced(text: string): string {
  const longestRun = Math.max(0, ...[...text.matchAll(/`+/g)].map((match) => match[0].length));
  const fence = "`".repeat(Math.max(3, longestRun + 1));
  return `${fence}text\n${text}\n${fence}`;
}
