# History — Hooper

## Project Learnings (from init)

**Project:** IssueTracker  
**Test Stack:** xUnit, bUnit, FluentAssertions, NSubstitute, TestContainers  
**Owner:** mpaulosky  
**Goal:** Aspireify and enhance with robust test coverage  

**xUnit Patterns:**
- Test method: `MethodName_Condition_ExpectedResult`
- Assertions: FluentAssertions (`x.Should().Be(expected)`, `x.Should().Throw<Exception>()`)
- Mocks: NSubstitute (`Substitute.For<IInterface>()`)
- Arrange-Act-Assert (AAA) pattern
- File-scoped namespaces, PascalCase test classes

**bUnit Patterns:**
- Component tests in `.razor.cs` files
- Render component: `var cut = RenderComponent<IssueComponent>(parameters => ...)`
- Test interactions: `cut.Find("button").Click()`
- Snapshot tests: `await cut.VerifyAsync()`
- Event callbacks: `cut.Instance.OnSubmit` setup

**Integration Tests:**
- TestContainers: Docker MongoDB for isolation
- Build container in test setup, clean up in teardown
- Test full flow: API → Service → Repository → Database

**Coverage Goals:**
- >80% coverage on business logic
- 100% on critical paths
- All public APIs: ≥1 integration test
- All components: ≥1 bUnit test

## Learnings

### 2026-02-16: Phase 1 Build Verification & Solution Validation

**Task:** Verify Wolinski's Phase 1 implementation (AppHost rename + ServiceDefaults creation).

**Build Verification Process:**
1. Full solution build: `dotnet build IssueTracker.slnx` — ✅ SUCCESS (exit code 0, 2.2s build time)
2. Individual project builds: AppHost (0.4s), ServiceDefaults (0.9s) — ✅ CLEAN
3. Spot-check existing test project: CoreBusiness.Tests.Unit — ✅ NO REGRESSIONS

**Project Structure Observations:**
- ✅ Solution file (`IssueTracker.slnx`) correctly references both `/src/AppHost/AppHost.csproj` and `/src/ServiceDefaults/ServiceDefaults.csproj`
- ✅ `dotnet sln list` confirms 12 projects total (2 new: AppHost, ServiceDefaults)
- ✅ ServiceDefaults targets `net10.0`, nullable enabled, C# 14 (`LangVersion>14.0</LangVersion>`)
- ✅ AppHost.csproj correctly configured as `.NET.Sdk.Web` with Aspire packages
- ⚠️ **Namespace observation:** AppHost/Program.cs uses top-level statements (no namespace declaration) — this is valid for entry point files and aligns with modern C# conventions
- ✅ ServiceDefaults follows file-scoped namespace pattern (`namespace ServiceDefaults;`, `namespace ServiceDefaults.HealthChecks;`)

**Phase 1 Project References:**
- AppHost correctly references: IssueTracker.UI, Services, CoreBusiness, PlugIns
- ServiceDefaults has minimal dependencies: HealthChecks + DependencyInjection packages (Phase 1 stubs)
- All project references valid, no broken dependencies

**Code Quality Compliance:**
- ✅ File copyright headers present (verified on Extensions.cs, GlobalUsings.cs, MongoDbHealthCheck.cs, OpenTelemetryExtensions.cs)
- ✅ XML documentation on public APIs (stub implementations note Phase 2 completion)
- ✅ File-scoped namespaces used (ServiceDefaults.*) where applicable
- ✅ Centralized Package Management (CPM): All packages reference Directory.Packages.props (no inline versions)

**Build Output Quality:**
- Zero errors, zero warnings (only one informational warning: TestingSupport.Library EnableIntermediateOutputPathMismatchWarning)
- Clean incremental build behavior observed
- No breaking changes to existing projects (CoreBusiness, Services, UI, PlugIns all built successfully)

**Phase 1 Quality Gate Status: ✅ PASS**

All success criteria met:
1. ✅ Full solution build succeeds with NO errors
2. ✅ AppHost and ServiceDefaults discoverable in solution file
3. ✅ No broken project references
4. ✅ All existing projects still build (CoreBusiness, Services, UI, PlugIns, tests)
5. ✅ ServiceDefaults.csproj follows code standards (net10.0, nullable, C# 14)

**Recommendations for Phase 2:**
- ServiceDefaults currently contains stub implementations — Phase 2 should implement actual OpenTelemetry tracing/metrics and MongoDB health check logic
- Consider adding architecture tests (NetArchTest) to validate `.AddServiceDefaults()` is called in all projects (per decisions.md risk mitigation)
- No blocking issues found — team can proceed to Phase 2
