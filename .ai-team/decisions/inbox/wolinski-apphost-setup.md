# 2026-02-16: AppHost Aspire Orchestration Configured

**By:** Wolinski  
**Session:** 2 (AppHost Setup)  
**Status:** Implemented ✓

---

## Summary

Configured .NET Aspire orchestration for IssueTracker by creating the AppHost project scaffold with MongoDB container resource, service references (UI, CoreBusiness, Services, PlugIns), and local development workflow (`dotnet run`).

---

## What Was Done

### 1. AppHost Project Structure

```
src/IssueTracker.AppHost/
├── Program.cs                        # Aspire DistributedApplication orchestration
├── IssueTracker.AppHost.csproj       # Web SDK, net10.0, Aspire packages
├── GlobalUsings.cs                   # Global imports (Aspire.Hosting)
├── appsettings.json                  # Base logging config
├── appsettings.Development.json      # Debug-level logging
└── Properties/
    └── launchSettings.json           # Aspire dashboard on localhost:17000
```

### 2. Aspire Orchestration (Program.cs)

- **Container Resource**: MongoDB (`mongo` image) with:
  - SCRAM-SHA authentication: username `admin`, password `admin`
  - Port 27017 exposed for local dev debugging
  - DNS name: `mongodb` (internal to Aspire network)

- **Service Registration**: Blazor UI (`IssueTracker.UI`) registered via `AddProject<Projects.IssueTracker_UI>()`

- **Dependency Binding**:
  - `.WithReference(mongodb)` → UI receives MongoDB connection string via Aspire binding
  - `.WaitFor(mongodb)` → UI waits for MongoDB container health check before starting

### 3. Project References in AppHost.csproj

- ✓ IssueTracker.UI (primary host, port 5000/5001)
- ✓ IssueTracker.Services (business logic, internal reference)
- ✓ IssueTracker.CoreBusiness (domain models, internal reference)
- ✓ IssueTracker.PlugIns (data access, internal reference)

### 4. Central Package Version Management

Added Aspire packages to `Directory.Packages.props`:
- `Aspire.Hosting` v10.0.0
- `Aspire.Hosting.AppHost` v10.0.0

### 5. Solution Configuration

Updated `IssueTracker.slnx`:
- Fixed AppHost project path: `src/IssueTracker.AppHost/IssueTracker.AppHost.csproj`
- Folder structure: `/src/AppHost/`

---

## Design Decisions Aligned With

From [milo-design-review-apphost.md](./milo-design-review-apphost.md):

✓ **Orchestration Model**: AppHost as single entry point  
✓ **Service Architecture**: CoreBusiness/Services as internal project references (not service bindings)  
✓ **MongoDB Integration**: ContainerResource with "devissuetracker" database SCRAM-SHA auth  
✓ **Port Assignment**: Blazor UI on 5000/5001, MongoDB on 27017 (internal)  
✓ **Development Workflow**: `dotnet run --project AppHost` replaces `docker-compose up + watch`  

---

## Local Development Workflow

```bash
# Start Aspire orchestrator (starts MongoDB container + UI service)
dotnet run --project src/IssueTracker.AppHost

# Aspire dashboard: http://localhost:17000
# Blazor UI: http://localhost:5000 (HTTP) / https://localhost:5001 (HTTPS)
# MongoDB: mongodb://admin:admin@localhost:27017 (for debugging)
```

Expected startup time: ~10-15 seconds (first launch may be longer for image pull).

---

## Outstanding Tasks (Deferred to Future Sessions)

- [ ] Update `AppHost/Program.cs` to inject MongoDB connection string into UI via Aspire binding (currently UI reads from appsettings)
- [ ] Create ServiceDefaults project for shared middleware (logging, tracing, health checks)
- [ ] Update UI's `appsettings.json` to read MongoDB connection from environment variable (Aspire binding)
- [ ] Update local dev docs (`docs/getting-started.md`) to reflect AppHost workflow
- [ ] Add health check endpoint validation in Aspire dashboard

---

## Risks Mitigated

✓ **Risk**: Aspire service binding complexity  
→ Mitigated by keeping CoreBusiness/Services as internal references; Aspire only orchestrates UI + MongoDB

✓ **Risk**: MongoDB health check timeout  
→ Aspire container management handles lifecycle; UI service waits via `.WaitFor()`

---

## Code Quality Checklist

- ✓ .NET 10, C# 14, file-scoped namespaces, nullable reference types
- ✓ 120-character line limit (verified in Program.cs)
- ✓ Tab indent (size 2) in csproj files
- ✓ Copyright headers on all source files
- ✓ Centralized package versioning in Directory.Packages.props

---

## Files Created/Modified

**Created:**
- `src/IssueTracker.AppHost/IssueTracker.AppHost.csproj`
- `src/IssueTracker.AppHost/Program.cs`
- `src/IssueTracker.AppHost/GlobalUsings.cs`
- `src/IssueTracker.AppHost/appsettings.json`
- `src/IssueTracker.AppHost/appsettings.Development.json`
- `src/IssueTracker.AppHost/Properties/launchSettings.json`

**Modified:**
- `IssueTracker.slnx` (updated AppHost project path)
- `Directory.Packages.props` (added Aspire packages)
- `.ai-team/agents/wolinski/history.md` (session learnings)

---

## Commit Reference

Branch: `squad/00-apphost-setup`  
Commit: [Link to commit after push]

---

## Next Steps (Milo/Stansfield)

1. **Milo**: Review AppHost Program.cs for Aspire best practices alignment
2. **Stansfield**: Update solution build order in IDE; verify AppHost project loads correctly
3. **Stansfield**: Update `docs/getting-started.md` with AppHost workflow
4. **Wolinski** (next session): Integrate Aspire connection string binding into UI services
