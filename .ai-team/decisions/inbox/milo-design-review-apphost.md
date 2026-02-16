# 2026-02-16: AppHost Design Review

**By:** Milo  
**Context:** Setting up .NET Aspire orchestrator for IssueTracker  
**Participants:** Milo, Wolinski (Backend), Stansfield (Frontend)

---

## Key Decisions

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

---

## Action Items

| Owner | Action |
|-------|--------|
| Wolinski | Create AppHost project scaffold (Microsoft.NET.Sdk.Web, Aspire packages); add MongoDB ContainerResource and health checks |
| Wolinski | Add project references (CoreBusiness, Services, PlugIns.Mongo, UI) to AppHost; configure DI for Blazor UI integration |
| Stansfield | Update IssueTracker.slnx to include AppHost project; verify build order and IDE integration |
| Stansfield | Update local dev docs (getting-started.md) to reflect AppHost workflow instead of docker-compose |
| Milo | Review AppHost Program.cs and project structure against standards (file-scoped namespaces, async patterns, 120-char line limit) |

---

## Risks & Concerns

- **Risk**: Aspire service binding complexity — ensure CoreBusiness/Services DI registration aligns with Aspire conventions
- **Risk**: MongoDB health check timeout on first startup; may need adjusted wait policies in Aspire config
- **Concern**: testEnvironments.json conflict — need clear deprecation path or migration strategy
- **Concern**: Local dev debugging with Aspire orchestration may have latency on first launch; document expected startup time (~10-15s)
- **Blocker**: None identified. Proceed with AppHost scaffold.

---

## Notes

- All code must follow team standards: .NET 10, C# 14, file-scoped namespaces, nullable reference types, 120-char lines, tab indent (size 2)
- OpenAPI/Scalar docs required if any internal APIs are exposed through AppHost
- Integration tests (IssueTracker.PlugIns.Tests.Integration) should use TestContainers directly, not rely on Aspire for test isolation
