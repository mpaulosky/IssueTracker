import json

import release_queue as rq


def pull(number, merged_at, title=None, sha=None):
    return {
        "number": number,
        "title": title or f"feat: PR {number}",
        "merged_at": merged_at,
        "merge_commit_sha": sha or f"sha{number}",
    }


def never_contained(sha):
    return False


def test_unreleased_prs_are_queued_oldest_merge_first():
    pulls = [
        pull(12, "2026-09-29T10:02:00Z"),
        pull(10, "2026-09-29T10:00:00Z"),
        pull(11, "2026-09-29T10:01:00Z"),
    ]

    assert rq.queue(pulls, released=set(), in_latest_release=never_contained, has_release_tag=True) == [10, 11, 12]


def test_released_and_skip_release_prs_are_left_out():
    pulls = [
        pull(10, "2026-09-29T10:00:00Z"),
        pull(11, "2026-09-29T10:01:00Z", title="docs: add release blog for PR #10 [skip-release]"),
        pull(12, "2026-09-29T10:02:00Z"),
    ]

    assert rq.queue(pulls, released={10}, in_latest_release=never_contained, has_release_tag=True) == [12]


def test_prs_already_inside_the_newest_release_tag_are_left_out():
    # PRs merged before the newest release are history, even without a Release of their own
    # (for example, merged before release automation existed).
    pulls = [pull(1, "2026-01-01T00:00:00Z"), pull(20, "2026-09-29T10:00:00Z")]

    queued = rq.queue(pulls, released=set(), in_latest_release=lambda sha: sha == "sha1", has_release_tag=True)

    assert queued == [20]


def test_without_any_release_tag_only_the_triggering_pr_is_queued():
    pulls = [pull(1, "2026-01-01T00:00:00Z"), pull(2, "2026-01-02T00:00:00Z")]

    queued = rq.queue(pulls, released=set(), in_latest_release=never_contained, has_release_tag=False, trigger=2)

    assert queued == [2]


def test_the_triggering_pr_is_queued_even_when_the_listing_missed_it():
    pulls = [pull(10, "2026-09-29T10:00:00Z")]
    trigger = pull(11, "2026-09-29T10:01:00Z")

    queued = rq.queue(
        pulls, released=set(), in_latest_release=never_contained, has_release_tag=True, trigger=11,
        trigger_pull=trigger,
    )

    assert queued == [10, 11]


def test_a_released_triggering_pr_is_not_queued_again():
    pulls = [pull(10, "2026-09-29T10:00:00Z")]

    queued = rq.queue(pulls, released={10}, in_latest_release=never_contained, has_release_tag=True, trigger=10)

    assert queued == []


def test_unmerged_prs_are_left_out():
    pulls = [pull(10, None), pull(11, "2026-09-29T10:01:00Z")]

    assert rq.queue(pulls, released=set(), in_latest_release=never_contained, has_release_tag=True) == [11]


class FakeGitHub:
    def __init__(self, pulls, releases):
        self._pulls = pulls
        self._releases = releases

    def merged_pulls(self):
        return self._pulls

    def pull(self, number):
        return next(p for p in self._pulls if p["number"] == number)

    def releases(self):
        return self._releases


def test_main_prints_the_queue_as_json(capsys):
    gh = FakeGitHub(
        pulls=[pull(10, "2026-09-29T10:00:00Z"), pull(11, "2026-09-29T10:01:00Z")],
        releases=[{"tag_name": "v0.0.9", "body": "Source PR: #9\n"}],
    )

    rq.main(["--repo", "octo/demo", "--pr", "11"], gh=gh, latest_tag="v0.0.9", contains=lambda tag, sha: False)

    assert json.loads(capsys.readouterr().out) == [10, 11]
