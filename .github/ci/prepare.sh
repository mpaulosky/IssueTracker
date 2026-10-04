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

case "$job" in
  build) ;;
  test)
    # Required by AppHost's AddConnectionString("mongodb") even though the
    # web app ignores it in Testing mode. ci.yml used to set it on every test
    # job, so it still applies to every test project.
    echo "ConnectionStrings__mongodb=mongodb://localhost:27017" >> "${GITHUB_ENV:-/dev/null}"

    # AppHost.Tests starts the whole Aspire app, which serves the UI's Blazor
    # static assets and pulls the Mongo and Redis containers.
    if [[ "$test_name" == "AppHost.Tests" ]]; then
      dotnet publish src/UI/IssueTracker.UI/IssueTracker.UI.csproj \
        --configuration Release --no-restore
      docker pull mongo:8.2 &
      docker pull redis:8.6 &
      wait
    fi
    ;;
  *) echo "prepare.sh: unknown job '$job'" >&2; exit 2 ;;
esac
