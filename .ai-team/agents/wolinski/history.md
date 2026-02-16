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

(None yet — first session)

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
- **Launch Profile**: AppHost listens on http://localhost:17000 (Aspire dashboard); actual services bind dynamically

### Remaining Considerations

- **MongoDB Connection String**: UI service receives connection string via Aspire binding (not read from appsettings); RegisterConnections extension must consume from environment/provider
- **Service Defaults Project**: Not implemented yet; future enhancement for shared middleware (logging, tracing, health checks) across services
- **Build Order**: Visual Studio resolves dependency graph (AppHost → UI → CoreBusiness/Services/PlugIns)

