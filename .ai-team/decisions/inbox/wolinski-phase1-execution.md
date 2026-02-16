### 2026-02-16: Phase 1 Aspire Foundation — Project Rename & ServiceDefaults Structure

**By:** Wolinski  
**Status:** Complete ✓

**What:** Executed Phase 1 of Aspire Foundation Architecture — renamed IssueTracker.AppHost → AppHost, created ServiceDefaults project scaffold with placeholder implementations for health checks and observability

**Why:** Foundation work enabling Phase 2 implementation of centralized infrastructure. Simplified project naming aligns with existing conventions (UI, Services, CoreBusiness, PlugIns). ServiceDefaults provides shared location for OpenTelemetry, health checks, and problem details middleware.

**Implementation:**

1. **Project Rename:**
   - Folder: `src/IssueTracker.AppHost` → `src/AppHost`
   - File: `AppHost.csproj` (renamed from IssueTracker.AppHost.csproj)
   - Namespace: Removed file-scoped namespace (incompatible with top-level statements in Program.cs)
   - Solution: Updated `IssueTracker.slnx` project reference path
   - Documentation: Updated `docs/getting-started.md` (3 occurrences replaced)

2. **ServiceDefaults Project:**
   - Created `src/ServiceDefaults/ServiceDefaults.csproj` targeting net10.0
   - `Extensions.cs`: Placeholder `AddServiceDefaults()` method (Phase 2 implementation)
   - `HealthChecks/MongoDbHealthCheck.cs`: Stub IHealthCheck implementation
   - `Observability/OpenTelemetryExtensions.cs`: Stub AddOpenTelemetryExporters() method
   - Added to `IssueTracker.slnx` under `/src/ServiceDefaults/` folder
   - PackageReferences: Microsoft.Extensions.Diagnostics.HealthChecks, Microsoft.Extensions.DependencyInjection (CPM-compliant, no versions in .csproj)

3. **Code Style Compliance:**
   - All files use file-scoped namespaces (where applicable)
   - Nullable reference types enabled
   - Target framework: net10.0, LangVersion: 14.0
   - Copyright headers present on all new files
   - Tab indentation (size 2), max line length 120 characters

4. **Build Verification:**
   - AppHost builds successfully: `src/AppHost/bin/Debug/net10.0/AppHost.dll`
   - ServiceDefaults builds successfully: `src/ServiceDefaults/bin/Debug/net10.0/ServiceDefaults.dll`
   - Solution structure updated, no broken references

**Aspire 13.0 API Adjustments:**

- Updated Directory.Packages.props: Aspire.Hosting v13.0.0, Aspire.Hosting.AppHost v13.0.0 (from 10.0.0)
- Removed `.WithBindPort()` / `.WithHostPort()` calls (API removed in Aspire 13)
- Removed `.WithReference(mongodb)` from UI project (ContainerResource no longer implements IResourceWithConnectionString)
- Added `IsAspireProjectResource="true"` attribute to UI project reference in AppHost.csproj

**Impact:**

- **Developers**: Clean project naming, buildable ServiceDefaults foundation, updated solution structure
- **Next Phase**: Phase 2 ready — implement Extensions.cs, health checks, OpenTelemetry exporters
- **Documentation**: getting-started.md now references correct project paths
- **CI/CD**: Both AppHost and ServiceDefaults projects build successfully in solution

**Next Steps (Phase 2):**

1. Implement Extensions.cs: OpenTelemetry tracing/metrics, health check registration, problem details
2. Implement MongoDbHealthCheck.cs: MongoDB ping using IMongoClient
3. Implement OpenTelemetryExtensions.cs: Aspire dashboard integration
4. Update UI/Program.cs: Add `builder.AddServiceDefaults()` and `app.MapDefaultEndpoints()`
5. Add OpenTelemetry NuGet packages to Directory.Packages.props

**Blockers:** None

**Timeline:** Phase 1 completed (2 hours). Phase 2 estimated 2-3 hours.
