#!/usr/bin/env bash
# The check the Sandcastle prompts tell the implementer and reviewer to run,
# and to fix until it exits 0. The host runs it too (.sandcastle/lib/check.mts)
# and publishes a branch only when it passes: its exit code decides, never what
# an agent reports. CI runs everything, the Docker-backed tests included.
# It lints the YAML and shell files, builds the solution as CI does, runs every
# test project but the ones that need Docker, then the Sandcastle tests. The
# sandbox image (.sandcastle/Dockerfile) installs the linters at CI's versions.
#
# The sandbox has no Docker on purpose: the host's Docker socket is root on
# the host, and the agents read public issue content. CI runs the
# Docker-backed test projects on the branch's pull request.
set -euo pipefail

cd "$(git rev-parse --show-toplevel)"

echo "▶ YAML lint"
# Every tracked YAML file, as CI's Lint YAML workflow does. pnpm writes its
# lockfile in its own style; it isn't hand-edited.
mapfile -t yaml_files < <(git ls-files '*.yml' '*.yaml' | grep -Ev '(^|/)pnpm-lock\.yaml$' || true)
if [[ ${#yaml_files[@]} -gt 0 ]]; then
  yamllint -c .yamllint.yml "${yaml_files[@]}"
fi

echo "▶ Shell lint"
# The files CI's shellcheck job checks: shell scripts, extensionless scripts
# and git hooks.
mapfile -t shell_files < <(git ls-files | grep -E '\.sh$|^scripts/[^/.]+$|^\.github/hooks/((pre|post)-[a-z-]+|(prepare-)?commit-msg)$' || true)
if [[ ${#shell_files[@]} -gt 0 ]]; then
  shellcheck "${shell_files[@]}"
fi

echo "▶ Build"
dotnet build IssueTracker.slnx --configuration Release -warnaserror

echo "▶ Tests (without Docker)"
# CI's discovery: every project under tests/ that resolves IsTestProject to true.
mapfile -t projects < <(python3 .github/scripts/discover_tests.py --list | grep . || true)
if [[ ${#projects[@]} -eq 0 ]]; then
  echo "discover_tests.py found no test projects under tests/." >&2
  exit 1
fi
for project in "${projects[@]}"; do
  # .sandcastle/needs-docker.sh says which: those using Testcontainers,
  # Aspire.Hosting.Testing or Playwright, or marked RequiresDocker.
  status=0
  .sandcastle/needs-docker.sh "$project" || status=$?
  case "$status" in
    0)
      echo "Skipping ${project}: it needs Docker. The pre-push gate and CI run it."
      continue
      ;;
    1) ;;
    *) exit "$status" ;;
  esac
  dotnet test "$project" --configuration Release
done

echo "▶ Sandcastle tests"
pnpm run test:sandcastle

echo "✅ Sandcastle check passed."
