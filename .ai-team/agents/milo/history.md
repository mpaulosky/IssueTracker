# History — Milo

## Project Learnings (from init)

**Project:** IssueTracker  
**Tech Stack:** .NET 10, Blazor Server, MongoDB, Docker, xUnit, bUnit  
**Owner:** mpaulosky (GitHub: @mpaulosky)  
**Goal:** Aspireify the application and enhance/improve features  
**Team:** Milo (Lead), Stansfield (Frontend), Wolinski (Backend), Hooper (Tester), Scribe, Ralph  

**Standards:**

- C# 14, .NET 10 target framework
- File-scoped namespaces, PascalCase for types, camelCase for vars
- Async/await everywhere
- Nullable reference types enabled
- Pattern matching preferred
- Max line length: 120 chars
- Tab indent (size 2), LF line endings, UTF-8 charset
- All public APIs: OpenAPI/Scalar docs required (not Swagger UI)
- Integration tests with TestContainers (Docker isolation)
- Vertical slice architecture for features
- FluentValidation for model validation
- NSubstitute for mocks, FluentAssertions for assertions
- bUnit for Blazor component testing

## Learnings

### Aspire Foundation Architecture (2026-02-16)

**Project Naming:**

- Simplified AppHost naming from `IssueTracker.AppHost` → `AppHost` for consistency
- All Aspire orchestration projects follow simple naming convention (no namespace prefix)

**ServiceDefaults Design:**

- Chose shared project over NuGet package — simpler for monorepo, easier debugging
- Centralized infrastructure: OpenTelemetry, health checks, problem details, service discovery
- Entry point: `.AddServiceDefaults()` called by all projects in Program.cs
- MongoDB health check implemented as `IHealthCheck` — pings database before app readiness

**AppHost Architecture:**

- Single orchestration point for MongoDB + Blazor UI
- No separate API service — Minimal APIs embedded in UI project (vertical slice pattern)
- MongoDB as `ContainerResource` with SCRAM-SHA auth, exposed on 27018 externally for debugging
- UI waits for MongoDB health check before startup (`.WaitFor(mongodb)`)

**Key Tradeoffs:**

- **Shared project vs. NuGet:** Shared wins for monorepo simplicity, avoids premature packaging
- **Embedded APIs vs. separate service:** Embedded APIs reduce orchestration complexity, faster dev loop
- **Phase 1 scope:** Foundation only (logging, health, observability) — Auth0 and resilience deferred to Phase 2

**Risks Identified:**

- Developers may forget `.AddServiceDefaults()` — mitigate with architecture tests (NetArchTest)
- ServiceDefaults could become "kitchen sink" — enforce strict code review, only infrastructure concerns
- MongoDB container startup latency (10-15s) — already documented in getting-started.md

**Dependencies:**

- OpenTelemetry packages: `OpenTelemetry.Exporter.OpenTelemetryProtocol`, `OpenTelemetry.Extensions.Hosting`, `OpenTelemetry.Instrumentation.AspNetCore`
- Aspire MongoDB: `Aspire.Hosting.MongoDB` package for AppHost
- All packages centralized in Directory.Packages.props (enforced standard)
