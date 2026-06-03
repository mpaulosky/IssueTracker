## Production Readiness Guide

### Overview

This guide outlines best practices and configurations for deploying Issue Tracker to production
environments. It covers Redis persistence, cache behavior, health checks, monitoring, and
performance tuning.

### Redis Persistence & Data Safety

In production, Redis must persist data to survive restarts and failures.

#### Persistence Strategies

##### RDB (Snapshot) - Default

**How it works**: Periodically saves entire dataset to disk

**Configuration**:

```yaml
# docker-compose.yml for production
services:
  redis:
    image: redis:7-alpine
    command: redis-server --save 60 1000 --appendonly no
    volumes:
      - redis-data:/data
    ports:
      - "6379:6379"

volumes:
  redis-data:
    driver: local
```

**Options**:

- `--save 60 1000` - Save every 60 seconds if 1000+ keys changed
- Adjust to `--save 300 10` for less frequent saves (slower recovery, better performance)

**Pros**: Simple, low overhead

**Cons**: Can lose data between snapshots

##### AOF (Append-Only File) - Safer

**How it works**: Logs every write command, replays on recovery

**Configuration**:

```yaml
services:
  redis:
    image: redis:7-alpine
    command: redis-server --appendonly yes --appendfsync everysec
    volumes:
      - redis-data:/data
    ports:
      - "6379:6379"
```

**Options**:

- `--appendfsync everysec` - Fsync once per second (balanced safety/performance)
- `--appendfsync always` - Fsync after every write (safest, slowest)
- `--appendfsync no` - Let OS decide when to fsync (fastest, riskier)

**Pros**: Safer, minimal data loss

**Cons**: Slower writes, larger disk footprint

##### Hybrid (RDB + AOF) - Recommended

**Configuration**:

```yaml
services:
  redis:
    image: redis:7-alpine
    command: |
      redis-server
        --save 60 1000
        --appendonly yes
        --appendfsync everysec
    volumes:
      - redis-data:/data
    ports:
      - "6379:6379"
```

On recovery, Redis uses AOF first (more recent), then RDB if AOF unavailable.

### Redis Replication (High Availability)

For production with downtime requirements, deploy Redis in a replicated setup:

```yaml
version: '3.8'
services:
  redis-primary:
    image: redis:7-alpine
    command: redis-server --port 6379
    volumes:
      - redis-primary:/data
    ports:
      - "6379:6379"

  redis-replica:
    image: redis:7-alpine
    command: redis-server --port 6380 --slaveof redis-primary 6379
    depends_on:
      - redis-primary
    volumes:
      - redis-replica:/data
    ports:
      - "6380:6380"

volumes:
  redis-primary:
  redis-replica:
```

**Application Configuration**:

```csharp
// Connects to primary for writes, can read from replicas
var options = new StackExchange.Redis.ConfigurationOptions
{
  EndPoints = { "redis-primary:6379", "redis-replica:6380" },
  TieBreaker = "",
  ServiceName = "mymaster"
};

var connection = await StackExchange.Redis.ConnectionMultiplexer.ConnectAsync(options);
```

### Cache Behavior: Local vs. Production

#### Local Development (AppHost)

- **Scope**: Single developer machine
- **Persistence**: Volumes created/destroyed with AppHost
- **TTLs**: Short (5-10 minutes) for rapid iteration
- **Invalidation**: Manual (restart AppHost to clear all cache)
- **Monitoring**: Aspire dashboard provides visibility

#### Production

- **Scope**: Multiple servers, distributed load
- **Persistence**: Persistent volumes (RDB/AOF)
- **TTLs**: Longer (30+ minutes) for cost/performance optimization
- **Invalidation**: Careful coordination (invalidate only what changed)
- **Monitoring**: OpenTelemetry metrics, alerting on cache misses

#### Key Differences

| Aspect | Local | Production |
|--------|-------|------------|
| **Replication** | Single instance | Master-slave or cluster |
| **Failover** | Manual restart | Automatic (Redis Sentinel or Cluster) |
| **Data Persistence** | Docker volume | Persistent storage + backups |
| **TTL Strategy** | Aggressive (fast iteration) | Conservative (cost/consistency) |
| **Invalidation** | Full cache wipes OK | Surgical, event-driven |

### Health Check Configuration for Production

Health checks must be configured differently for startup vs. ongoing operation:

#### Startup Phase (High Confidence Required)

During container startup, services must be "ready" before accepting traffic:

```csharp
// In Program.cs
var healthChecks = builder.Services.AddHealthChecks()
  .AddCheck<MongoDbHealthCheck>(
    "mongodb",
    HealthStatus.Unhealthy,  // Fail on any error
    tags: new[] { "startup" }
  )
  .AddCheck<RedisHealthCheck>(
    "redis",
    HealthStatus.Unhealthy,  // Fail on any error
    tags: new[] { "startup" }
  );
```

Kubernetes probe:

```yaml
readinessProbe:
  httpGet:
    path: /health
    port: 5000
  initialDelaySeconds: 30  # Wait for startup
  periodSeconds: 10
  failureThreshold: 3
```

#### Ongoing Phase (Graceful Degradation)

Once running, services should degrade rather than fail:

```csharp
// After startup, mark as "Degraded" instead of "Unhealthy"
var healthChecks = builder.Services.AddHealthChecks()
  .AddCheck<MongoDbHealthCheck>(
    "mongodb",
    HealthStatus.Degraded,  // Non-fatal issues
    tags: new[] { "liveness" }
  )
  .AddCheck<RedisHealthCheck>(
    "redis",
    HealthStatus.Degraded,   // Cache is optional, not critical
    tags: new[] { "liveness" }
  );
```

### Monitoring & Observability

#### OpenTelemetry Metrics Collection

Issue Tracker exports metrics via OpenTelemetry. Configure exporters in production:

```csharp
// In ServiceDefaults/Extensions.cs (already implemented)
builder.Services.AddOpenTelemetry()
  .WithMetrics(metrics => metrics
    .AddAspNetCoreInstrumentation()
    .AddHttpClientInstrumentation()
    .AddOtlpExporter(options =>
    {
      options.Endpoint = new Uri("http://otel-collector:4317");
    })
  );
```

**Metrics to Export**:

- Cache hit/miss rates
- MongoDB query latency
- Redis command latency
- HTTP request latency
- Error rates per service

#### Prometheus Scraping

Configure Prometheus to scrape metrics endpoint:

```yaml
global:
  scrape_interval: 15s

scrape_configs:
  - job_name: 'issuetracker'
    static_configs:
      - targets: ['issuetracker-ui:5000']
    metrics_path: '/metrics'
```

#### Application Insights (Azure)

For Azure deployments, configure Application Insights:

```csharp
builder.Services.AddApplicationInsightsTelemetry();

builder.Services.ConfigureOpenTelemetryMeterProvider(metrics =>
  metrics.AddAzureMonitorMetricExporter()
);
```

### Performance Tuning

#### Cache TTL Optimization

Analyze cache hit/miss rates and adjust TTLs:

**Strategy**:

- **High Hit Rate (>80%)**: TTL is good, no change needed
- **Low Hit Rate (<50%)**: Increase TTL (data is reusable longer)
- **Stale Data Complaints**: Decrease TTL (data changes more frequently)

**Recommended Production TTLs**:

```csharp
// Query results: Re-execute queries every 30 minutes
const int QueryResultsTTL = 30;

// Output/reports: Re-render every 60 minutes
const int ReportTTL = 60;

// Session data: Keep user prefs for 24 hours
const int SessionTTL = 24 * 60;
```

#### Redis Memory Management

Configure Redis memory limits and eviction policy:

```yaml
services:
  redis:
    image: redis:7-alpine
    command: |
      redis-server
        --maxmemory 512mb
        --maxmemory-policy allkeys-lru
    ports:
      - "6379:6379"
```

**Policies**:

- `allkeys-lru` - Evict least recently used keys when limit reached
- `volatile-lru` - Evict keys with TTL when limit reached
- `allkeys-random` - Random eviction

**Monitoring**:

```bash
# Check memory usage
redis-cli INFO memory

# Expected output:
# used_memory_human:125.42M
# maxmemory:512000000
```

#### Database Query Optimization

Ensure MongoDB indexes are created for frequently-queried fields:

```csharp
// In migration or setup
var collection = database.GetCollection<Issue>("issues");
var indexModel = new CreateIndexModel<Issue>(
  Builders<Issue>.IndexKeys.Ascending(x => x.CreatedBy)
);
await collection.Indexes.CreateOneAsync(indexModel);
```

**Verify indexes**:

```bash
mongosh --host localhost --port 27017
use devissuetracker
db.issues.getIndexes()
```

### Backup & Disaster Recovery

#### MongoDB Backups

Schedule daily backups using `mongodump`:

```bash
#!/bin/bash
# backup-mongodb.sh
mongodump \
  --host mongodb-prod:27017 \
  -u admin -p $MONGO_PASSWORD \
  --out /backups/mongo-$(date +%Y%m%d)
```

#### Redis Backups

Copy RDB/AOF files to persistent storage:

```bash
#!/bin/bash
# backup-redis.sh
docker exec redis-prod redis-cli BGSAVE
docker cp redis-prod:/data/dump.rdb /backups/dump-$(date +%Y%m%d).rdb
```

#### Recovery Procedures

**MongoDB Recovery**:

```bash
mongorestore \
  --host mongodb-prod:27017 \
  -u admin -p $MONGO_PASSWORD \
  /backups/mongo-20240101
```

**Redis Recovery**:

```bash
docker cp /backups/dump-20240101.rdb redis-prod:/data/dump.rdb
docker restart redis-prod
```

### Scaling Strategies

#### Horizontal Scaling (Multiple UI Instances)

Use load balancer in front of multiple UI instances:

```yaml
services:
  loadbalancer:
    image: nginx:latest
    ports:
      - "80:80"
    volumes:
      - ./nginx.conf:/etc/nginx/nginx.conf

  ui-1:
    image: issuetracker-ui:latest
    depends_on:
      - mongodb
      - redis

  ui-2:
    image: issuetracker-ui:latest
    depends_on:
      - mongodb
      - redis
```

#### Redis Cluster (Horizontal Cache)

For massive cache volumes, use Redis Cluster:

```yaml
services:
  redis-cluster:
    image: redis:7-alpine
    command: redis-server --cluster-enabled yes
    environment:
      - REDIS_CLUSTER_NODES=6
```

Application connects to any node; Redis handles sharding automatically.

### Troubleshooting Production Issues

#### Symptom: Slow Response Times

1. Check health endpoint:
   ```bash
   curl https://prod.example.com/health
   ```

2. Inspect OpenTelemetry traces for slow queries/services

3. Review cache hit rates (low hit rate = increasing database load)

4. Check Redis and MongoDB resource usage:
   ```bash
   docker stats
   ```

#### Symptom: High Memory Usage

1. Check Redis memory:
   ```bash
   redis-cli INFO memory
   ```

2. If near limit, keys are being evicted; consider increasing memory or adjusting TTL

3. Check MongoDB memory:
   ```bash
   mongosh --eval "db.stats()"
   ```

#### Symptom: Frequent Health Check Failures

1. Review health check timeout thresholds (may be too strict)

2. Check network latency between containers

3. Increase timeout values if infrastructure is slow:
   ```csharp
   private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(5);  // Increased from 3
   ```

### Security Considerations

#### Network Isolation

Ensure MongoDB and Redis are not exposed to the internet:

```yaml
services:
  mongodb:
    ports:
      - "127.0.0.1:27017:27017"  # Localhost only

  redis:
    ports:
      - "127.0.0.1:6379:6379"    # Localhost only
```

#### Authentication

Enable authentication for both services:

**MongoDB**:

```yaml
environment:
  - MONGO_INITDB_ROOT_USERNAME=admin
  - MONGO_INITDB_ROOT_PASSWORD=${MONGO_PASSWORD}
```

**Redis**:

```yaml
command: redis-server --requirepass ${REDIS_PASSWORD}
```

#### Encryption

Enable TLS for production connections:

**MongoDB TLS**:

```csharp
var settings = MongoClientSettings.FromConnectionString(
  "mongodb+srv://admin:pass@mongodb.example.com/?ssl=true"
);
var client = new MongoClient(settings);
```

**Redis TLS**:

```csharp
var options = ConfigurationOptions.Parse(
  "redis-prod.example.com:6380,ssl=true,sslProtocols=Tls12"
);
var connection = await ConnectionMultiplexer.ConnectAsync(options);
```

### Pre-Deployment Checklist

- [ ] Redis persistence enabled (RDB or AOF)
- [ ] MongoDB backups configured and tested
- [ ] Health checks configured for startup + ongoing operation
- [ ] OpenTelemetry metrics exporter configured
- [ ] Cache TTLs optimized for production load
- [ ] MongoDB indexes created for query performance
- [ ] Network security in place (no internet exposure)
- [ ] Authentication enabled for MongoDB and Redis
- [ ] TLS/HTTPS enabled for all external communication
- [ ] Monitoring and alerting configured
- [ ] Disaster recovery procedures documented
- [ ] Load balancer configured (if scaling horizontally)

### Post-Deployment Monitoring

After deployment:

1. Monitor health check endpoint every 5 minutes
2. Track cache hit/miss rates daily
3. Review performance metrics weekly
4. Test backup/recovery procedures monthly
5. Rotate credentials quarterly
