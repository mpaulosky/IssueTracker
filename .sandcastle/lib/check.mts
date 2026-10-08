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
import { BASE_BRANCH } from "./config.mts";
import { git } from "./shell.mts";

// The files that decide what the check runs and skips.
export const CHECK_FILES = [
  ".sandcastle/check.sh",
  ".sandcastle/needs-docker.sh",
  ".github/scripts/discover_tests.py",
  "package.json",
] as const;

// The files the branch changes, since it left the base branch, that decide
// what the check runs: CHECK_FILES, and any MSBuild file where a
// RequiresDocker line was added or removed.
export function checkFileChanges(worktreePath: string): string[] {
  const range = `${BASE_BRANCH}...HEAD`;
  const names = (...args: string[]) => git(worktreePath, "diff", "--no-ext-diff", "--no-textconv", "--name-only", ...args).split("\n").filter(Boolean);
  return [...new Set([...names(range, "--", ...CHECK_FILES), ...names("-G", "RequiresDocker", range, "--", "*.csproj", "*.props", "*.targets")])].sort();
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
