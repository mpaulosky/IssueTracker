# Project Context

- **Owner:** Jeffrey T. Fritz (csharpfritz@users.noreply.github.com)
- **Project:** Aspire.Hosting.Minecraft — .NET Aspire integration for Minecraft servers
- **Stack:** C#, .NET 10, Docker, Aspire, OpenTelemetry, Minecraft Paper Server, RCON
- **Created:** 2026-02-10

## Key Facts

- Three NuGet packages: Aspire.Hosting.Minecraft, Aspire.Hosting.Minecraft.Rcon, Aspire.Hosting.Minecraft.Worker
- Version 0.1.0, packed via `dotnet pack -o nupkgs`
- GitHub repo: csharpfritz/Aspire-Minecraft
- No CI/CD pipeline exists yet (no .github/workflows/ found)
- .gitattributes configured for merge=union on .ai-team/ files
- MIT licensed

## Learnings

<!-- Append new learnings below. Each entry is something lasting about the project. -->

📌 **Skill Structure Standardization (2026-02-16):** All skills must follow the directory pattern `{skill-name}/SKILL.md`, not loose `.md` files at the skills directory level. Each SKILL.md file must include YAML frontmatter with `name`, `description`, `domain`, `confidence`, and `source` fields. This prevents tool name collisions and maintains consistency across the squad. — decided by Wong

📌 **Tool Name Uniqueness:** Skill names (from YAML frontmatter `name` field) and MCP server names must not overlap. During infrastructure setup, audit all `.ai-team/skills/*/SKILL.md` files and `.copilot/mcp-config.json` for naming conflicts before deploying changes. — decided by Wong

📌 Milestone release changelog template: Section headers (Features Delivered, Issues Resolved, Test Coverage, Breaking Changes), bulleted feature lists with issue references, test metrics summary — use this for future release documentation — decided by Wong

📌 Team update (2026-02-10): NuGet packages blocked — no CI/CD pipeline exists, must be created — decided by Shuri
📌 Team update (2026-02-10): 3-sprint roadmap adopted — Sprint 1 assigns Wong: build.yml CI, release.yml stub, branch protection — decided by Rhodey

### Sprint 1 — CI/CD Pipeline Created (2026-02-10)

**Workflows created:**

- `.github/workflows/build.yml` — CI on push/PR to `main`, matrix build (ubuntu + windows), restore → build → test → pack → upload artifacts. Concurrency groups cancel stale runs.
- `.github/workflows/release.yml` — Publishes to NuGet.org on `v*` tag push. Also creates a GitHub Release with nupkg files attached. Uses `softprops/action-gh-release@v2`.
- `.github/PULL_REQUEST_TEMPLATE.md` — Standard PR template with What/Why/Testing/Checklist sections.

**Key decisions:**

- No separate `pr-validation.yml` — `build.yml` already triggers on PRs, so a separate workflow would be duplicate work and wasted runner minutes.
- NuGet artifact upload restricted to `ubuntu-latest` to avoid duplicate artifact names in the matrix.
- `--skip-duplicate` on nuget push so re-running a tag workflow doesn't fail if packages were already published.
- Used `dotnet-version: '10.0.x'` to match the `net10.0` target framework in Directory.Build.props.

**Secrets required:**

- `NUGET_API_KEY` — must be added in GitHub repo Settings → Secrets → Actions. Generate from nuget.org account → API Keys.

📌 Team update (2026-02-10): NuGet hardening completed — 6 floating deps pinned, SourceLink/deterministic builds added to Directory.Build.props — decided by Shuri
📌 Team update (2026-02-10): Test infrastructure created — 62 tests (45 RCON + 17 hosting) now available for CI execution — decided by Nebula
📌 Team update (2026-02-10): All sprint work tracked as GitHub issues with team member and sprint labels — decided by Jeffrey T. Fritz

📌 Team update (2026-02-10): Single NuGet package consolidation — only Aspire.Hosting.Minecraft is packable, CI/CD should build/pack accordingly — decided by Jeffrey T. Fritz, Shuri

📌 Team update (2026-02-10): NuGet PackageId renamed from Aspire.Hosting.Minecraft to Fritz.Aspire.Hosting.Minecraft (Aspire.Hosting prefix reserved by Microsoft) — decided by Jeffrey T. Fritz, Shuri

📌 Team update (2026-02-10): NuGet package version now defaults to 0.1.0-dev; CI overrides via -p:Version from git tag — decided by Shuri
📌 Team update (2026-02-10): Release workflow extracts version from git tag and passes to dotnet build/pack — decided by Wong

### Sprint 2 — CI Hardening (Issue #17)

**Changes:**

- **Test execution in CI:** Already present in `build.yml` — `dotnet test Aspire-Minecraft.slnx --no-build -c Release --verbosity normal` runs after the build step on both ubuntu and windows matrix legs. No changes needed.
- **Dependabot:** Created `.github/dependabot.yml` with weekly updates for both `nuget` and `github-actions` ecosystems targeting the root directory. Open PR limit set to 10 per ecosystem.
- **Issue templates:** Created YAML-form issue templates:
  - `.github/ISSUE_TEMPLATE/bug_report.yml` — fields: summary, description, steps to reproduce, expected/actual behavior, environment.
  - `.github/ISSUE_TEMPLATE/feature_request.yml` — fields: summary, description, use case, proposed solution.
  - `.github/ISSUE_TEMPLATE/config.yml` — `blank_issues_enabled: true` so users can still open freeform issues.

**Key observations:**

- The test step was already in build.yml from Sprint 1 — Wong had included it in the original CI workflow. The Sprint 2 roadmap item was redundant.
- Dependabot will auto-create PRs for outdated NuGet packages and Actions versions, reducing manual dependency management.

### Release Versioning — Tag-Driven NuGet Versions

**Changes:**

- Updated `.github/workflows/release.yml` to extract the version from the git tag (`v*` → strip `v` prefix) and pass it via `-p:Version=` to both `dotnet build` and `dotnet pack`. GitHub Release name now includes the version.
- `build.yml` (CI) intentionally left unchanged — CI builds use the default csproj version, which is correct for non-release artifacts.

**Key observations:**

- Without `-p:Version=`, every release produced `0.1.0` packages regardless of the tag. This was a silent bug — the pipeline appeared to work but published wrong versions.
- The `GITHUB_REF_NAME` variable gives the tag name directly (e.g., `v0.2.1`), and `${GITHUB_REF_NAME#v}` strips the `v` prefix in bash. This is simpler than parsing `GITHUB_REF` (which includes `refs/tags/`).
- Version is passed to both build and pack to ensure assembly version and package version are consistent.

### Sprint 3 — Changelog, Symbol Packages, CodeQL (Issue #26)

**Changes:**

- **Changelog generation:** Already handled — `release.yml` uses `generate_release_notes: true` in `softprops/action-gh-release@v2`, which auto-generates release notes from PRs and commits. No additional tooling or workflow needed.
- **Symbol packages (.snupkg):** Added `<IncludeSymbols>true</IncludeSymbols>` and `<SymbolPackageFormat>snupkg</SymbolPackageFormat>` to `src/Aspire.Hosting.Minecraft/Aspire.Hosting.Minecraft.csproj`. Updated `release.yml` with a separate `dotnet nuget push "nupkgs/*.snupkg"` step and attached snupkg files to GitHub Release. Updated `build.yml` to upload snupkg alongside nupkg in CI artifacts.
- **CodeQL scanning:** Created `.github/workflows/codeql.yml` — triggers on push/PR to main + weekly schedule (Monday 06:25 UTC). Uses `github/codeql-action/init@v3` and `github/codeql-action/analyze@v3` with `csharp` language and default query suite. Permissions: `security-events: write` + `contents: read`.
- **GitHub Pages docs:** Deferred — too heavy for this sprint per task description.

**Key decisions:**

- Changelog: No extra workflow or tooling. GitHub's built-in release notes generation is sufficient for this project's scale.
- Symbol packages pushed as a separate `dotnet nuget push` step (not relying on `.nupkg` push to auto-detect `.snupkg`) for explicitness and debuggability.
- CodeQL uses a full `dotnet build` step (not autobuild) because the project requires .NET 10 SDK setup.
- CodeQL schedule uses a non-default minute offset (`25`) to avoid GitHub Actions cron congestion at :00.

**Verified:**

- `dotnet build -c Release` passes (packable project builds clean).
- `dotnet test --no-build -c Release` passes (62 tests: 45 RCON + 17 hosting).
- `dotnet pack` produces both `.nupkg` and `.snupkg` files.

📌 Team update (2026-02-10): Azure RG epic designed — separate NuGet package Fritz.Aspire.Hosting.Minecraft.Azure, polling for v1, DefaultAzureCredential — decided by Rhodey, Shuri
📌 Team update (2026-02-10): CI/CD needed for second NuGet package (Fritz.Aspire.Hosting.Minecraft.Azure) — decided by Rhodey
📌 Team update (2026-02-10): API surface frozen for v0.2.0 — any additions require explicit review — decided by Rhodey
📌 Team update (2026-02-10): User directive — each sprint in a dedicated branch, merged via PR to main — decided by Jeffrey T. Fritz

### Documentation Path Filtering (2026-02-10)

**Changes:**

- Added `paths-ignore` filters to all three GitHub Actions workflows (build.yml, release.yml, codeql.yml) to skip builds/tests/analysis when only documentation changes.
- Ignored paths: `docs/**`, `user-docs/**`, `*.md` (root-level markdown), `.ai-team/**` (squad state).
- Applied to both `push` and `pull_request` triggers for build.yml and codeql.yml. Applied to `push.tags` in release.yml for completeness (unlikely scenario but prevents accidental doc-only tag releases).

**Key decisions:**

- Documentation updates (README, CONTRIBUTING, docs/, user-docs/) don't require CI build/test/pack cycles — they don't affect code correctness or package output.
- The `.ai-team/**` folder contains squad-internal state and decisions, also irrelevant to builds.
- Root-level `*.md` pattern catches README.md, CONTRIBUTING.md, etc. but not markdown files in subdirectories (those would be caught by `docs/**` or `user-docs/**` as appropriate).
- The scheduled CodeQL run (Monday 06:25 UTC) is unaffected by path filters — it always runs on schedule regardless of recent commits.
 Team update (2026-02-11): All sprints must include README and user documentation updates to be considered complete  decided by Jeffrey T. Fritz
 Team update (2026-02-11): All plans must be tracked as GitHub issues and milestones; each sprint is a milestone  decided by Jeffrey T. Fritz

### Sprint 4 — Milestone & Issue Tracking Created (2026-02-11)

**Milestone created:**

- "Sprint 4 — Visual Identity & Dashboard" (milestone #1) — Enhanced building styles (cylinder databases, Azure flags), Redstone Dashboard wall, Dragon Health Egg, and DX polish.

**Label created:**

- `sprint-4` — applied to all 14 issues.

**Issues created (14 total):**

- #49: Unit tests for Sprint 4 features [enhancement, sprint-4]
- #62: WithAllFeatures() convenience method [enhancement, sprint-4]
- #63: Tighten feature env var checks to == "true" [enhancement, sprint-4]
- #64: Welcome teleport on player join [enhancement, sprint-4]
- #65: Dragon Health Egg monument [enhancement, sprint-4]
- #66: Cylinder buildings for database resources [enhancement, sprint-4]
- #67: Azure-themed buildings with blue banner [enhancement, sprint-4]
- #68: Update health indicator for new structure types [enhancement, sprint-4]
- #69: Redstone Dashboard Wall construction [enhancement, sprint-4]
- #70: Health history ring buffer [enhancement, sprint-4]
- #71: Dashboard scroll and bar chart updates [enhancement, sprint-4]
- #72: WithRedstoneDashboard() extension method [enhancement, sprint-4]
- #73: README update for Sprint 4 features [documentation, sprint-4]
- #74: User docs for Sprint 4 features [documentation, sprint-4]

**Notes:**

- `enhancement` and `documentation` labels already existed; `sprint-4` was created new (color: #5319e7).
- Issue #49 and #62 were created via MCP tool; #63–#74 via REST API due to rate limiting on parallel MCP calls.
