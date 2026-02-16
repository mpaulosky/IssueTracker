# Decisions

> Authoritative decision ledger for the IssueTracker Squad.
>
> Agents write decisions to `.ai-team/decisions/inbox/{name}-{slug}.md`.
> Scribe merges inbox entries here and deduplicates/consolidates overlaps.
> All agents read this file at spawn time to respect team decisions.

---

## 2026-02-16: AppHost Design Review

**By:** Milo  
**Context:** Setting up .NET Aspire orchestrator for IssueTracker  
**Participants:** Milo, Wolinski (Backend), Stansfield (Frontend)

### Key Decisions

1. **Orchestration Model**: AppHost is the single entry point; it launches Blazor UI (port 5000) as the primary host and manages MongoDB container via Aspire resources.

2. **Service Architecture**: 
   - CoreBusiness and Services are project references (internal dependencies) — NOT service bindings
   - PlugIns.Mongo is a project reference providing data access implementations
   - UI references CoreBusiness and PlugIns for business logic

3. **MongoDB Integration**:
   - MongoDB managed as `ContainerResource` in AppHost
   - Init database: "devissuetracker" with SCRAM-SHA auth (course/whatever for dev, secrets for prod)
   - Connection string injected via Aspire binding; no hardcoded strings in appsettings
   - Container DNS name: `mongodb` (internal to Aspire network)

4. **Port Assignment**:
   - Blazor UI: 5000 (HTTPS) / 5001 (HTTP)
   - MongoDB: 27017 (internal to Aspire network; expose only for local debugging if needed)
   - Health check endpoints required on UI before Aspire considers deployment ready

5. **Development Workflow**:
   - Replace `docker-compose up + dotnet watch run` with single `dotnet run --project AppHost`
   - Breakpoint debugging flows through AppHost process
   - testEnvironments.json deprecated; use Aspire configuration instead

6. **Build & CI/CD**:
   - Add AppHost project to IssueTracker.slnx
   - Build order: AppHost → UI → Services → PlugIns (Aspire resolves graph)
   - AppHost publishes as container for staging/production

### Risks Mitigated

- **Risk**: Aspire service binding complexity — ensure CoreBusiness/Services DI registration aligns with Aspire conventions
- **Risk**: MongoDB health check timeout on first startup; may need adjusted wait policies in Aspire config
- **Concern**: testEnvironments.json conflict — need clear deprecation path or migration strategy
- **Concern**: Local dev debugging with Aspire orchestration may have latency on first launch; document expected startup time (~10-15s)

---

## 2026-02-16: AppHost Aspire Orchestration Configured

**By:** Wolinski  
**Status:** Implemented ✓

### Summary

Configured .NET Aspire orchestration for IssueTracker by creating the AppHost project scaffold with MongoDB container resource, service references (UI, CoreBusiness, Services, PlugIns), and local development workflow (`dotnet run`).

### Implementation Details

**AppHost Project Structure:**
- Program.cs with Aspire DistributedApplication orchestration
- MongoDB ContainerResource: SCRAM-SHA auth (admin/admin), port 27017, DNS name `mongodb`
- Service Registration: Blazor UI via `AddProject<Projects.IssueTracker_UI>()`
- Dependency Binding: `.WithReference(mongodb)` + `.WaitFor(mongodb)`
- Project References: IssueTracker.UI, Services, CoreBusiness, PlugIns

**Central Package Versioning:**
- Added Aspire packages to Directory.Packages.props: v10.0.0

**Solution Configuration:**
- Updated IssueTracker.slnx: AppHost project path, folder structure

### Local Development Workflow

```bash
dotnet run --project src/IssueTracker.AppHost
```

- Aspire dashboard: http://localhost:17000
- Blazor UI: http://localhost:5000 (HTTP/5001 HTTPS)
- MongoDB: mongodb://admin:admin@localhost:27017 (debugging)
- Expected startup: 10-15 seconds

### Code Quality

✓ .NET 10, C# 14, file-scoped namespaces, nullable reference types, 120-char lines, tab indent (size 2)

### Outstanding Tasks

- [ ] Aspire connection string binding into UI services
- [ ] ServiceDefaults project for shared middleware
- [ ] Health check endpoint validation

---

## 2026-02-16: Solution & Dev Docs Updated for AppHost Integration

**By:** Stansfield  
**Status:** Complete

### Summary

Updated IssueTracker solution structure and local development documentation to support .NET Aspire-based AppHost orchestration.

### Changes

**Solution File (IssueTracker.slnx):**
- Added `/src/AppHost/` folder
- Added reference to IssueTracker.AppHost.csproj
- Build order includes AppHost as entry point

**Getting Started Docs (docs/getting-started.md):**
- Replaced docker-compose + manual MongoDB setup with `dotnet run --project AppHost`
- Updated prerequisites: .NET 10 SDK, Docker Desktop (Aspire-managed)
- Added Aspire dashboard documentation (`:15000`)
- Expanded troubleshooting: Aspire startup, MongoDB init, port conflicts, startup time (10-15s)
- Clarified Blazor UI starts automatically via orchestration

### Impact

- **Developers**: Faster onboarding, simpler local setup, better service visibility
- **CI/CD**: AppHost publishable as container; consistent build/deploy pipeline
- **Testing**: Integration tests use TestContainers directly (not Aspire)
- **Maintenance**: Single source of truth for local orchestration

### Developer UX

Single entry point: `dotnet run --project AppHost` handles everything:
- Automatic database provisioning (devissuetracker + dev credentials)
- Integrated health checks before readiness
- Real-time Aspire dashboard for debugging
- No manual appsettings.json configuration needed

---

## 2026-02-16: Aspire Foundation Architecture

**Decision Date:** 2026-02-16  
**Lead:** Milo  
**Status:** Approved for Implementation  
**Assigned To:** Wolinski (Backend)

### Context

IssueTracker has AppHost scaffolding from Wolinski's initial implementation. We need to establish the complete Aspire foundation:

1. **Simplified project naming** — align Aspire projects with existing conventions
2. **ServiceDefaults project** — centralized infrastructure concerns
3. **Complete AppHost design** — production-ready orchestration

This is **Priority 1** work — blocks all other Aspire integration.

### Decisions

#### 1. Project Naming Convention

**Decision:** Rename `IssueTracker.AppHost` → `AppHost`

**Rationale:**
- Consistency: Other projects follow simple naming (UI, Services, CoreBusiness, PlugIns)
- Simplicity: "AppHost" clearly indicates orchestrator role without namespace clutter
- .NET Aspire convention: Most reference architectures use simple names for orchestration projects

**Impact:**
- Rename folder: `src/IssueTracker.AppHost` → `src/AppHost`
- Update `.csproj` name: `AppHost.csproj`
- Update `IssueTracker.slnx` project reference
- Update namespace: `namespace AppHost;`
- Update docs/getting-started.md references

**Risk:** Low. Single file changes, no breaking dependencies (AppHost doesn't get referenced by other projects).

#### 2. ServiceDefaults Architecture

**Decision:** Create `ServiceDefaults` as a **shared project** (not NuGet package).

**Structure:**

```
src/ServiceDefaults/
├── ServiceDefaults.csproj
├── GlobalUsings.cs
├── Extensions.cs           // Main entry point: AddServiceDefaults()
├── HealthChecks/
│   └── MongoDbHealthCheck.cs
├── Observability/
│   └── OpenTelemetryExtensions.cs
└── Middleware/
    └── ErrorHandlingMiddleware.cs (optional, Phase 2)
```

**What Goes In:**

1. **OpenTelemetry** — Tracing, Metrics, Logging (Aspire dashboard integration)
2. **Health Checks** — MongoDB connectivity, API readiness
3. **Problem Details** — Standardized error responses (RFC 7807)
4. **Service Discovery** — Aspire-aware HTTP client configuration
5. **Resilience** — Polly retry/circuit breaker policies (Phase 2)

**What Stays Out:**

- Auth0 authentication (UI-specific, added by `.AddAuth0()` in UI/Program.cs)
- Business logic (CoreBusiness, Services remain untouched)
- MongoDB schema/seeding (PlugIns responsibility)

**Usage Pattern:**

Every project calls `.AddServiceDefaults()` in `Program.cs`:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();  // <-- Centralized infrastructure

// Project-specific registrations follow...
builder.Services.AddRazorComponents();
// etc.

var app = builder.Build();

app.MapDefaultEndpoints();  // <-- Health checks, liveness
app.Run();
```

**Why Not NuGet Package?**
- Simplicity: Shared project reference is easier for local dev/debug
- Deployment: AppHost publishes as single container; no NuGet complexity needed
- Flexibility: Can refactor to package later if multi-repo scenarios emerge

**Risks:**
- **Concern:** Every project must remember to call `.AddServiceDefaults()`. Mitigate with architecture tests (NetArchTest).
- **Risk:** ServiceDefaults grows into a "kitchen sink." Mitigate by strict code review — only infrastructure concerns allowed.

#### 3. AppHost Resource Definitions

**Decision:** AppHost orchestrates MongoDB + Blazor UI. No API service (we're using Minimal APIs embedded in UI).

**Resource Graph:**

```
AppHost
├── MongoDB (ContainerResource)
│   ├── Image: mongo:latest
│   ├── Port: 27017 (internal), 27018 (external for debugging)
│   ├── Credentials: admin/admin (dev), UserSecrets (prod)
│   └── Database: devissuetracker
└── Blazor UI (ProjectResource)
    ├── Ports: 5000 (HTTP), 5001 (HTTPS)
    ├── References: MongoDB connection string
    ├── WaitFor: MongoDB health check
    └── Project references: CoreBusiness, Services, PlugIns (in .csproj)
```

**MongoDB Configuration:**

- Use Aspire's `AddMongoDB()` extension for connection string injection
- Container init: SCRAM-SHA authentication required
- Health check: Ping database before UI starts
- External port (27018) for local debugging with MongoDB Compass; not exposed in production

**Why No Separate API Service?**

- IssueTracker uses **Minimal APIs in the Blazor app** (see UI/Program.cs)
- No need for microservices split at this stage
- Vertical slice architecture: feature endpoints live in UI project
- Reduces orchestration complexity and latency

**Phase 2 Consideration:** If API surface grows, split into `API` project with its own service resource.

#### 4. Execution Plan for Wolinski

**Phase 1: Rename and Structure** (1-2 hours)

1. Rename project:
   - `src/IssueTracker.AppHost` → `src/AppHost`
   - Update `AppHost.csproj`, namespace, `IssueTracker.slnx`
   - Update `docs/getting-started.md` (search/replace "IssueTracker.AppHost" → "AppHost")

2. Create ServiceDefaults project:
   - `src/ServiceDefaults/ServiceDefaults.csproj`
   - Add to `IssueTracker.slnx` under `/src/ServiceDefaults/`
   - Target framework: `net10.0`, nullable enabled, file-scoped namespaces

**Phase 2: ServiceDefaults Implementation** (2-3 hours)

1. **Extensions.cs** — Main entry point:
   ```csharp
   public static class ServiceDefaultsExtensions
   {
       public static IHostApplicationBuilder AddServiceDefaults(this IHostApplicationBuilder builder)
       {
           // OpenTelemetry
           builder.AddOpenTelemetryExporters();
           
           // Health checks
           builder.Services.AddHealthChecks()
               .AddCheck<MongoDbHealthCheck>("mongodb");
           
           // Problem details
           builder.Services.AddProblemDetails();
           
           return builder;
       }
   }
   ```

2. **HealthChecks/MongoDbHealthCheck.cs** — Ping MongoDB:
   ```csharp
   public class MongoDbHealthCheck : IHealthCheck
   {
       private readonly IMongoClient _client;
       
       public MongoDbHealthCheck(IMongoClient client)
       {
           _client = client;
       }
       
       public async Task<HealthCheckResult> CheckHealthAsync(
           HealthCheckContext context,
           CancellationToken cancellationToken = default)
       {
           try
           {
               await _client.GetDatabase("admin")
                   .RunCommandAsync((Command<BsonDocument>)"{ping:1}", cancellationToken: cancellationToken);
               return HealthCheckResult.Healthy("MongoDB connection is healthy");
           }
           catch (Exception ex)
           {
               return HealthCheckResult.Unhealthy("MongoDB connection failed", ex);
           }
       }
   }
   ```

3. **Observability/OpenTelemetryExtensions.cs** — Aspire dashboard integration:
   ```csharp
   public static class OpenTelemetryExtensions
   {
       public static IHostApplicationBuilder AddOpenTelemetryExporters(this IHostApplicationBuilder builder)
       {
           builder.Services.AddOpenTelemetry()
               .WithMetrics(metrics => metrics.AddAspNetCoreInstrumentation())
               .WithTracing(tracing => tracing.AddAspNetCoreInstrumentation());
           
           return builder;
       }
   }
   ```

4. **PackageReferences** (add to Directory.Packages.props):
   - `Aspire.Hosting` (already present)
   - `Microsoft.Extensions.Diagnostics.HealthChecks` (already present)
   - `MongoDB.Driver` (already present)
   - Add: `OpenTelemetry.Exporter.OpenTelemetryProtocol` (version 1.11.0)
   - Add: `OpenTelemetry.Extensions.Hosting` (version 1.11.0)
   - Add: `OpenTelemetry.Instrumentation.AspNetCore` (version 1.11.0)

**Phase 3: Integration** (1 hour)

1. Update `UI/IssueTracker.UI/Program.cs`:
   - Add `builder.AddServiceDefaults();` before other registrations
   - Add `app.MapDefaultEndpoints();` before `app.Run()`

2. Update `AppHost/Program.cs`:
   - Use Aspire MongoDB extension: `.AddMongoDB("mongodb")`
   - Wire UI with `.WithReference(mongodb).WaitFor(mongodb)`
   - Add external port binding for debugging: `.WithBindPort(27018, 27017)`

3. Update `UI/IssueTracker.UI.csproj`:
   - Add `<ProjectReference Include="..\..\ServiceDefaults\ServiceDefaults.csproj" />`

4. Update `AppHost/AppHost.csproj`:
   - Add package: `Aspire.Hosting.MongoDB`

### Alternatives Considered

**Alternative 1:** NuGet package for ServiceDefaults  
**Rejected:** Over-engineering for single-repo monolith. Adds build complexity without deployment benefit.

**Alternative 2:** Separate API service in AppHost  
**Rejected:** Premature. Minimal APIs in UI project are sufficient. Revisit if API surface grows beyond 10 endpoints.

**Alternative 3:** Keep project name `IssueTracker.AppHost`  
**Rejected:** Inconsistent with project naming conventions. Adds namespace noise without clarity.

### Success Criteria

1. **Developer workflow:**
   - `dotnet run --project src/AppHost` starts entire application
   - Aspire dashboard (`:15000`) shows MongoDB + UI with health checks green
   - Breakpoint debugging works through AppHost orchestration

2. **Code quality:**
   - All ServiceDefaults code follows standards (file-scoped namespaces, nullable, 120-char lines)
   - XML docs on public APIs
   - Architecture tests validate `.AddServiceDefaults()` called in all projects

3. **Documentation:**
   - `docs/getting-started.md` reflects new project names
   - `README.md` updated with Aspire dashboard link

4. **No regressions:**
   - All existing unit tests pass
   - Integration tests (TestContainers) unaffected

### Next Steps

1. **Wolinski:** Implement Phases 1-3 (6-7 hours total)
2. **Hooper:** Write architecture test validating ServiceDefaults usage
3. **Stansfield:** Review UI/Program.cs integration after Wolinski completes
4. **Milo:** Code review PR before merge

**Blockers:** None. All dependencies present.

**Timeline:** Target completion by end of week.

### References

- [.NET Aspire Documentation](https://learn.microsoft.com/en-us/dotnet/aspire/)
- [Aspire MongoDB Integration](https://learn.microsoft.com/en-us/dotnet/aspire/database/mongodb-integration)
- [OpenTelemetry .NET](https://opentelemetry.io/docs/languages/net/)
- [ASP.NET Core Health Checks](https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks)

---

## 2026-02-16: Markdown Formatting Quality Directive

**By:** User (via Coordinator)

**What:** When creating new Markdown files, scan and resolve formatting issues in the documents before finalizing. Ensure compliance with project Markdown standards defined in `.github/instructions/markdown.instructions.md`.

**Why:** Maintain consistent, high-quality documentation across the project from creation time rather than requiring later cleanup passes.

---

## 2026-02-16: CPM (Centralized Package Management) directive

**By:** User (via Coordinator)

**What:** When creating new projects, all package references must follow CPM patterns — no version specifications in project files. All versions managed centrally in `Directory.Packages.props`.

**Why:** User preference for consistency and centralized version control across the solution.
