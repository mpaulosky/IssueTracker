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
