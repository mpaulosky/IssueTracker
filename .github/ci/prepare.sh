#!/usr/bin/env bash
# Repo-specific CI setup, called by ci.yml after it restores and before it builds.
#
#   prepare.sh build              in the "Build Solution" job
#   prepare.sh test <test-name>   in each "Tests: <test-name>" matrix job
#
# ci.yml is Owned by the repo-ci-baseline Template and is overwritten on every
# Apply; this file is Seed, so it belongs to the repo. Put what only this repo
# needs here: tools the build runs (for example `corepack enable` for pnpm),
# images to pull or projects to publish before a test project runs. To pass
# environment variables to the later steps, append NAME=value lines to
# "$GITHUB_ENV".
#
# The test name comes from a file name in the PR, so treat it as data: quote
# it and never eval it.
set -euo pipefail

job="${1:?usage: prepare.sh build|test [test-name]}"
test_name="${2:-}"

# Sandcastle's tests, when the PR changes anything they cover, as the local
# gate does. Without an origin/main to compare with, run them.
sandcastle_tests() {
  local base=""
  base="$(git merge-base HEAD origin/main 2>/dev/null)" || base=""
  bash .github/ci/sandcastle-tests.sh "$base"
}

case "$job" in
  build) sandcastle_tests ;;
  test)
    : "$test_name"
    # Required by AppHost's AddConnectionString("mongodb") even though the
    # web app ignores it in Testing mode. ci.yml used to set it on every test
    # job, so it still applies to every test project.
    echo "ConnectionStrings__mongodb=mongodb://localhost:27017" >> "${GITHUB_ENV:-/dev/null}"
    ;;
  *) echo "prepare.sh: unknown job '$job'" >&2; exit 2 ;;
esac
