# IssueTracker

An issue tracker built as a Blazor Server app on .NET 10 and C# 14, with MongoDB for storage and Redis for caching,
orchestrated by .NET Aspire. The files under `.github/instructions/` add detail for specific file types; where one of
them disagrees with this file, this file wins.

## Layout

The solution is `IssueTracker.slnx`. It follows Clean Architecture, and dependencies point inward only:

| Project | Layer | Holds |
| --- | --- | --- |
| `src/UI/IssueTracker.UI` | Presentation | Blazor Server pages (`Pages/`), components (`Components/`), layout (`Shared/`), DI registration (`Extensions/`) |
| `src/Services/IssueTracker.Services` | Application | One service per aggregate (`Issue/`, `Comment/`, `Category/`, `Status/`, `User/`) and the repository interfaces they need (`PlugInRepositoryInterfaces/`) |
| `src/CoreBusiness/IssueTracker.CoreBusiness` | Domain | Models (`IssueModel`, `CommentModel`, ...), their `Basic*Model` summaries, enums, and Bogus fakes |
| `src/PlugIns/IssueTracker.PlugIns` | Infrastructure | MongoDB repositories (`DataAccess/`) and `MongoDbContextFactory` |
| `src/ServiceDefaults` | Cross-cutting | Aspire service defaults, OpenTelemetry, health checks, `CacheService` |
| `src/AppHost` | Orchestration | Starts MongoDB, Redis, and the UI |

`tests/Architecture.Tests` checks part of this with NetArchTest: CoreBusiness doesn't depend on the UI or AppHost,
nothing references AppHost, and ServiceDefaults has no cycles. The rest of the layering is convention, so keep to it.

## How the pieces fit

- The UI calls services only. It references `IssueTracker.PlugIns` for two things: `Extensions/` (the composition root)
  registers the repositories, and `Helpers/MongoHealthCheck.cs` pings MongoDB for the health endpoint. Pages and
  components never touch MongoDB or a repository.
- Services depend on the repository interfaces in `PlugInRepositoryInterfaces/`; `IssueTracker.PlugIns` implements them.
  Services cache reads in `IMemoryCache`. Every write must remove the cached entry, or pages show stale data for up to
  a day; `StatusService`'s create, update and delete don't yet. Redis backs the distributed cache that
  `ServiceDefaults` registers (`CacheService`).
- Authentication is Azure AD B2C through Microsoft.Identity.Web (`AzureAdB2C` configuration section), wired up in
  `Extensions/AuthenticationService.cs`.
- UI components come from Radzen.Blazor, styled with Bootstrap plus `wwwroot/css/site.css`.
- Configuration comes from user secrets, environment variables (prefix `IssueTrackerUI_`), and Aspire's resource references.
  Never put secrets in `appsettings*.json` or code.

## Build, run and test

```bash
dotnet build IssueTracker.slnx -c Release -warnaserror   # what CI and the pre-push gate run
dotnet run --project src/AppHost                          # the whole app with MongoDB and Redis (needs Docker)
dotnet test tests/<Project> -c Release                    # one test project
scripts/gate.sh                                           # everything the pre-push hook checks
```

- The SDK is pinned in `global.json` to the 10.0.3xx band (`rollForward: latestPatch`). SDK 10.0.4xx doesn't compile the
  Razor pages yet.
- Package versions live in `Directory.Packages.props` (Central Package Management). A `PackageReference` has no `Version`.
- Builds treat warnings as errors. A `NoWarn` needs a comment saying why and when it goes away.
- Run test projects one at a time. At the `.slnx` level the test runner can report `Zero tests ran`.
- Integration tests (`Integration.Tests`, `IssueTracker.PlugIns.Tests.Integration`) and `AppHost.Tests` start containers
  with Testcontainers or Aspire, so they need Docker.

## Tests

- xUnit v2 on VSTest, with FluentAssertions, NSubstitute or Moq, and Bogus fakes from CoreBusiness.
- Components are tested with bUnit. The existing tests still use bUnit's obsolete `TestContext`, pending a migration;
  new tests use its replacement, `BunitContext`.
- Every test has `// Arrange`, `// Act` and `// Assert` markers. Test names read as
  `Method_With_Condition_Should_Result_Test`, as in `CreateCategory_With_Valid_Values_Should_Return_Test`.
- Write the failing test first for new behaviour and bug fixes.
- CI fails below 60% line coverage.

## Code style

`.editorconfig` is authoritative: tabs, file-scoped namespaces, explicit types rather than `var`, and `System` usings
first. Every `.cs` file starts with the repository's copyright header block, and public members have XML doc comments.

## Workflow

- Branches: `squad/{issue}-{slug}`, `sprint/{n}-{slug}`, `feature/{issue}-{slug}`, `hotfix/{issue}-{slug}` or
  `chore/{slug}`, each in its own worktree under `../IssueTracker-worktrees/`. PRs target `main`.
- Turn on the hooks once per clone: `git config core.hooksPath .github/hooks`. Pre-commit lints staged Markdown;
  pre-push checks the branch name and runs `scripts/gate.sh`.
- Commit messages follow `.github/instructions/git-commit-instructions.md`.
- `Build Solution` and `Test Suite` are required checks. `pr-automerge.yml` squash-merges a PR once they pass, Copilot has
  reviewed it and every thread is resolved; open a draft PR to hold one back.
- Each PR merged to `main` gets a release (patch by default; label `semver:minor` or `semver:major` to bump more) and
  a follow-up PR with its blog post under `docs/blogs/`. PRs with `[skip-release]` in the title, such as those blog
  PRs and metrics refreshes, don't release.
