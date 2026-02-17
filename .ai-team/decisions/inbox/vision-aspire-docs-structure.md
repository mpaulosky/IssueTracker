# Vision: Aspire Documentation Structure Plan

**Date:** 2026-02-16  
**Author:** Vision (Technical Writer)  
**Status:** Planning  
**Scope:** Documentation outline for Aspire integration and Redis caching feature

---

## Overview

The IssueTracker project is integrating .NET Aspire orchestration with Redis distributed caching. This document outlines the documentation structure needed to support developers and operators in understanding, configuring, and troubleshooting the new distributed system.

The actual documentation will be written in Phase 5 (Post-Implementation). This outline ensures:
- No documentation gaps after implementation
- Consistent structure across all docs
- Clear ownership of each topic
- Realistic scope of what needs documenting

---

## 1. Aspire Topology Documentation

**Purpose:** Help developers understand the system architecture, service relationships, and how components communicate.

### 1.1 System Architecture Overview
- **What to document:**
  - ASCII diagram or prose description of the Aspire topology
  - Service components: AppHost (orchestrator), Blazor UI (host application), MongoDB (database), Redis (cache)
  - Data flow: UI ↔ AppHost ↔ MongoDB; UI ↔ AppHost ↔ Redis
  - Network topology: Aspire internal DNS, port bindings, service discovery

- **Key visuals needed:**
  - Service dependency graph (AppHost as center, UI/MongoDB/Redis as resources)
  - Port mapping table (internal vs. external for dev/debug)
  - Process startup sequence (AppHost → health checks → services ready)

- **Audience:** Developers onboarding to the project; ops teams understanding deployment

### 1.2 Service Endpoints and Resource Configuration
- **What to document:**
  - Each resource definition in AppHost (MongoDB container, Redis container, Blazor UI project)
  - DNS names and how to reference them in code (`mongodb`, `redis`)
  - Port assignments (internal Aspire network, external debug ports, HTTPS ports)
  - Health check endpoints and readiness criteria
  - Connection string injection mechanism (how Aspire binds MongoDB/Redis to UI)

- **Code examples:**
  - AppHost Program.cs resource definitions (MongoDB, Redis, UI)
  - How UI project receives connection strings via dependency injection
  - Sample code: instantiating IDistributedCache in a controller

- **Audience:** Backend developers; infrastructure engineers setting up environments

### 1.3 Local vs. Production Topology Differences
- **What to document:**
  - Local topology: AppHost runs as dotnet process, containers managed by Docker Desktop
  - Production topology: AppHost published as container in orchestrator (AKS, Docker Compose, etc.)
  - Configuration differences (credentials, logging, telemetry)
  - Secrets management: dev (hardcoded safe defaults) vs. prod (User Secrets, Key Vault)

- **Audience:** DevOps/ops teams; developers deploying to staging/prod

---

## 2. Health Check Documentation

**Purpose:** Explain health monitoring system so developers and ops know when services are healthy and how to interpret failures.

### 2.1 Built-In Health Checks
- **What to document:**
  - MongoDB health check: ping database, expected responses, failure modes
  - Redis health check: ping cache server, expected responses, failure modes
  - Blazor UI health check: endpoint status, dependencies
  - Startup health checks: whether services wait for dependencies before considering "ready"

- **Health check states:**
  - Healthy (green): All connections working, latency acceptable
  - Degraded (yellow): Partial functionality, eventual consistency expected
  - Unhealthy (red): Service unavailable, requests may fail

- **Audience:** Ops teams monitoring production; developers debugging failed startups

### 2.2 Interpreting Health Check Results
- **What to document:**
  - How to check health endpoint: `GET /health`
  - Reading Aspire dashboard health indicators (real-time status)
  - JSON response structure of health endpoint (status, checks, details)
  - What each health check name means (`mongodb`, `redis`, `aspnetcore`)

- **Example outputs:**
  - Healthy state (all green)
  - MongoDB down (showing which service affected)
  - Redis timeout (degraded state)
  - Connection string misconfiguration

- **Audience:** Operators monitoring dashboards; support teams troubleshooting

### 2.3 Troubleshooting Health Check Failures
- **What to document:**
  - MongoDB health check fails:
    - Symptom: Dashboard shows red MongoDB indicator
    - Root causes: Container not running, credentials wrong, network isolation
    - Solutions: Restart container, verify credentials, check Docker network
    - Logs to check: AppHost console, MongoDB container logs

  - Redis health check fails:
    - Symptom: Cache operations timeout, UI may continue working (fallback)
    - Root causes: Container not running, port conflict, password wrong
    - Solutions: Restart Redis container, check port 6379 availability, verify connection string
    - Logs to check: Redis logs, AppHost ServiceDefaults logging

  - UI health check fails:
    - Symptom: Aspire shows UI red, browser cannot connect
    - Root causes: Port conflict (5000/5001), dependency initialization timeout, startup exception
    - Solutions: Kill process on ports 5000-5001, increase startup wait timeout, check UI logs for exceptions

- **Diagnostic commands:**
  - `dotnet run --project AppHost` (starting with verbose logging)
  - Docker: `docker ps` (verify containers), `docker logs mongodb`, `docker logs redis`
  - Aspire dashboard: real-time resource status and logs
  - MongoDB Compass: verify database connectivity
  - Redis CLI: `redis-cli ping`

- **Audience:** Developers troubleshooting local setup; ops teams investigating prod incidents

---

## 3. Cache Usage Guide

**Purpose:** Show developers where caching is used, how to use IDistributedCache, and patterns for cache invalidation.

### 3.1 Where Caching Is Used in IssueTracker
- **What to document:**
  - Current cached operations:
    - Issue list queries (cache by filter combination)
    - User profile data (cache by user ID)
    - Dashboard metrics (cache with 5-minute expiry)
  - Cache keys naming convention (e.g., `issues:filter:{filterId}`, `user:{userId}:profile`)
  - Expiry policies (absolute, sliding, per-operation)
  - Cache layers (L1 in-memory not used; L2 distributed via Redis only)

- **Audit checklist:** Which operations should be cached vs. shouldn't (auth tokens: no; static reference data: yes)

- **Audience:** Backend developers; architects reviewing caching strategy

### 3.2 How to Add New Cached Operations
- **What to document:**
  - Step-by-step: Identify cacheable operation → Define cache key → Wrap in IDistributedCache.GetAsync/SetAsync → Set expiry → Handle cache misses
  - Code pattern examples:

    ```csharp
    // Get from cache or compute
    string cacheKey = $"issues:list:{filter.Id}";
    var cached = await _cache.GetStringAsync(cacheKey);
    
    if (cached is not null)
    {
        return JsonSerializer.Deserialize<List<Issue>>(cached);
    }
    
    // Cache miss: load from DB
    var issues = await _repo.GetIssuesAsync(filter);
    await _cache.SetStringAsync(
        cacheKey,
        JsonSerializer.Serialize(issues),
        new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        }
    );
    return issues;
    ```

  - Dependency injection: `IDistributedCache` registered by ServiceDefaults
  - Testing cached operations: seeding cache in tests, verifying cache hits
  - Observability: logging cache hits/misses for monitoring

- **Decision points:**
  - When to cache (data accessed >2x/request, expensive to compute)
  - Expiry times (balance freshness vs. cache effectiveness)
  - Cache-aside vs. write-through patterns

- **Audience:** Developers implementing features; code reviewers

### 3.3 Cache Invalidation Patterns
- **What to document:**
  - Manual invalidation: `_cache.RemoveAsync(cacheKey)` when data changes
  - Tag-based invalidation: invalidate related caches together (e.g., removing an issue invalidates list cache)
  - Time-based expiry: automatic expiration (preferred for read-heavy data)
  - Event-driven invalidation: publish event on issue change, subscribe to invalidate caches

- **Anti-patterns:**
  - Caching without expiry (stale data forever)
  - Cache-busting on every request (defeats caching purpose)
  - Overcomplicating invalidation logic (use time-based for 80% of cases)

- **Consistency guarantees:**
  - Data consistency window: "data will be fresh within 5 minutes"
  - Document where inconsistency is acceptable (dashboard metrics OK to be stale; user auth not OK)

- **Observability:**
  - Log cache invalidation events
  - Monitor cache hit rate (should trend >70% for effective caching)
  - Alert on cache eviction storms (indicates undersized Redis)

- **Audience:** Backend developers; system architects

---

## 4. Running Aspire Locally

**Purpose:** Guide developers through starting the system, accessing dashboards, and diagnosing startup issues.

### 4.1 Prerequisites and Setup
- **What to document:**
  - System requirements: .NET 10 SDK, Docker Desktop (enabled), 4GB+ RAM, 2GB free disk
  - Verify installation: `dotnet --version`, `docker version`
  - User Secrets setup (for credentials if not using defaults): `dotnet user-secrets set`
  - Port availability check: Verify ports 5000, 5001, 15000 (Aspire dashboard), 27017 (MongoDB), 6379 (Redis) are free

- **Audience:** New developers onboarding; ops setting up lab environments

### 4.2 Starting AppHost
- **What to document:**
  - Command: `dotnet run --project src/AppHost`
  - Expected console output sequence:
    1. AppHost starting, initializing Aspire DistributedApplication
    2. MongoDB container launching
    3. Redis container launching
    4. Health checks running
    5. UI project (Blazor) starting
    6. "Application started. Press Ctrl+C to shut down"
  - Expected startup time: 10-15 seconds on first launch (containers pulling images), 3-5 seconds on subsequent launches
  - Console output to watch for errors (red text, exceptions)

- **Stopping AppHost:** Ctrl+C gracefully shuts down all services

- **Audience:** Developers starting local dev environment; CI/CD engineers

### 4.3 Accessing Aspire Dashboard
- **What to document:**
  - URL: `http://localhost:15000`
  - Dashboard tabs:
    - Resources: Shows MongoDB, Redis, Blazor UI status (green/yellow/red)
    - Logs: Real-time logs from each service
    - Metrics: Request counts, latency, error rates
    - Traces: Distributed tracing across services
  - Real-time status indicators: Green = healthy, Yellow = degraded, Red = failing
  - How to drill into a service (click resource name)
  - Viewing service endpoints (click resource → "Endpoints")

- **Example walkthrough:**
  - Accessing MongoDB logs to verify init script ran
  - Checking Redis is accepting connections
  - Viewing UI startup trace to find slow initialization

- **Audience:** Developers; anyone unfamiliar with Aspire dashboard

### 4.4 Accessing the Application
- **What to document:**
  - Blazor UI URL: `https://localhost:5001` (HTTPS) or `http://localhost:5000` (HTTP)
  - First request may be slow (app warming up, health checks running)
  - Browser cert warning for self-signed dev cert (normal, click "proceed")
  - Login via Auth0 (if feature enabled)
  - Verify basic functionality (create issue, view list, etc.)

- **Audience:** Developers running local setup

### 4.5 Troubleshooting Common Startup Issues
- **What to document:**

  - **Port already in use (5000/5001/15000):**
    - Symptom: "Port 5000 is already in use" error
    - Diagnosis: `netstat -ano | findstr LISTENING` (Windows)
    - Solutions: Kill process on port, change AppHost port in Program.cs, restart computer

  - **Docker daemon not running:**
    - Symptom: "Cannot connect to Docker daemon"
    - Diagnosis: Docker Desktop taskbar icon, check system tray
    - Solution: Start Docker Desktop, wait 30 seconds, retry

  - **Image pull timeout:**
    - Symptom: "Pull timeout: context deadline exceeded" or "No space left on device"
    - Diagnosis: Check internet, disk space (`docker system df`)
    - Solutions: Retry (pull again), free disk space, `docker system prune -a` to reclaim space

  - **MongoDB initialization fails:**
    - Symptom: Dashboard shows MongoDB red, logs show "Authentication failed"
    - Diagnosis: Check AppHost logs for "Connection string" errors, check default credentials (admin/admin)
    - Solutions: Verify connection string in AppHost, check MongoDB container logs, use MongoDB Compass to test connection

  - **Redis connection timeout:**
    - Symptom: Cache operations hang, dashboard shows Redis yellow, requests eventually timeout
    - Diagnosis: Check Redis container is running (`docker ps | grep redis`)
    - Solutions: Restart Redis (`docker-compose restart redis`), check Redis password in connection string, verify Redis port 6379 accessible

  - **Blazor UI stays red indefinitely:**
    - Symptom: Dashboard shows UI spinning/red for >30 seconds
    - Diagnosis: Check AppHost console for exceptions, check health endpoint manually (`curl http://localhost:5000/health`)
    - Solutions: Stop AppHost, kill lingering dotnet processes, check for port conflicts, review error logs

  - **"Timed out waiting for health check":**
    - Symptom: Aspire logs show health check timeout before declaring UI ready
    - Diagnosis: Startup is slow (network I/O, first-run initialization)
    - Solutions: Increase Aspire health check timeout in AppHost, check machine specs (use more powerful machine), verify no background jobs running

- **Diagnostic commands to document:**
  - `docker ps` — List running containers
  - `docker logs {container-name}` — View service logs
  - `curl http://localhost:5000/health` — Check UI health endpoint (raw response)
  - `redis-cli -h localhost ping` — Test Redis connectivity
  - Check AppHost console output for exception stack traces

- **When to escalate:**
  - Issue persists after all steps → Check GitHub Issues or ask team
  - Suspecting system-level problem (network, DNS) → Verify with IT

- **Audience:** New developers; anyone troubleshooting local setup issues

---

## 5. Production Readiness

**Purpose:** Guide ops teams and architects in configuring and monitoring Aspire in production environments.

### 5.1 Health Check Expectations in Production
- **What to document:**
  - Health check endpoints must be available on all services before load balancer directs traffic
  - Aspire orchestrator's readiness probes: Dependencies must all be healthy (MongoDB, Redis) before UI is marked ready
  - Health check interval/timeout tuning:
    - Typical: Check every 10 seconds, timeout after 5 seconds
    - Tune based on: Database latency, cache latency, acceptable false-positive rate
  - Expected health check response times:
    - MongoDB ping: <100ms (local network)
    - Redis ping: <50ms (local network)
    - UI app startup: <10 seconds (with dependencies ready)

- **Monitoring SLOs:**
  - Health check success rate: Maintain >99% (alert if dropping below 98%)
  - Time to healthy: <30 seconds from container start
  - Degraded state transitions: Document why degraded might happen (transient network hiccup) vs. unhealthy (persistent failure)

- **Audience:** Ops/SRE teams; load balancer/orchestrator administrators

### 5.2 Redis Backup, Persistence, and High Availability
- **What to document:**
  - Production Redis configuration:
    - Persistence: Enable RDB snapshots (save every 1 minute) or AOF (write-ahead log)
    - Replication: Master-replica setup for HA (if using Redis Enterprise/managed service)
    - Password protection: Strong password required (not hardcoded defaults)
    - TLS: Enable if transmitting across networks

  - Backup strategy:
    - Automated daily RDB snapshots to object storage (S3, Blob)
    - Retention: Keep 7 daily snapshots, 4 weekly, 1 monthly
    - Test recovery: Quarterly restore-and-test from backup

  - Data loss tolerance:
    - Cache data is not critical (can be recomputed from DB)
    - Acceptable data loss window: Up to 1 minute (RDB interval)
    - Unacceptable: Loss of user session data (if cached) — use persistent session store

  - HA setup:
    - Redis cluster (3+ nodes) vs. managed service (recommended)
    - Failover time: Auto-failover should complete <10 seconds
    - Sentinels (if self-hosted): Monitor Redis health, trigger failover

  - Connection pooling:
    - StackExchange.Redis (or similar) manages connection pool
    - Monitor pool exhaustion (should never happen with correct configuration)

- **Audience:** DevOps engineers; database administrators; architects

### 5.3 Monitoring and Observability Setup
- **What to document:**
  - Aspire metrics exposed:
    - Request count, latency (p50, p95, p99)
    - Cache hit rate (should be >70% for effective caching)
    - Health check latency (alert if >1 second)
    - Service availability (uptime %)

  - Logging aggregation:
    - All AppHost services log to centralized sink (Application Insights, ELK, Splunk)
    - Structured logging: JSON format with correlation IDs for tracing across services
    - Log levels: Info for normal operations, Warning/Error for issues

  - Tracing setup:
    - OpenTelemetry exporters configured to Application Insights (or Jaeger)
    - Distributed traces show end-to-end flow: UI request → AppHost → MongoDB/Redis → response
    - Latency SLIs: P95 latency <200ms, P99 latency <500ms

  - Alerting rules:
    - MongoDB health check failing >2 consecutive checks → Alert (page ops)
    - Redis health check degraded >5 minutes → Alert (page ops)
    - Error rate >0.1% → Alert (email, investigate)
    - Cache hit rate <50% → Alert (email, investigate caching effectiveness)

  - Dashboard setup:
    - Real-time service status (Aspire dashboard or custom dashboard)
    - Health check status per service
    - Request latency trends
    - Cache effectiveness (hit rate, eviction rate)
    - Error rate by endpoint

- **Dashboards to create:**
  - Overview: System status, SLO compliance, alert summary
  - Service details: Per-service metrics, logs, traces
  - Cache analysis: Hit rate, key distribution, eviction patterns
  - Database: Connection pool, query latency, slow query log

- **Audience:** Ops/SRE teams; on-call engineers; architects defining SLOs

### 5.4 Environment-Specific Configuration
- **What to document:**
  - Configuration layers:
    - Development: Hardcoded safe defaults (admin/admin for MongoDB, no password for Redis)
    - Staging: User Secrets or config files (test data, controlled environment)
    - Production: Secrets from Key Vault (strong credentials, restricted access)

  - Configuration sources (in priority order):
    1. Environment variables (set by orchestrator)
    2. User Secrets (development only, never committed)
    3. appsettings.{Environment}.json (checked into repo, no secrets)
    4. appsettings.json (baseline defaults)

  - Credentials management:
    - MongoDB credentials (dev vs. prod different)
    - Redis password (prod only, no password in dev)
    - Connection strings never hardcoded (injected by Aspire)

  - Feature flags:
    - Enable/disable Redis caching: `ASPIRE_CACHE_ENABLED=true|false`
    - Cache expiry times: Tunable per environment
    - Logging levels: Debug in dev, Info in prod

- **Audience:** DevOps engineers; platform teams; anyone deploying to prod

### 5.5 Deployment Checklist
- **What to document:**
  - Pre-deployment validation:
    - ✓ Health checks green in staging
    - ✓ Load test: Cache hit rate >70%
    - ✓ Backup: Redis backup created and tested
    - ✓ Secrets: All production credentials in Key Vault
    - ✓ Monitoring: Dashboards, alerts configured

  - Deployment process:
    1. Blue-green: Deploy to green environment, run smoke tests
    2. Health checks: Wait for all resources healthy (timeout 5 min)
    3. Smoke test: Simple request flow (create issue → view → verify cache working)
    4. Traffic shift: Gradually shift traffic from blue to green (10% → 50% → 100% over 10 minutes)
    5. Rollback plan: Revert traffic shift if error rate >0.5%

  - Post-deployment validation:
    - ✓ All services healthy
    - ✓ Error rate <0.1%
    - ✓ Latency p99 <500ms
    - ✓ Cache hit rate >70%
    - ✓ Logs show no errors in first 5 minutes

- **Audience:** DevOps engineers; release managers; on-call operators

---

## Documentation Delivery Plan

| Phase | Content | Owner | Timeline |
|-------|---------|-------|----------|
| **Phase 4** | Implement Aspire + Redis integration | Wolinski (Backend), Shuri (Caching) | In progress |
| **Phase 5** | Write full documentation from outline | Vision (Technical Writer) | After implementation complete |
| **Phase 5** | Create ASCII diagrams and topology visuals | Vision | During Phase 5 |
| **Phase 5** | Write troubleshooting guide with real errors | Vision | During Phase 5 |
| **Phase 5** | Create ops/deployment runbooks | Vision + Milo (DevOps) | During Phase 5 |
| **Phase 5** | Review with Stansfield (Frontend), Hooper (QA) | Vision | End of Phase 5 |
| **Phase 6+** | Maintain/update as system evolves | Vision | Ongoing |

---

## Next Steps

1. **Implementation team (Wolinski, Shuri):** Continue Aspire + Redis integration per `.ai-team/decisions.md`
2. **Vision:** Monitor implementation for documentation-specific questions, edge cases
3. **After Phase 4:** Vision begins fleshing out sections 1-5 with real code examples, actual command outputs, real error scenarios
4. **Coordination:** Milo coordinates review of ops sections with DevOps team before finalization

---

## Document Index (For Phase 5)

Files to create in `docs/`:

- `docs/aspire-topology.md` — Sections 1.1-1.3
- `docs/health-checks.md` — Sections 2.1-2.3
- `docs/caching-guide.md` — Sections 3.1-3.3
- `docs/running-aspire-locally.md` — Sections 4.1-4.5
- `docs/production-readiness.md` — Sections 5.1-5.5
- `README.md` — Update with Aspire + Redis overview (cross-link to detailed docs)

All docs will follow markdown standards from `.github/instructions/markdown.instructions.md`:
- Proper heading hierarchy (H2/H3)
- Code blocks with language syntax highlighting
- Tables for reference data
- Clear structure: Overview → How-To → Examples → Troubleshooting
- XML comment blocks for code examples
- Line length <120 characters for readability

---

**Status:** Ready for implementation phase handoff to Vision (Phase 5).
