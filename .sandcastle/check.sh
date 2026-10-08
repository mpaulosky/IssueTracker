#!/usr/bin/env bash
# The check the Sandcastle prompts tell the implementer, reviewer and merger
# to run, and to fix until it exits 0. Nothing on the host runs it yet, so it
# is advisory: the pre-push gate and CI are what hold a broken branch back.
# It lints the YAML and shell files, builds the solution as CI does, runs every
# test project but the ones that need Docker, then the Sandcastle tests. The
# sandbox image (.sandcastle/Dockerfile) installs the linters at CI's versions.
#
# The sandbox has no Docker on purpose: the host's Docker socket is root on
# the host, and the agents read public issue content. The pre-push gate
# (scripts/gate.sh) runs the Docker-backed test projects when a branch is
# pushed, and CI runs everything.
set -euo pipefail

cd "$(git rev-parse --show-toplevel)"

# A test project needs Docker when it, or a project under tests/ it references
# at any depth, uses one of these packages: Testcontainers starts containers,
# Aspire.Hosting.Testing starts the AppHost's, and Playwright drives a browser
# against a running app. A new project that uses one of them is skipped here
# without anyone editing a list. A project that needs Docker some other way
# (say, Aspire.Hosting's own StartAsync) sets <RequiresDocker>true</RequiresDocker>
# in its .csproj to be skipped too.
DOCKER_MARKERS='<PackageReference[^>]+Include="(Testcontainers[^"]*|Aspire\.Hosting\.Testing|Microsoft\.Playwright[^"]*)"|<RequiresDocker>[[:space:]]*true[[:space:]]*</RequiresDocker>'

# needs_docker <csproj> [visited...]: true when the project, or a tests/
# project it references directly or indirectly, matches DOCKER_MARKERS.
# Newlines are folded first, so an element split across lines still matches.
needs_docker() {
  local csproj="$1" text dir reference path
  shift
  text="$(tr '\n\r' '  ' < "$csproj")"
  grep -qiE "$DOCKER_MARKERS" <<< "$text" && return 0
  dir="$(dirname "$csproj")"
  while read -r reference; do
    path="$(realpath -m --relative-to=. "$dir/${reference//\\//}")"
    [[ "$path" == tests/* && -f "$path" ]] || continue
    [[ " $* " == *" $path "* ]] && continue
    needs_docker "$path" "$csproj" "$@" && return 0
  done < <(grep -oE '<ProjectReference[^>]+Include="[^"]+"' <<< "$text" | sed -E 's/.*Include="([^"]+)"/\1/')
  return 1
}

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
  if needs_docker "$project"; then
    echo "Skipping ${project}: it needs Docker. The pre-push gate and CI run it."
    continue
  fi
  dotnet test "$project" --configuration Release
done

echo "▶ Sandcastle tests"
pnpm run test:sandcastle

echo "✅ Sandcastle check passed."
