// The pull request the host opens for a finished issue. Its title and body
// follow docs/PROCESS.md, so the required PR title check passes and the
// description reads like any other.

import type { SandcastleIssue } from "./github.mts";

const conventionalTitle = /^(feat|fix|docs|style|refactor|perf|test|build|ci|chore|revert)(\([^)]+\))?!?: \S/;

// The issue title when it's already in commit format, otherwise the title
// behind `fix: ` for a bug or `feat: ` for anything else. The summary starts
// with a capital and has no closing period, as scripts/check-pr-title.sh wants.
export function prTitle(issue: Pick<SandcastleIssue, "title" | "labels">): string {
  const title = issue.title.trim().replace(/\s+/g, " ");
  const match = conventionalTitle.exec(title);
  const prefix = match ? title.slice(0, match[0].length - 1) : `${issue.labels.includes("bug") ? "fix" : "feat"}: `;
  // Drop closing periods and any spaces they leave behind ("Add search ." ends
  // up "Add search"), since the title check refuses both.
  const summary = (match ? title.slice(match[0].length - 1) : title).replace(/[.\s]+$/, "") || "Resolve the issue";
  return `${prefix}${summary.charAt(0).toUpperCase()}${summary.slice(1)}`;
}

// GitHub closes an issue when a merged PR's description says "fixes #12",
// "Closes owner/repo#12" and so on. The reviewer's summary is model text that
// may mention other issues that way, so its references after a closing
// keyword are rewritten as "issue 12", which GitHub doesn't act on. Only the
// "Fixes #n" line the host adds closes anything.
const closingReference = /\b(close[sd]?|fix(?:e[sd])?|resolve[sd]?)(\s*:?\s+)([\w.-]+\/[\w.-]+)?#(\d+)/gi;

export function withoutClosingKeywords(text: string): string {
  return text.replace(closingReference, (_, keyword: string, gap: string, repo: string | undefined, number: string) =>
    `${keyword}${gap}${repo ? `${repo} ` : ""}issue ${number}`,
  );
}

// checkFiles: the files this branch changes that decide what .sandcastle/check.sh
// runs (see checkFileChanges), named in the PR so the reviewer knows the
// check that passed isn't main's.
export function prBody(
  issue: Pick<SandcastleIssue, "number" | "title">,
  reviewSummary: string,
  checkFiles: readonly string[] = [],
): string {
  const checkWarning =
    checkFiles.length === 0
      ? []
      : [
          "> [!WARNING]",
          "> This branch changes files that decide what `.sandcastle/check.sh` runs or skips, so the check that passed",
          "> in the sandbox isn't the one on `main`. Review these changes before trusting it:",
          ...checkFiles.map((file) => `> - \`${file}\``),
          "",
        ];
  return [
    ...checkWarning,
    "## Why",
    "",
    `Issue #${issue.number}: ${issue.title}`,
    "",
    "## What changed",
    "",
    "Sandcastle's implementer built the change in a sandbox, and its reviewer approved it:",
    "",
    ...withoutClosingKeywords(reviewSummary).split("\n").map((line) => `> ${line}`),
    "",
    "## Verification",
    "",
    "- `.sandcastle/check.sh` (the lints, the Release build and the test projects that don't need Docker) passed in the",
    "  sandbox, after the branch merged `origin/main`.",
    "- CI runs the full suite, including the Docker-backed tests. The host pushes without running the pre-push hook,",
    "  because that hook lives in the worktree the agents edited.",
    "",
    `Fixes #${issue.number}`,
  ].join("\n");
}
