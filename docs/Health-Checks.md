## Health Checks & Service Monitoring

### Overview

Health checks are automated probes that verify the availability and responsiveness of external
dependencies (MongoDB, Redis). Issue Tracker exposes two standardized health check endpoints that
return the aggregate health status and detailed per-service information.

### Health Check Endpoints

#### `/health` - Readiness Probe

Indicates whether the application is **ready to accept traffic**.

**Purpose**: Used by load balancers, orchestrators, and deployment tools to determine readiness

**HTTP Status Codes**:

- `200 OK` - All services healthy, application ready
- `503 Service Unavailable` - One or more services degraded or unhealthy

**Response Example (All Healthy)**:

```json
{
  "status": "Healthy",
  "checks": {
    "mongodb": {
      "status": "Healthy",
      "description": "MongoDB connection is responsive"
    },
    "redis": {
      "status": "Healthy",
      "description": "Redis connection is responsive"
    }
  }
}
```

**Response Example (MongoDB Degraded)**:

```json
{
  "status": "Degraded",
  "checks": {
    "mongodb": {
      "status": "Unhealthy",
      "description": "MongoDB connection timed out after 3s"
    },
    "redis": {
      "status": "Healthy",
      "description": "Redis connection is responsive"
    }
  }
}
```

#### `/health/live` - Liveness Probe

Indicates whether the application **process is alive** (not applicable to Issue Tracker currently,
but reserved for future implementation).

**Purpose**: Used to detect and restart dead processes

**HTTP Status Codes**:

- `200 OK` - Process is running
- `503 Service Unavailable` - Process is deadlocked or hung

### Interpreting Health Responses

#### Status Levels

| Status | Meaning | Action |
|--------|---------|--------|
| **Healthy** | Service responds within timeout, fully operational | No action needed |
| **Degraded** | Service responds but with issues (slow, partial failure) | Investigate logs, consider restart |
| **Unhealthy** | Service unresponsive, timed out, or failed | Restart service, check container logs |

#### Common Issues and Meanings

| Response | Cause | Solution |
|----------|-------|----------|
| `"MongoDB connection is responsive"` | MongoDB healthy | None |
| `"MongoDB connection timed out after 3s"` | MongoDB slow or offline | Check `docker logs`, restart container |
| `"Redis connection is responsive"` | Redis healthy | None |
| `"Redis connection timed out after 2s"` | Redis slow or offline | Check `docker logs`, restart container |
| `"MongoDB ping returned zero response time"` | Unexpected response | Restart MongoDB, check network |

### MongoDB Health Check

**Service**: `mongodb`

**Probe Mechanism**: Sends a `ping` command to MongoDB admin database

**Timeout**: 3 seconds

**Implementation Details**:

```csharp
public class MongoDbHealthCheck : IHealthCheck
{
  private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(3);

  public async Task<HealthCheckResult> CheckHealthAsync(...)
  {
    using var timeoutCts = new CancellationTokenSource(Timeout);
    
    var database = _client.GetDatabase("admin");
    var pingCommand = new BsonDocument("ping", 1);
    
    await database.RunCommandAsync<BsonDocument>(pingCommand, ...);
    
    return HealthCheckResult.Healthy("MongoDB connection is responsive");
  }
}
```

**Troubleshooting**:

```bash
# Check MongoDB container is running
docker ps | grep mongodb

# Inspect container logs
docker logs <mongodb-container-id>

# Test connection manually
mongosh --host localhost --port 27017 -u course -p whatever

# If failed, restart container
docker restart <mongodb-container-id>
```

### Redis Health Check

**Service**: `redis`

**Probe Mechanism**: Sends a `PING` command to Redis server

**Timeout**: 2 seconds

**Implementation Details**:

```csharp
public class RedisHealthCheck : IHealthCheck
{
  private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(2);

  public async Task<HealthCheckResult> CheckHealthAsync(...)
  {
    using var timeoutCts = new CancellationTokenSource(Timeout);
    
    var server = _connection.GetServer(_connection.GetEndPoints().First());
    var pong = await server.PingAsync(flags: CommandFlags.DemandMaster);
    
    if (pong != TimeSpan.Zero)
    {
      return HealthCheckResult.Healthy("Redis connection is responsive");
    }
    
    return HealthCheckResult.Unhealthy("Redis ping returned zero response time");
  }
}
```

**Troubleshooting**:

```bash
# Check Redis container is running
docker ps | grep redis

# Inspect container logs
docker logs <redis-container-id>

# Test connection manually
redis-cli -h localhost -p 6379 PING

# If failed, restart container
docker restart <redis-container-id>
```

### Troubleshooting Unhealthy Services

#### Scenario 1: MongoDB Timeout

**Symptoms**:

- Health check shows: `"MongoDB connection timed out after 3s"`
- Blazor UI cannot load issue data

**Steps**:

1. Check if container is running:
   ```bash
   docker ps | grep mongodb
   ```

2. View container logs:
   ```bash
   docker logs <mongodb-container-id> --tail 50
   ```

3. If container is not running, restart AppHost:
   ```bash
   # Stop AppHost (Ctrl+C)
   # Then restart
   dotnet run --project src/AppHost/AppHost.csproj
   ```

4. If container is running but slow, check resource constraints:
   ```bash
   docker stats <mongodb-container-id>
   ```

5. If memory/CPU usage is high, restart the container:
   ```bash
   docker restart <mongodb-container-id>
   ```

#### Scenario 2: Redis Unreachable

**Symptoms**:

- Health check shows: `"Redis connection timed out after 2s"`
- Cache operations fail, no caching available

**Steps**:

1. Verify Redis is running:
   ```bash
   redis-cli -h localhost -p 6379 PING
   ```

2. If command fails, check if container exists:
   ```bash
   docker ps -a | grep redis
   ```

3. Restart Redis container:
   ```bash
   docker restart <redis-container-id>
   ```

4. Or restart entire AppHost:
   ```bash
   dotnet run --project src/AppHost/AppHost.csproj
   ```

#### Scenario 3: Intermittent Unhealthy Status

**Symptoms**:

- Health check occasionally shows "Degraded"
- Application works but is slow

**Causes**:

- High database/network load
- Container resource constraints
- DNS resolution delays

**Steps**:

1. Monitor container resources:
   ```bash
   docker stats
   ```

2. Check network latency:
   ```bash
   ping localhost
   ```

3. Increase timeouts if acceptable for your use case (edit health check code)

4. Scale or optimize database queries

### Integration with Kubernetes/Container Orchestrators

Health check endpoints integrate with container orchestrators (Kubernetes, Docker Swarm, etc.)
for automated service recovery.

#### Kubernetes Probe Configuration

```yaml
apiVersion: v1
kind: Pod
metadata:
  name: issuetracker
spec:
  containers:
  - name: ui
    image: issuetracker-ui:latest
    
    # Readiness probe: is service ready for traffic?
    readinessProbe:
      httpGet:
        path: /health
        port: 5000
      initialDelaySeconds: 10
      periodSeconds: 10
      timeoutSeconds: 5
      failureThreshold: 3
    
    # Liveness probe: is process still alive?
    livenessProbe:
      httpGet:
        path: /health/live
        port: 5000
      initialDelaySeconds: 30
      periodSeconds: 10
      timeoutSeconds: 5
      failureThreshold: 3
```

**Behavior**:

- **initialDelaySeconds**: Wait 10 seconds after container starts before first probe
- **periodSeconds**: Check every 10 seconds
- **failureThreshold**: Restart container after 3 consecutive failures

#### Docker Compose Health Check

```yaml
services:
  ui:
    image: issuetracker-ui:latest
    healthcheck:
      test: ["CMD", "curl", "-f", "http://localhost:5000/health"]
      interval: 10s
      timeout: 5s
      retries: 3
      start_period: 30s
```

### Monitoring Health Metrics

#### Check Health Endpoint from CLI

```bash
# Using curl
curl http://localhost:5000/health | jq

# Using PowerShell
Invoke-WebRequest -Uri "http://localhost:5000/health" | ConvertFrom-Json | ConvertTo-Json -Depth 5
```

#### Parse Health Response

```csharp
public class HealthResponse
{
  public string Status { get; set; }
  public Dictionary<string, HealthCheckData> Checks { get; set; }
}

public class HealthCheckData
{
  public string Status { get; set; }
  public string Description { get; set; }
}
```

#### Metrics to Track

- **Health Check Latency**: Time to complete health check probe
- **Failure Rate**: Percentage of failed health checks
- **Recovery Time**: Time to transition from Unhealthy → Healthy

Use OpenTelemetry metrics collection (see Production-Readiness.md) to export these metrics to
monitoring systems.

### Health Check Best Practices

1. **Run health checks frequently** (every 10 seconds) to detect failures quickly
2. **Use appropriate timeouts** (MongoDB: 3s, Redis: 2s) to avoid cascading failures
3. **Alert on degraded status**, not just unhealthy
4. **Test health endpoints manually** during deployment
5. **Log health check results** for debugging
6. **Implement gradual restarts** (exponential backoff) to avoid thundering herd
