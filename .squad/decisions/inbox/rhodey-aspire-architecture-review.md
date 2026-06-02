---
date: 2026-02-16
author: Rhodey
status: Decision
---

# Aspire Architecture Review & Redis Integration Plan

## Current State Summary

**✓ What's Working Well**

The Aspire foundation is **sound and intentional**. AppHost orchestrates MongoDB with health checks, ServiceDefaults centralizes infrastructure concerns (OTel, health checks, problem details), and the UI project correctly wires everything via `AddServiceDefaults()` and `MapDefaultEndpoints()`. The team followed conventions over configuration — MongoDB is a `ContainerResource` with SCRAM-SHA auth, connection strings are injected via Aspire binding, and port assignments are explicit (5000/5001 for UI, 27017 internal for MongoDB).

Key strengths:
- **AppHost is the single orchestration entry point.** MongoDB container, health checks, and UI service are cleanly separated.
- **ServiceDefaults applies consistently.** UI project calls `AddServiceDefaults()` before other registrations, and `MapDefaultEndpoints()` is in the pipeline.
- **OpenTelemetry is production-ready.** Sampling strategy is in place (AlwaysOn for dev, 10% ratio for prod), all three signals configured (metrics, tracing), and OTLP exporter points to Aspire dashboard.
- **Health checks are correctly structured.** MongoDB ping timeout is 3 seconds, cancellation handling is robust, and the health check endpoint is mapped at `/health`.

**⚠️ Gaps to Address**

1. **No Redis in Directory.Packages.props** — Aspire Redis support exists but is not declared.
2. **ServiceDefaults does not register a cache provider** — only health checks for MongoDB; no caching infrastructure.
3. **No cache invalidation strategy documented** — where and how cache entries expire is undefined.
4. **No explicit readiness/liveness health check distinction** — AppHost treats all health checks the same; no differentiation between startup-required and runtime checks.

---

## Redis Integration Plan

### Step 1: Add Redis Package to Directory.Packages.props

**Decision:** Add `Aspire.Hosting.Redis` v13.0.0 (matching existing Aspire v13.0.0)

```xml
<PackageVersion Include="Aspire.Hosting.Redis" Version="13.0.0" />
<PackageVersion Include="Microsoft.Extensions.Caching.StackExchangeRedis" Version="10.0.0" />
```

**Rationale:**
- Pins Redis hosting to the same Aspire minor version for consistency.
- `Microsoft.Extensions.Caching.StackExchangeRedis` provides the `IDistributedCache` abstraction for .NET 10.

### Step 2: Add Redis to AppHost

Update `src/AppHost/Program.cs`:

```csharp
var mongodb = builder.AddMongoDB("mongodb")
    .WithDataVolume()
    .WithHealthCheck("mongodb");

var redis = builder
    .AddRedis("redis")
    .WithHealthCheck("redis")
    .WithDataVolume();

var ui = builder
    .AddProject<Projects.IssueTracker_UI>("ui")
    .WithReference(mongodb)
    .WithReference(redis);
```

**Decision:** Use `WithHealthCheck()` on Redis; health check is **startup-required** (same as MongoDB).

**Rationale:** If Redis is unavailable at startup, the app cannot serve cached responses and degrades to database queries under load. Not optional.

### Step 3: Register IDistributedCache in ServiceDefaults

Add to `src/ServiceDefaults/Extensions.cs`:

```csharp
public static IHostApplicationBuilder AddServiceDefaults(this IHostApplicationBuilder builder)
{
    // ... existing OTel and MongoDB health check code ...

    // Distributed Cache (Redis)
    builder.AddRedisDistributedCache("redis");

    return builder;
}
```

**Why:** AppHost passes Redis connection string via Aspire binding; `AddRedisDistributedCache()` consumes it automatically.

### Step 4: Update AppHost.csproj

Add package reference:

```xml
<PackageReference Include="Aspire.Hosting.Redis" />
```

### Step 5: Verify ServiceDefaults Project Reference in UI

Confirm UI.csproj includes:

```xml
<ProjectReference Include="..\..\ServiceDefaults\ServiceDefaults.csproj" />
```

✓ Already present.

---

## Cache Strategy

### Where Caching Should Be Applied

**Tier 1: Query Results (Blazor Component Level)**

- **What:** Cache frequently accessed Issue lists, filters, and search results.
- **How:** Inject `IDistributedCache` into service classes; store JSON serialized data.
- **TTL:** 5 minutes for list queries, 10 minutes for filtered results.
- **Implementation Pattern:**

```csharp
var cacheKey = $"issues:list:{userId}:page-{page}";
var cached = await cache.GetAsync<IEnumerable<IssueDto>>(cacheKey);
if (cached != null) return cached;

var issues = await repository.GetIssuesAsync(userId, page);
await cache.SetAsync(cacheKey, issues, new() { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) });
return issues;
```

**Tier 2: Rendered Components (Output Caching)**

- **What:** Cache full HTTP responses for static pages (e.g., dashboards, read-only views).
- **How:** Use ASP.NET Core output caching middleware.
- **TTL:** 10-15 minutes for dashboards, user-specific pages exempt.
- **Implementation Pattern:**

```csharp
app.UseOutputCache();

// In endpoints
app.MapGet("/dashboard", DashboardHandler)
    .CacheOutput(c => c
        .Expire(TimeSpan.FromMinutes(15))
        .WithTag("dashboard"));
```

**Tier 3: Session Data (Blazor Server)**

- **What:** Cache user preferences, permission checks, and feature flags.
- **How:** Use Blazor cascading parameters + `IDistributedCache` for multi-tab consistency.
- **TTL:** Session lifetime (e.g., 1 hour, refresh on activity).
- **Implementation Pattern:**

Use existing `Blazored.SessionStorage` for client-side, Redis for server-side session state if multi-instance deployment is needed.

---

## Health Check Architecture

### Startup-Required vs. Optional Checks

**Decision:** All infrastructure dependencies are **startup-required** (current approach is correct).

**Rationale:**
- MongoDB: Database unavailable = app cannot function.
- Redis: Cache unavailable = app falls back to database (acceptable but degraded).

**BUT:** Distinguish checks in health response for observability.

### Recommended Health Check Structure

Update `src/ServiceDefaults/Extensions.cs`:

```csharp
builder.Services.AddHealthChecks()
    .AddCheck<MongoDbHealthCheck>("mongodb", tags: ["startup"])
    .AddCheck<RedisHealthCheck>("redis", tags: ["startup"])
    .AddCheck<ApplicationHealthCheck>("app", tags: ["liveness"]);

// In MapDefaultEndpoints():
app.MapHealthChecks("/health", new() { IncludedTags = ["startup"] });
app.MapHealthChecks("/health/live", new() { IncludedTags = ["liveness"] });
```

**Why:**
- `/health` → readiness endpoint (Aspire startup gate).
- `/health/live` → liveness endpoint (Kubernetes/container orchestrators use for restart decisions).

### Redis Health Check Implementation

Create `src/ServiceDefaults/HealthChecks/RedisHealthCheck.cs`:

```csharp
public sealed class RedisHealthCheck : IHealthCheck
{
    private readonly IConnectionMultiplexer _redis;
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(2);

    public RedisHealthCheck(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var cts = new CancellationTokenSource(Timeout);
            using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);

            var db = _redis.GetDatabase();
            await db.PingAsync();
            return HealthCheckResult.Healthy("Redis connection is responsive");
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            return HealthCheckResult.Unhealthy("Redis health check cancelled");
        }
        catch (OperationCanceledException)
        {
            return HealthCheckResult.Unhealthy($"Redis connection timed out after {Timeout.TotalSeconds}s");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Redis connection failed", ex);
        }
    }
}
```

Register in Extensions.cs:

```csharp
.AddCheck<RedisHealthCheck>("redis", tags: ["startup"])
```

---

## NuGet Package Decision

**Decision: Aspire.Hosting.Redis v13.0.0**

| Package | Current | New | Reason |
|---------|---------|-----|--------|
| `Aspire.Hosting` | 13.0.0 | — | No change; established baseline |
| `Aspire.Hosting.Redis` | — | 13.0.0 | **New**; matches Aspire minor version |
| `Microsoft.Extensions.Caching.StackExchangeRedis` | — | 10.0.0 | **New**; aligned with .NET 10 |

**Rationale:**
- v13.0.0 aligns with existing Aspire v13.0.0; avoids version skew.
- StackExchangeRedis v10.0.0 is the .NET 10-compatible release.
- Centralized Package Management enforced: no version specs in project files.

---

## Next Phase Handoff to Shuri (Backend)

### Implementation Order

**Phase A: Foundation (3-4 hours)**

1. Update `Directory.Packages.props` with Redis packages.
2. Add Redis resource to `AppHost/Program.cs` with `WithHealthCheck()` and `WithDataVolume()`.
3. Create `RedisHealthCheck.cs` in `ServiceDefaults/HealthChecks/`.
4. Register `AddRedisDistributedCache("redis")` in `ServiceDefaults/Extensions.cs`.
5. Verify `IDistributedCache` injection works via integration test.

**Phase B: Cache Implementation (4-6 hours)**

6. **Query Result Caching:** Identify top 3 hot queries (e.g., recent issues, user issues). Wrap in `IDistributedCache` with 5-minute TTL.
7. **Output Caching:** Mark read-only endpoints (e.g., GET /issues, GET /dashboard) with `[OutputCache]` attribute; 10-minute TTL.
8. **Session Caching:** If multi-instance deployment is planned, migrate Blazor session data to Redis (optional for Phase B; can defer to Phase C).

**Phase C: Observability & Testing (2-3 hours)**

9. Add logging to cache hits/misses for monitoring.
10. Write integration tests: Redis unavailable → fallback to database gracefully.
11. Document cache invalidation strategy (cache keys, TTLs, event-driven cleanup).

### Acceptance Criteria

- ✓ AppHost starts with Redis and MongoDB both healthy.
- ✓ Cache misses result in database queries; hits bypass database.
- ✓ Aspire dashboard shows Redis with green health check.
- ✓ `/health` endpoint reports Redis status.
- ✓ Integration test verifies Redis failover (app still works if Redis is down).

### Build Order

1. ServiceDefaults → AppHost → UI (ServiceDefaults is a dependency of UI)

---

## Blockers & Open Questions

**None.** All dependencies are present, NuGet packages are available, and the architectural pattern is established.

### Known Risks

1. **Redis data persistence:** `WithDataVolume()` is configured; ensure volume cleanup is documented for local dev.
2. **Cache key collisions:** Document naming convention (e.g., `{domain}:{entity}:{id}:{variant}`).
3. **TTL strategy:** Define how cache entries expire — use absolute expirations for predictability over sliding windows in distributed scenarios.

---

## Summary

IssueTracker's Aspire foundation is **architecturally sound.** Redis integration is straightforward: add packages to Directory.Packages.props, register in AppHost and ServiceDefaults, implement caching at query and HTTP response tiers, and test failover. All infrastructure checks (MongoDB, Redis) are startup-required; health check endpoints distinguish readiness (`/health`) from liveness (`/health/live`). Shuri should follow Phase A → Phase B → Phase C for a clean, testable rollout.

No regressions expected; all changes are additive.
