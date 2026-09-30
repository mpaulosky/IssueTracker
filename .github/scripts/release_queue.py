#!/usr/bin/env python3
"""Print, as a JSON list, the merged PRs the release workflow still has to release.

Called by .github/workflows/release.yml before it releases anything:

    python3 .github/scripts/release_queue.py --repo owner/name [--pr 42]

Every release run shares one concurrency group, and GitHub keeps only the
newest pending run in a group, cancelling the one it replaces. So a run
doesn't release only the PR that started it: it releases every merged PR
that is still owed a release, oldest merge first. A PR whose run was
cancelled is picked up by the next one, and versions follow merge order.

A PR is owed a release when it merged into main, its title has no
[skip-release] marker, no GitHub Release names it ("Source PR: #n"), and its
merge commit isn't already inside the newest release tag. The last rule
keeps PRs merged before release automation existed out of the queue. With no
release tag at all, only the triggering PR is queued.

Standard library only, like release_post.py; git and the gh CLI do the rest.
"""

import argparse
import json
import re
import subprocess
import sys

import release_post as rp

# Merged PRs to look back through. Far more than can pile up between runs.
LOOKBACK = 50

SKIP_MARKER = "[skip-release]"


class GitHub(rp.GitHub):
    """release_post's gh wrapper, plus the recently merged PRs."""

    def merged_pulls(self):
        # One page is enough, so no --paginate: closed PRs, most recently updated first.
        path = f"repos/{self.repository}/pulls?state=closed&base=main&sort=updated&direction=desc&per_page={LOOKBACK}"
        output = subprocess.run(["gh", "api", path], check=True, capture_output=True, text=True).stdout
        return [
            {key: pr.get(key) for key in ("number", "title", "merged_at", "merge_commit_sha")}
            for pr in json.loads(output)
        ]


def latest_release_tag():
    """The highest vMAJOR.MINOR.PATCH tag, or None when there is none."""
    output = subprocess.run(["git", "tag", "--list", "v[0-9]*"], check=True, capture_output=True, text=True).stdout
    tags = [tag for tag in output.split() if re.fullmatch(r"v\d+\.\d+\.\d+", tag)]
    return max(tags, key=rp.version_key) if tags else None


def tag_contains(tag, sha):
    """Whether the commit is the tag's commit or one of its ancestors."""
    result = subprocess.run(["git", "merge-base", "--is-ancestor", sha, tag], capture_output=True)
    return result.returncode == 0


def owed(pr, released, in_latest_release):
    return (
        bool(pr.get("merged_at"))
        and SKIP_MARKER not in (pr.get("title") or "")
        and pr["number"] not in released
        and not in_latest_release(pr.get("merge_commit_sha") or "")
    )


def queue(pulls, released, in_latest_release, has_release_tag, trigger=None, trigger_pull=None):
    """The PR numbers owed a release, oldest merge first."""
    by_number = {pr["number"]: pr for pr in pulls}
    if trigger is not None and trigger not in by_number and trigger_pull is not None:
        by_number[trigger] = trigger_pull
    if not has_release_tag:
        # A first release: don't sweep up history, just release what triggered the run.
        by_number = {trigger: by_number[trigger]} if trigger in by_number else {}
    pending = [pr for pr in by_number.values() if owed(pr, released, in_latest_release)]
    return [pr["number"] for pr in sorted(pending, key=lambda pr: pr["merged_at"])]


def main(argv=None, gh=None, latest_tag=None, contains=tag_contains):
    parser = argparse.ArgumentParser(description=__doc__.splitlines()[0])
    parser.add_argument("--repo", required=True, help="owner/name")
    parser.add_argument("--pr", type=int, help="the PR that triggered this run")
    args = parser.parse_args(argv)

    gh = gh or GitHub(args.repo)
    latest_tag = latest_tag if latest_tag is not None else latest_release_tag()
    released = {n for n in (rp.source_pr_of(r) for r in gh.releases()) if n is not None}
    pulls = gh.merged_pulls()
    trigger_pull = None
    if args.pr is not None and all(pr["number"] != args.pr for pr in pulls):
        trigger_pull = gh.pull(args.pr)

    result = queue(
        pulls,
        released,
        in_latest_release=lambda sha: bool(latest_tag) and bool(sha) and contains(latest_tag, sha),
        has_release_tag=bool(latest_tag),
        trigger=args.pr,
        trigger_pull=trigger_pull,
    )
    # stdout carries only the JSON the workflow reads; the explanation goes to stderr.
    print(f"Newest release tag: {latest_tag or 'none'}. Queue: {result or 'empty'}.", file=sys.stderr)
    print(json.dumps(result))
    return result


if __name__ == "__main__":
    main()
