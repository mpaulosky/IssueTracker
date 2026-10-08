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

# The tests import the packages package.json lists (@ai-hero/sandcastle, zod)
# and the script type-checks with typescript, so install exactly what
# pnpm-lock.yaml pins first. pnpm comes from the PATH, or from corepack at the
# version package.json's "packageManager" pins; Node 25 and later ship no
# corepack, so there pnpm has to be on the PATH.
if command -v pnpm >/dev/null 2>&1; then
  pnpm=(pnpm)
elif command -v corepack >/dev/null 2>&1; then
  export COREPACK_ENABLE_DOWNLOAD_PROMPT=0
  pnpm=(corepack pnpm)
else
  echo "The Sandcastle tests need pnpm, or corepack to provide it; this Node $(node --version) has neither." >&2
  exit 1
fi
echo "Sandcastle tests"
"${pnpm[@]}" install --frozen-lockfile
"${pnpm[@]}" run test:sandcastle
