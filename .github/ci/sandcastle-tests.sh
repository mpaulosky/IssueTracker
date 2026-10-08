#!/usr/bin/env bash
# Runs Sandcastle's tests (pnpm's test:sandcastle script) when anything they
# cover changed since <base>, or always when <base> is empty. The pre-push
# gate (.github/ci/gate-checks.sh) and CI's Build Solution job
# (.github/ci/prepare.sh build) both call this, so they watch the same paths.
#
#   sandcastle-tests.sh <base>
set -euo pipefail

base="${1-}"

# .sandcastle/ itself, the root package files, the scripts its tests run
# (branches.test.mts checks every branch name against check-branch-name.sh),
# and this script and its callers, so a change to how the tests run runs them.
paths=(
  .sandcastle package.json pnpm-lock.yaml scripts/check-branch-name.sh
  .github/ci/sandcastle-tests.sh .github/ci/gate-checks.sh .github/ci/prepare.sh
)

if [[ -n "$base" ]] && git diff --quiet --no-renames "$base" HEAD -- "${paths[@]}"; then
  echo "No changes the Sandcastle tests cover."
  exit 0
fi

# node --test strips the .mts files' types only from Node 22.18. CI's ci.yml
# (Owned by the Template) sets up no Node, so CI uses the runner image's.
if ! node -e 'const [major, minor] = process.versions.node.split(".").map(Number); process.exit(major > 22 || (major === 22 && minor >= 18) ? 0 : 1)' 2>/dev/null; then
  echo "The Sandcastle tests need Node 22.18 or later; this has $(node --version 2>/dev/null || echo no Node)." >&2
  echo "In CI, add a pinned actions/setup-node to the repo-ci-baseline Template's ci.yml." >&2
  exit 1
fi

# node --run runs the package.json script without pnpm, so a Node without
# corepack (25 and later) still works. The tests import only node: builtins
# and each other; one that imports a package would need
# "pnpm install --frozen-lockfile" here first.
echo "Sandcastle tests"
node --run test:sandcastle
