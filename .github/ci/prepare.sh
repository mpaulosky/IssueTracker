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

# Sandcastle's orchestration code (.sandcastle/): run its tests when the PR
# changes it or the root package files, as the local gate's
# .github/ci/gate-checks.sh does. Without an origin/main to compare with, run
# them.
sandcastle_tests() {
  local base
  if base="$(git merge-base HEAD origin/main 2>/dev/null)" \
    && git diff --quiet --no-renames "$base" HEAD -- .sandcastle package.json pnpm-lock.yaml; then
    echo "No Sandcastle or root package changes to test."
    return
  fi
  # ci.yml (Owned by the Template) sets up no Node here, so this uses the
  # runner image's. node --test strips the .mts files' types only from 22.18.
  # node --run runs the package.json script without pnpm, so a runner Node
  # without corepack (25 and later) still works. The tests import only node:
  # builtins and each other; one that imports a package would need
  # "pnpm install --frozen-lockfile" here first.
  if ! node -e 'const [major, minor] = process.versions.node.split(".").map(Number); process.exit(major > 22 || (major === 22 && minor >= 18) ? 0 : 1)' 2>/dev/null; then
    echo "::error::The Sandcastle tests need Node 22.18 or later; the runner has $(node --version 2>/dev/null || echo none). Add a pinned actions/setup-node to the repo-ci-baseline Template's ci.yml." >&2
    return 1
  fi
  node --run test:sandcastle
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
