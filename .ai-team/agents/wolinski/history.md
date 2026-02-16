# History — Wolinski

## Project Learnings (from init)

**Project:** IssueTracker  
**API Framework:** ASP.NET Core Minimal APIs, .NET 10  
**Database:** MongoDB (MongoDB.Driver + MongoDB.EntityFrameworkCore)  
**Orchestration:** .NET Aspire (AppHost, ServiceDefaults)  
**Owner:** mpaulosky  
**Goal:** Aspireify and enhance the application  

**API Patterns:**

- Minimal APIs in `Program.cs` or map extensions
- OpenAPI via Scalar (not Swagger UI)
- Request DTOs with FluentValidation
- Async/await: `async Task<IResult> GetIssue(int id, IIssueService service) => ...`
- File-scoped namespaces, PascalCase types

**Database (MongoDB):**

- Repositories: `IIssueRepository`, `ICommentRepository`, etc.
- DbContext pooling if using EF Core MongoDB
- Change tracking for updates
- Async operations: `await repo.GetIssueAsync(id)`

**Aspire Architecture:**

- AppHost defines services and resources
- ServiceDefaults provides shared middleware (logging, health checks, tracing)
- Each service registers: `.AddServiceDefaults()` in Program.cs
- Resources: containers (MongoDB), API endpoints

**Testing:**

- Integration tests with TestContainers (Docker MongoDB)
- xUnit + FluentAssertions
- Test both happy path and error cases

## Learnings

### Phase 1: Aspire Foundation Architecture Implementation

**Project Naming Convention:**

- Renamed `src/IssueTracker.AppHost` → `src/AppHost` for consistency with other projects
- Updated namespace references, solution file (IssueTracker.slnx), documentation (docs/getting-started.md)
- Program.cs uses top-level statements — cannot have file-scoped namespace declaration

**ServiceDefaults Project Structure:**

- Created `src/ServiceDefaults/` as shared infrastructure project
- Folder structure: HealthChecks/, Observability/ for future Phase 2 implementation
- Extensions.cs provides `AddServiceDefaults()` entry point (placeholder stub)
- MongoDbHealthCheck.cs and OpenTelemetryExtensions.cs created as stubs

**Aspire 13.0 API Changes (from 10.0):**

- `.WithBindPort()` and `.WithHostPort()` APIs removed — container ports auto-managed
- `.WithReference()` for ContainerResource to ProjectResource incompatible — simplified to `.WaitFor()` only
- Project references require `IsAspireProjectResource="true"` attribute for strong-typed Projects namespace generation
- Directory.Packages.props version updated to Aspire 13.0.0 for compatibility

**CPM (Centralized Package Management):**

- All PackageReference elements in .csproj files must omit Version attribute
- Versions managed exclusively in Directory.Packages.props at repo root
- Enforced via `<ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>`

**File Paths:**

- AppHost: `src/AppHost/AppHost.csproj`, `src/AppHost/Program.cs`
- ServiceDefaults: `src/ServiceDefaults/ServiceDefaults.csproj`, `src/ServiceDefaults/Extensions.cs`
- Solution: `IssueTracker.slnx` (XML-based solution format)
- Documentation: `docs/getting-started.md`

## Session 2: 2026-02-16 — AppHost Aspire Orchestration Setup

### Aspire Configuration Patterns

- **DistributedApplication API**: Used `DistributedApplication.CreateBuilder()` to scaffold the AppHost orchestrator
- **Container Resources**: MongoDB containerized via `.AddContainer("mongodb", "mongo")` with environment variable injection (`MONGO_INITDB_ROOT_USERNAME`, `MONGO_INITDB_ROOT_PASSWORD`)
- **Service References**: Services registered via `.AddProject<T>()` with strong typing (`IssueTracker_UI` namespace)
- **Dependency Binding**: `.WithReference(mongodb)` establishes service-to-resource dependency; `.WaitFor(mongodb)` ensures startup ordering
- **Port Binding**: Explicit port mapping via `.WithBindPort(27017, 27017)` for local dev debugging

### MongoDB Container Setup Strategy

- **Init Authentication**: SCRAM-SHA-compatible (`MONGO_INITDB_ROOT_USERNAME`, `MONGO_INITDB_ROOT_PASSWORD`) for dev environment
- **Default Credentials**: Username `admin`, password `admin` for local development
- **Port Exposure**: Port 27017 exposed to host machine for debugging (Aspire internal DNS name: `mongodb`)
- **Health Checks**: Aspire automatically manages container lifecycle; explicit health check registration not required in AppHost (delegated to UI service)

### Service Reference Structure

- **Project References**: All core dependencies (CoreBusiness, Services, PlugIns.Mongo, UI) referenced in AppHost.csproj to enable strong-typed service registration
- **Blazor UI as Primary Host**: UI service registered first as the main application endpoint
- **No Internal APIs in AppHost**: CoreBusiness and Services remain internal project references (not exposed as separate service bindings); they're consumed by UI via dependency injection
- **Vertical Slice Preserved**: Each project maintains its own extension methods for DI registration (e.g., `RegisterConnections`, `RegisterDatabaseContext`)

### .NET 10 / Aspire-Specific Notes

- **Aspire NuGet Packages**: `Aspire.Hosting` v10.0.0 and `Aspire.Hosting.AppHost` v10.0.0 added to Directory.Packages.props (centralized version management enforced)
- **Generic Type Registration**: Used `AddProject<Projects.IssueTracker_UI>()` pattern for compile-time type safety; requires proper namespace qualification
- **Configuration Source Order**: AppHost appsettings.json provides base logging; Development.json overrides for debug verbosity
- **Launch Profile**: AppHost listens on <http://localhost:17000> (Aspire dashboard); actual services bind dynamically

### Remaining Considerations

- **MongoDB Connection String**: UI service receives connection string via Aspire binding (not read from appsettings); RegisterConnections extension must consume from environment/provider
- **Build Order**: Visual Studio resolves dependency graph (AppHost → UI → CoreBusiness/Services/PlugIns)

### Phase 2: ServiceDefaults Implementation Complete (2026-02-16)

**OpenTelemetry Configuration:**

- Full instrumentation pipeline: ASP.NET Core, HTTP Client, Runtime metrics
- Dual exporters: Console (local dev debugging) + OTLP (Aspire dashboard / production)
- Environment-aware sampling: 10% in production (TraceIdRatioBasedSampler), 100% in dev (AlwaysOnSampler)
- Package versions: OpenTelemetry 1.11.0 (NU1902 vulnerability warning on OpenTelemetry.Api 1.11.1 transitive dependency — acceptable for dev)

**MongoDB Health Check:**

- Implemented `MongoDbHealthCheck` with 3-second timeout and graceful degradation
- Uses MongoDB ping command against admin database (`{ping:1}`)
- Proper exception handling: OperationCanceledException (timeout vs user cancellation), MongoException, generic fallback
- Linked cancellation tokens for timeout + user cancellation coordination

**ServiceDefaults Architecture:**

- `Extensions.cs`: Entry point with `.AddServiceDefaults()` registering OpenTelemetry, health checks, and problem details
- `Observability/OpenTelemetryExtensions.cs`: Centralized OTel configuration with `.AddOpenTelemetryExporters()`
- `HealthChecks/MongoDbHealthCheck.cs`: Production-ready MongoDB connectivity check
- Global usings: Added MongoDB.Driver, OpenTelemetry.* namespaces for cleaner code

**Build Verification:**

- ServiceDefaults: Builds successfully with 1 warning (NU1902)
- Full solution: Builds successfully (0 errors)
- Architecture tests: 4/4 passing

**File Structure:**

- `src/ServiceDefaults/Extensions.cs` — Main entry point
- `src/ServiceDefaults/HealthChecks/MongoDbHealthCheck.cs` — MongoDB health check
- `src/ServiceDefaults/Observability/OpenTelemetryExtensions.cs` — OTel configuration
- `src/ServiceDefaults/GlobalUsings.cs` — Shared namespace imports
- `Directory.Packages.props` — Added 7 OTel packages (version 1.11.0)

**Next Phase:**

- Phase 3: Wire UI/Program.cs to call `.AddServiceDefaults()` and `app.MapDefaultEndpoints()`
- Phase 3: Update AppHost to use Aspire MongoDB resource binding

### Phase 3 Step 2: Aspire AppHost Orchestration Complete (2026-02-16)

**Aspire 13.0 MongoDB Integration Pattern:**

- Used `.AddMongoDB("mongodb")` instead of manual `.AddContainer()` — Aspire-native MongoDB resource
- `.WithDataVolume()` for persistent MongoDB data across container restarts
- `.WithHealthCheck("mongodb")` requires explicit key parameter (breaking change from Aspire 10.0)
- `.WithReference(mongodb)` establishes service-to-resource dependency and injects connection string
- `.WaitFor(mongodb)` removed — not needed in Aspire 13.0 API (implicit with `.WithReference()`)

**MongoDB Driver Version Compatibility:**

- Aspire.Hosting.MongoDB 13.0.0 requires MongoDB.Driver >= 3.5.0
- Updated Directory.Packages.props: MongoDB.Driver 3.4.3 → 3.5.0, MongoDB.Bson 3.4.3 → 3.5.0
- Breaking change: Must coordinate Aspire package version with MongoDB.Driver minimum version requirement

**AppHost Build Configuration:**

- Added Aspire.Hosting.MongoDB package to Directory.Packages.props (version 13.0.0)
- Added package reference to AppHost.csproj (CPM pattern — no version attribute)
- AppHost.csproj requires `IsAspireProjectResource="true"` on UI project reference for strong-typed Projects namespace

**Build Verification Results:**

- AppHost builds successfully: 0 errors, 1 warning (NU1902 — OpenTelemetry.Api vulnerability, acceptable)
- Full solution build: 0 errors, 12 warnings (all NU1902)
- Architecture tests: 4/4 passing
  - `AppHost_MustNotBeReferencedByOtherProjects` ✓
  - `UI_ShouldNotDependOnAppHost` ✓
  - `CoreBusiness_ShouldNotDependOnUIOrAppHost` ✓
  - `ServiceDefaults_MustHaveNoCircularDependencies` ✓

**Orchestration Architecture:**

- AppHost orchestrates: MongoDB ContainerResource + Blazor UI ProjectResource
- UI receives MongoDB connection string via Aspire dependency injection (not appsettings.json)
- Health check key "mongodb" registered for monitoring
- Data volume ensures dev data persists across container restarts

**File Structure:**

- `src/AppHost/Program.cs` — Full orchestration implementation
- `src/AppHost/AppHost.csproj` — Verified with Aspire.Hosting.MongoDB reference
- `Directory.Packages.props` — MongoDB.Driver 3.5.0, Aspire.Hosting.MongoDB 13.0.0

**Next Phase:**

- Phase 3 Step 3: Wire UI/Program.cs to consume MongoDB connection from Aspire binding
- Phase 3 Step 4: Test full orchestration startup (`dotnet run --project src/AppHost`)
