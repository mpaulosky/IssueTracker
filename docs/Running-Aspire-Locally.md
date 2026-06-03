## Running Aspire Locally - Quick Start

### Prerequisites

Before running Issue Tracker locally, ensure you have:

- **.NET 10 SDK** (check with `dotnet --version`, minimum: 10.0.100)
- **Docker Desktop** (required for container provisioning)
- **Git** (for cloning the repository)
- **At least 4 GB RAM** available for Docker containers
- **Open ports**: 5000, 5001, 6379, 27017, 18888

### Step 1: Clone the Repository

```bash
git clone https://github.com/mpaulosky/IssueTracker.git
cd IssueTracker
```

### Step 2: Verify Prerequisites

```bash
# Check .NET version
dotnet --version
# Should output: 10.0.x

# Check Docker is running
docker ps
# Should succeed without error
```

If Docker fails, start Docker Desktop and retry.

### Step 3: Restore Dependencies

```bash
dotnet restore
```

This downloads all NuGet packages specified in `Directory.Packages.props`.

**Expected output**:

```
Determining projects to restore...
Restore completed in 1.23 sec for E:\github\IssueTracker\IssueTracker.slnx
```

### Step 4: Start AppHost

```bash
dotnet run --project src/AppHost/AppHost.csproj
```

**Expected output**:

```
Aspire.Hosting[0]
      Running on: http://localhost:18888
```

This command starts:

1. **Aspire Orchestrator** - Manages services and containers
2. **MongoDB Container** - Document database (port 27017)
3. **Redis Container** - In-memory cache (port 6379)
4. **Blazor UI** - Web application (ports 5000/5001)

Do **not** close this terminal window while developing.

### Step 5: Verify Services Are Running

#### Option A: Using Aspire Dashboard

Open your browser and navigate to: `http://localhost:18888`

**Dashboard shows**:

- All running services (MongoDB, Redis, UI)
- Health status: `Healthy`, `Degraded`, or `Unhealthy`
- Log streams from each service
- OpenTelemetry traces
- Resource usage

**Expected services**:

- `mongodb` - Status: Healthy (or Degraded/Unhealthy if not ready)
- `redis` - Status: Healthy (or Degraded/Unhealthy if not ready)
- `ui` - Status: Healthy (UI service running)

#### Option B: Using Command Line

```bash
# Check containers are running
docker ps

# Should show:
# - issuetracker-mongodb or my-mongodb
# - issuetracker-redis or redis
# - issuetracker-ui or ui
```

#### Option C: Check Health Endpoint

```bash
# Using curl
curl http://localhost:5000/health

# Using PowerShell
Invoke-WebRequest http://localhost:5000/health | ConvertFrom-Json | ConvertTo-Json -Depth 5
```

**Expected response**:

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

### Step 6: Access the Application

#### Web Application

- **HTTP**: `http://localhost:5000`
- **HTTPS**: `https://localhost:5001`

Both URLs serve the Blazor UI. Accept any SSL warnings in your browser (development certificate).

#### Aspire Dashboard

- **URL**: `http://localhost:18888`
- **Features**: Real-time logs, traces, metrics for all services

### Services Reference

| Service | Port | URL | Purpose |
|---------|------|-----|---------|
| Blazor UI | 5000/5001 | `http://localhost:5000` | Issue Tracker web app |
| MongoDB | 27017 | `mongodb://localhost:27017` | Document database |
| Redis | 6379 | `redis://localhost:6379` | Distributed cache |
| Aspire Dashboard | 18888 | `http://localhost:18888` | Monitoring & diagnostics |

### Accessing Services During Development

#### MongoDB Connection

From within the Blazor application, MongoDB is accessed via the connection string configured in
AppHost:

```csharp
var mongodb = builder.AddMongoDB("mongodb");
```

The UI service automatically receives the connection string from Aspire:

```csharp
var ui = builder
  .AddProject<Projects.IssueTracker_UI>("ui")
  .WithReference(mongodb);
```

**Manual Connection** (for debugging):

```bash
# Using mongosh (MongoDB shell)
mongosh --host localhost --port 27017 -u course -p whatever

# Or from MongoDB Compass: mongodb://course:whatever@localhost:27017
```

#### Redis Connection

Redis is similarly injected into the UI service:

```csharp
var redis = builder.AddRedis("redis");
```

**Manual Connection** (for debugging):

```bash
# Using redis-cli
redis-cli -h localhost -p 6379

# Test connection
redis-cli -h localhost -p 6379 PING
# Should return: PONG
```

### Stopping Services

#### Graceful Shutdown

Press `Ctrl+C` in the terminal running AppHost:

```
^C
Hosting stopped
```

This cleanly shuts down:

1. Blazor UI
2. Redis container
3. MongoDB container
4. Aspire orchestrator

All data is persisted to Docker volumes and restored on next startup.

#### Force Shutdown

If Ctrl+C does not work:

```bash
# PowerShell: Find and stop the AppHost process
Get-Process -Name "dotnet" | Where-Object {$_.CommandLine -like "*AppHost*"} | Stop-Process -Force

# Or manually stop Docker containers
docker stop $(docker ps -q)
```

### Clearing Data for Fresh Start

To reset all development data and start clean:

#### Option 1: Just Clear Data Volumes

```bash
# Identify Docker volumes
docker volume ls | grep issuetracker

# Remove specific volumes
docker volume rm issuetracker-mongodb_data issuetracker-redis_data

# Restart AppHost (will recreate empty volumes)
dotnet run --project src/AppHost/AppHost.csproj
```

#### Option 2: Complete Docker Reset

```bash
# Stop all containers
docker stop $(docker ps -q)

# Remove all Issue Tracker containers
docker ps -a | grep issuetracker | awk '{print $1}' | xargs docker rm

# Remove all volumes
docker volume rm $(docker volume ls | grep issuetracker | awk '{print $2}')

# Restart AppHost
dotnet run --project src/AppHost/AppHost.csproj
```

**Warning**: This removes all development data. Use only for testing.

### Common Issues

#### Issue: "Aspire dashboard not accessible (Connection refused)"

**Symptoms**: Cannot reach `http://localhost:18888`

**Solutions**:

1. Check AppHost is still running (terminal should show active process)
2. Verify port 18888 is not blocked by firewall
3. Restart AppHost

#### Issue: "MongoDB connection timeout"

**Symptoms**: Health check shows MongoDB unhealthy

**Solutions**:

```bash
# Check MongoDB container logs
docker logs <mongodb-container-id> --tail 20

# Restart MongoDB
docker restart <mongodb-container-id>

# Or restart AppHost
dotnet run --project src/AppHost/AppHost.csproj
```

#### Issue: "Redis connection refused"

**Symptoms**: Cache operations fail, `/health` shows Redis unhealthy

**Solutions**:

```bash
# Verify Redis is running
docker ps | grep redis

# Check Redis logs
docker logs <redis-container-id>

# Test Redis manually
redis-cli -h localhost -p 6379 PING

# Restart Redis
docker restart <redis-container-id>
```

#### Issue: "Port 5000 already in use"

**Symptoms**: AppHost fails to start, error: `Address already in use`

**Solutions**:

```bash
# Find process using port 5000 (PowerShell)
Get-NetTCPConnection -LocalPort 5000 -ErrorAction SilentlyContinue | Select-Object OwningProcess
tasklist /FI "PID eq <PID>"

# Kill the process (replace PID)
Stop-Process -Id <PID> -Force

# Or find and stop on port 6379 or 27017 if those are conflicting
```

### Performance Tips

1. **Allocate sufficient Docker resources** (Settings → Resources: 4 GB RAM, 2 CPUs minimum)
2. **Use HTTPS for production testing** (`https://localhost:5001`)
3. **Monitor Aspire dashboard** for slow services
4. **Check health endpoint** if services feel unresponsive
5. **Clear volumes periodically** to prevent disk clutter

### Next Steps

After AppHost is running:

- Read [Cache-Strategy.md](Cache-Strategy.md) to understand caching
- Review [Health-Checks.md](Health-Checks.md) for monitoring
- Check [Aspire.md](Aspire.md) for architecture details
- See [Production-Readiness.md](Production-Readiness.md) for deployment guidance
