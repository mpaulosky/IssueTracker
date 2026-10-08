#!/usr/bin/env bash
# Whether a test project needs Docker, which Sandcastle's sandbox doesn't have.
# .sandcastle/check.sh skips the projects this says yes to.
#
#   needs-docker.sh <csproj>    exits 0 when it needs Docker, 1 when it doesn't,
#                               2 when the project doesn't exist
#
# Run it from the repo root, with the project's path relative to it.
#
# A test project needs Docker when it, or a project under tests/ it references
# at any depth, uses one of these packages: Testcontainers starts containers,
# Aspire.Hosting.Testing starts the AppHost's, and Playwright drives a browser
# against a running app. So a new project that uses one is skipped without
# anyone editing a list. A project that needs Docker some other way (say,
# Aspire.Hosting's own StartAsync) sets <RequiresDocker>true</RequiresDocker>.
# Tests: .sandcastle/lib/needs-docker.test.mts.
set -euo pipefail

DOCKER_MARKERS='<PackageReference[^>]+Include="(Testcontainers[^"]*|Aspire\.Hosting\.Testing|Microsoft\.Playwright[^"]*)"|<RequiresDocker>[[:space:]]*true[[:space:]]*</RequiresDocker>'

# needs_docker <csproj> [visited...]: true when the project, or a tests/
# project it references directly or indirectly, matches DOCKER_MARKERS.
# Newlines are folded and XML comments dropped first, so an element split
# across lines matches and a commented-out one doesn't.
needs_docker() {
  local csproj="$1" text dir reference path
  shift
  text="$(tr '\n\r' '  ' < "$csproj" | sed -E 's/<!--([^-]|-[^-])*-->//g')"
  grep -qiE "$DOCKER_MARKERS" <<< "$text" && return 0
  dir="$(dirname "$csproj")"
  while read -r reference; do
    path="$(realpath -m --relative-to=. "$dir/${reference//\\//}")"
    [[ "$path" == tests/* && -f "$path" ]] || continue
    [[ " $* " == *" $path "* ]] && continue
    needs_docker "$path" "$csproj" "$@" && return 0
  done < <(grep -oE '<ProjectReference[^>]+Include="[^"]+"' <<< "$text" | sed -E 's/.*Include="([^"]+)"/\1/' || true)
  return 1
}

csproj="${1:?usage: needs-docker.sh <csproj>}"
if [[ ! -f "$csproj" ]]; then
  echo "needs-docker.sh: no such project: $csproj" >&2
  exit 2
fi
needs_docker "$csproj"
