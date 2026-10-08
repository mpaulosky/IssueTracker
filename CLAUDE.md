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
  Services cache reads in `IMemoryCache` (one minute for issues and comments, a day for categories and statuses).
  Every write path removes each cache entry it affects, including `IssueService`'s per-Author list, which is keyed by
  the Author's Id; keep it that way in new ones. Redis backs the distributed cache that
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

- `global.json` comes from the repo-ci-baseline Template: it pins the SDK (10.0.401, `rollForward: latestMinor`) and runs
  `dotnet test` on Microsoft Testing Platform. Change it in the Template, not here; the next Apply overwrites it.
- Bogus Razor errors (RZ1021 in pages the branch never touched) come from a build server another SDK started, not from
  the code: run `dotnet build-server shutdown` and build again.
- Package versions live in `Directory.Packages.props` (Central Package Management). A `PackageReference` has no `Version`.
- Builds treat warnings as errors. A `NoWarn` needs a comment saying why and when it goes away.
- Run test projects one at a time. At the `.slnx` level the test runner can report `Zero tests ran`.
- A test project needs Docker when it uses Testcontainers, `Aspire.Hosting.Testing` or Playwright. Today that is
  `IssueTracker.PlugIns.Tests.Integration`, which starts MongoDB with Testcontainers. `Integration.Tests` and
  `AppHost.Tests` build Aspire models and fakes without starting containers, so they run without Docker.
- `.sandcastle/check.sh` is the gate Sandcastle's agents run in their sandbox, which has no Docker: the YAML and shell
  lints, the Release build and every test project that doesn't need Docker. The pre-push gate and CI run the rest.

## Tests

- xUnit v3 on Microsoft Testing Platform, with FluentAssertions, NSubstitute or Moq, and Bogus fakes from CoreBusiness.
  `IAsyncLifetime` returns `ValueTask`.
- Components are tested with bUnit: test classes derive from `BunitContext`, not the obsolete `TestContext`.
- Every test has `// Arrange`, `// Act` and `// Assert` markers. Test names read as
  `Method_With_Condition_Should_Result_Test`, as in `CreateCategory_With_Valid_Values_Should_Return_Test`.
- Write the failing test first for new behaviour and bug fixes.
- CI fails below 60% line coverage.

## Code style

`.editorconfig` is authoritative: tabs, file-scoped namespaces, explicit types rather than `var`, and `System` usings
first. Every `.cs` file starts with the repository's copyright header block, and public members have XML doc comments.

## Workflow

Branches, worktrees, commits, PR titles and descriptions, checks, review, merging and releases follow
[docs/PROCESS.md](docs/PROCESS.md). Worktrees go under `../IssueTracker-worktrees/`.
