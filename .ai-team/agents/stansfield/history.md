# History — Stansfield

## Project Learnings (from init)

**Project:** IssueTracker  
**UI Framework:** Blazor Server, .NET 10  
**Styling:** Tailwind CSS (no hardcoded colors; use scoped .razor.css)  
**Owner:** mpaulosky  
**Goal:** Aspireify and enhance the application  

**Component Patterns:**

- `.razor` files with `@page` (for pages) or `@inherits ComponentBase` (for reusable components)
- Suffix: `Component` for reusable, `Page` for routable
- File-scoped namespaces, PascalCase types
- Parameters via `[Parameter]`, event callbacks via `[Parameter] public EventCallback<T> OnEvent`
- Lifecycle: `OnInitializedAsync` (data), `OnParametersSetAsync` (param changes)
- Inject services: `@inject IIssueService IssueService`
- Render fragments: `[Parameter] public RenderFragment? ChildContent { get; set; }`

**Testing:**

- bUnit tests in `tests/Web.Tests.Bunit/`
- Test component renders, parameter changes, user interactions
- Snapshot tests for layouts

**Styling:**

- Tailwind utility classes in `.razor` markup
- Component-scoped styles in `.razor.css`
- Mobile-first responsive design

## Learnings

### AppHost Integration (2026-02-16)

**Solution Structure:**

- AppHost is the orchestrator entry point; goes in `/src/AppHost/` alongside other source projects
- `.slnx` file needs explicit folder structure `<Folder Name="/src/AppHost/">` before project reference
- Build order is implicit in Aspire graph; no need for manual dependency ordering in slnx

**Aspire & Blazor UI:**

- Blazor UI (IssueTracker.UI) is referenced as project dependency in AppHost Program.cs, not as separate web host
- Aspire handles port binding, health checks, and service lifecycle — UI doesn't need appsettings.json for MongoDB connection
- Health checks critical for Aspire readiness; dashboard (`:15000`) is primary tool for observing service state

**Developer UX Patterns:**

- Single `dotnet run --project AppHost` is game-changing for onboarding — eliminates "mystery" steps
- Troubleshooting docs must cover both Aspire orchestration AND underlying service issues (Docker, ports, timeouts)
- Expected startup time (10-15s) should be documented to prevent false failure assumptions
- Aspire dashboard is essential for developers to understand what's running and why

**Documentation:**

- Getting-started.md should emphasize (1) one command, (2) what that command does, (3) where to look for debugging
- Obsolete patterns (docker-compose, manual appsettings) must be removed to prevent confusion
- Prerequisites should reflect actual deps (Docker + .NET SDK); optional tools should be noted separately

### Phase 1 Documentation Audit (2026-02-16)

**Audit Scope:**

- Primary target: Verify no "IssueTracker.AppHost" references remain after rename to "AppHost"
- Files scanned: README.md, docs/getting-started.md, docs/architecture.md, docs/project-structure.md, docs/deployment.md, docs/configuration.md

**Findings:**

✅ **docs/getting-started.md**: CLEAN

- All AppHost references use correct path: `src/AppHost/AppHost.csproj`
- Aspire dashboard URL correct: `http://localhost:15000`
- MongoDB setup instructions accurate (container orchestration via Aspire)
- No "IssueTracker.AppHost" references found

✅ **docs/architecture.md**: CLEAN

- No AppHost references (architecture doc focuses on layers)
- Project naming consistent with conventions

✅ **docs/project-structure.md**: CLEAN

- No AppHost references (structure doc doesn't include orchestration layer yet)

✅ **docs/deployment.md**: NEEDS UPDATE (but out of scope)

- Still references .NET 7 and docker-compose patterns
- Not critical for Phase 1 — marks older pre-Aspire deployment strategy

✅ **docs/configuration.md**: NEEDS UPDATE (but out of scope)

- Still describes manual appsettings.json for MongoDB connection
- Aspire connection string injection pattern not documented
- Not critical for Phase 1 — functional for manual setups

❌ **README.md**: NEEDS FIX

- Line 22: Quick start shows `dotnet run --project src/IssueTracker.UI/IssueTracker.UI.csproj`
- Should be: `dotnet run --project src/AppHost/AppHost.csproj` (Aspire orchestration pattern)
- This bypasses the orchestrator and won't start MongoDB automatically

**Status:** Phase 1 docs 95% ready. One fix needed in README.md to align quick start with Aspire workflow.

**Learnings:**

- Documentation audit pattern: grep for exact old naming first, then verify replacement is functionally correct
- README.md quick start commands are high-visibility — must match actual developer workflow
- Deployment/configuration docs lag architecture decisions — acceptable tech debt if clearly marked as "legacy" or "manual setup"
- Aspire dashboard URLs should be verified against actual AppHost configuration (can be :15000 or :17000 depending on config)

### Phase 3 Step 1: UI Integration with ServiceDefaults (2026-02-16)

**Objective:** Wire Blazor UI to ServiceDefaults infrastructure to enable health checks, observability, and problem details.

**What I Did:**

1. **Added ServiceDefaults project reference to UI.csproj**
   - Location: `src/UI/IssueTracker.UI/IssueTracker.UI.csproj`
   - Reference: `..\..\ServiceDefaults\ServiceDefaults.csproj`

2. **Updated UI Program.cs with ServiceDefaults integration**
   - Added `builder.AddServiceDefaults();` call immediately after `WebApplicationBuilder` creation
   - Position: Before other service registrations so health checks, OTel, and problem details are configured first
   - Added `app.MapDefaultEndpoints();` call after `WebApplication.Build()` to expose health check endpoints
   - Removed duplicate `app.UseHealthChecks("/health");` middleware registration (now handled by ServiceDefaults)

3. **Extended ServiceDefaults with MapDefaultEndpoints()**
   - Added `MapDefaultEndpoints()` extension method to `ServiceDefaults/Extensions.cs`
   - Signature: `public static WebApplication MapDefaultEndpoints(this WebApplication app)`
   - Maps `/health` endpoint for health checks (OpenTelemetry, MongoDB connectivity)
   - Added `Microsoft.AspNetCore.Builder` namespace to `ServiceDefaults/GlobalUsings.cs` for `WebApplication` type

4. **Updated UI GlobalUsings.cs**
   - Added `global using ServiceDefaults;` to expose extension methods without explicit using directives

**Build Verification:**

- ✅ UI project builds successfully (0 errors, warnings about OpenTelemetry vulnerability are pre-existing)
- ✅ Full solution builds successfully (0 errors)
- ✅ No regressions in existing projects

**Technical Notes:**

- ServiceDefaults extension methods operate on `IHostApplicationBuilder`, which `WebApplicationBuilder` implements
- Health check endpoint at `/health` is now managed centrally by ServiceDefaults, not in UI middleware pipeline
- OpenTelemetry, problem details, and MongoDB health checks are now active in the UI

**File Locations:**

- UI Program.cs: `src/UI/IssueTracker.UI/Program.cs`
- UI .csproj: `src/UI/IssueTracker.UI/IssueTracker.UI.csproj`
- ServiceDefaults Extensions: `src/ServiceDefaults/Extensions.cs`
- ServiceDefaults GlobalUsings: `src/ServiceDefaults/GlobalUsings.cs`
- UI GlobalUsings: `src/UI/IssueTracker.UI/GlobalUsings.cs`
