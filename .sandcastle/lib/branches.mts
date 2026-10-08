// Branch naming and fetching. Branch names come from code, never from a model,
// so re-planning an issue always lands on the branch that holds its earlier
// work, and every name passes scripts/check-branch-name.sh.

import { BASE_BRANCH } from "./config.mts";
import { git, GitConfigChangedError } from "./shell.mts";

const maxSlugLength = 50;

// The prefixes of a branch that names an issue, from docs/PROCESS.md. Sandcastle
// names new branches feature/ or fix/; hotfix/ is for a person's urgent fix,
// recognized so an issue's earlier work on one is still found.
const issuePrefixes = ["feature", "fix", "hotfix"] as const;

// The parts of an issue its branch name depends on.
export type BranchIssue = { number: number; title: string; labels: string[] };

// The issue title as a branch slug: no conventional-commit prefix, lower case,
// apostrophes dropped so "haven't" stays one word, and runs of ASCII letters
// and digits joined with "-", cut to 50 characters at a "-" boundary.
export function slugFor(title: string): string {
  const words = title
    .replace(/^\s*[a-z]+(?:\([^)]*\))?!?:\s*/i, "")
    .toLowerCase()
    .replace(/['’]/g, "")
    .match(/[a-z0-9]+/g) ?? [];
  const slug = words.join("-");
  if (slug.length === 0) return "issue";
  if (slug.length <= maxSlugLength) return slug;

  const cut = slug.slice(0, maxSlugLength + 1);
  const boundary = cut.lastIndexOf("-");
  return boundary > 0 ? cut.slice(0, boundary) : slug.slice(0, maxSlugLength);
}

// Whether a branch belongs to the issue: feature/{n}-*, fix/{n}-* or hotfix/{n}-*.
export function isIssueBranch(branch: string, issueNumber: number): boolean {
  return issuePrefixes.some((prefix) => branch.startsWith(`${prefix}/${issueNumber}-`));
}

// The issue's branch: its existing feature/, fix/ or hotfix/{n}-* branch when
// there is one, even if the title or labels have changed since, so earlier
// work is built on rather than redone. Otherwise fix/{n}-{slug} for a bug and
// feature/{n}-{slug} for everything else.
export function branchFor(issue: BranchIssue, existingBranches: readonly string[]): string {
  const existing = existingBranches
    .filter((branch) => isIssueBranch(branch, issue.number))
    .sort()[0];
  if (existing) return existing;

  const prefix = issue.labels.includes("bug") ? "fix" : "feature";
  return `${prefix}/${issue.number}-${slugFor(issue.title)}`;
}

// Hold back every issue that already has an open pull request from one of its
// branches: its work is waiting for review, and building it again would only
// pile commits onto that PR.
export function withoutOpenPullRequests<T extends BranchIssue>(
  issues: readonly T[],
  openPrBranches: readonly string[],
): { ready: T[]; inReview: T[] } {
  const ready: T[] = [];
  const inReview: T[] = [];
  for (const issue of issues) {
    (openPrBranches.some((branch) => isIssueBranch(branch, issue.number)) ? inReview : ready).push(issue);
  }
  return { ready, inReview };
}

// Branch names from `git ls-remote --heads` output, without refs/heads/.
export function parseHeads(lsRemote: string): string[] {
  return lsRemote
    .split("\n")
    .filter(Boolean)
    .map((line) => line.split("\t")[1]!.replace(/^refs\/heads\//, ""));
}

// The git operations prepareBranches needs; tests pass a stub.
export type BranchGit = {
  // The issue branches on origin.
  remoteIssueBranches(): string[];
  // The issue branches in this clone, including ones a rejected or failed
  // build left unpushed.
  localIssueBranches(): string[];
  // Fetch origin's branch into its remote-tracking ref.
  fetch(branch: string): void;
  // Create the local branch at origin's, or move it up to origin's when that
  // is a fast-forward, so the sandbox starts from the pushed work rather than
  // from main or an older copy.
  syncLocal(branch: string): void;
};

const cloneGit: BranchGit = {
  remoteIssueBranches: () =>
    parseHeads(
      git(process.cwd(), "ls-remote", "--heads", "origin", ...issuePrefixes.map((prefix) => `refs/heads/${prefix}/*`)),
    ),
  localIssueBranches: () =>
    git(process.cwd(), "for-each-ref", "--format=%(refname:short)", ...issuePrefixes.map((prefix) => `refs/heads/${prefix}/`))
      .split("\n")
      .filter(Boolean),
  fetch: (branch) => {
    git(process.cwd(), "fetch", "--quiet", "origin", `+refs/heads/${branch}:refs/remotes/origin/${branch}`);
  },
  syncLocal: (branch) => {
    // `git fetch . a:b` creates b at a, or moves it only when that is a
    // fast-forward, and refuses when b is checked out somewhere. A refusal
    // leaves the local branch as it is, and the push later fails rather than
    // overwrite origin's work.
    try {
      git(process.cwd(), "fetch", "--quiet", ".", `refs/remotes/origin/${branch}:refs/heads/${branch}`);
    } catch (error) {
      if (error instanceof GitConfigChangedError) throw error;
      console.warn(`  Couldn't bring ${branch} up to origin's copy; building on the local branch.`);
    }
  },
};

// Refresh origin/main, the base of every new issue branch, the branch each
// issue merges before it's checked, and what the reviewer's diff compares with.
// The host's git calls are synchronous, so two pipelines never fetch at once.
export function fetchMain(): void {
  git(process.cwd(), "fetch", "--quiet", "origin", "main");
}

// Count the commits on the worktree's branch that the base branch doesn't have.
export function commitsAhead(worktreePath: string): number {
  return Number(git(worktreePath, "rev-list", "--count", `${BASE_BRANCH}..HEAD`));
}

// The commit the worktree has checked out.
export function headOf(worktreePath: string): string {
  return git(worktreePath, "rev-parse", "HEAD");
}

// Name each issue's branch, and fetch the ones that already exist on origin,
// so the sandbox starts from the work already pushed rather than from main.
export function prepareBranches<T extends BranchIssue>(
  issues: readonly T[],
  branchGit: BranchGit = cloneGit,
): { issue: T; branch: string }[] {
  const remote = branchGit.remoteIssueBranches();
  const local = branchGit.localIssueBranches();
  const known = [...new Set([...remote, ...local])];
  return issues.map((issue) => {
    const branch = branchFor(issue, known);
    if (remote.includes(branch)) {
      branchGit.fetch(branch);
      branchGit.syncLocal(branch);
    }
    return { issue, branch };
  });
}
