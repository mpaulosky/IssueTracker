## Aspire Architecture & Orchestration

### Overview

Issue Tracker uses **.NET Aspire** for local development orchestration. Aspire is a .NET distributed
application framework that simplifies building cloud-native applications. It manages containers,
services, and their dependencies through code-first configuration.

### System Topology

The application consists of three main components orchestrated by Aspire:

```
┌─────────────────────────────────────────────┐
│         Aspire AppHost                      │
│  (Local Orchestration & Container Manager)  │
└────────────┬────────────────────────────────┘
             │
    ┌────────┴──────────┐
    │                   │
    ▼                   ▼
┌─────────┐         ┌────────┐
│ MongoDB │         │  Redis │
│ :27017  │         │ :6379  │
└─────────┘         └────────┘
    ▲                   ▲
    │                   │
    └───────┬───────────┘
            │
    ┌───────▼──────────┐
    │  Blazor UI       │
    │  :5000 / :5001   │
    └──────────────────┘
```

### Resource Configuration

#### MongoDB

- **Image**: `mongodb/mongodb-community-server:latest`
- **Port**: `27017` (standard MongoDB port)
- **Volume**: Managed by Aspire with data persistence
- **Health Check**: Enabled (ping timeout: 3 seconds)
- **Aspire Configuration**:

```csharp
var mongodb = builder.AddMongoDB("mongodb")
  .WithDataVolume()
  .WithHealthCheck("mongodb");
```

#### Redis

- **Image**: `redis:latest`
- **Port**: `6379` (standard Redis port)
- **Volume**: Managed by Aspire with data persistence
- **Health Check**: Enabled (ping timeout: 2 seconds)
- **Aspire Configuration**:

```csharp
var redis = builder.AddRedis("redis")
  .WithDataVolume()
  .WithHealthCheck("redis");
```

#### Blazor UI (IssueTracker.UI)

- **Port**: `5000` (HTTP) / `5001` (HTTPS)
- **References**: Both MongoDB and Redis (injected as connection strings)
- **Aspire Configuration**:

```csharp
var ui = builder
  .AddProject<Projects.IssueTracker_UI>("ui")
  .WithReference(mongodb)
  .WithReference(redis);
```

### Running AppHost Locally

#### Prerequisites

- **.NET 10 SDK** installed (check with `dotnet --version`)
- **Docker Desktop** running (required for container provisioning)
- **Port availability**: Ensure ports 5000, 5001, 6379, 27017, and 18888 are available

#### Start AppHost

```bash
dotnet run --project src/AppHost/AppHost.csproj
```

This command:

1. Starts the Aspire orchestrator
2. Provisions MongoDB container with development credentials
3. Provisions Redis container
4. Launches the Blazor UI
5. Applies health checks before marking services ready
6. Provides a dashboard at `http://localhost:18888`

Expected console output:

```
...
Building...
info: Aspire.Hosting[0]
      Running on: http://localhost:18888
...
```

#### Accessing the Application

- **Blazor UI**: `http://localhost:5000` or `https://localhost:5001`
- **Aspire Dashboard**: `http://localhost:18888`
- **MongoDB**: `mongodb://localhost:27017` (internal to Aspire)
- **Redis**: `redis://localhost:6379` (internal to Aspire)

### Aspire Dashboard

The dashboard provides real-time visibility into:

- **Services**: Running containers and their status (Healthy, Degraded, Unhealthy)
- **Logs**: Streaming logs from all services
- **Traces**: OpenTelemetry traces showing request flows
- **Metrics**: Performance and resource utilization

Access it at `http://localhost:18888` while AppHost is running.

### Troubleshooting AppHost Startup Failures

#### Issue: "Port Already in Use"

**Symptoms**: Error mentioning port 5000, 6379, 27017, or 18888

**Solution**:

```bash
# Find process using the port (Windows PowerShell)
Get-NetTCPConnection -LocalPort 6379 | Select-Object OwningProcess
tasklist /FI "PID eq <PID>"

# Or stop all Docker containers
docker stop $(docker ps -q)
```

#### Issue: "Docker Daemon Not Running"

**Symptoms**: Error: `Cannot connect to Docker daemon`

**Solution**:

- Ensure Docker Desktop is running
- Verify with: `docker ps`

#### Issue: "MongoDB/Redis Health Check Timeout"

**Symptoms**: Services stuck in "Degraded" state in dashboard

**Solution**:

```bash
# Check container logs
docker logs <container-name>

# Restart the container
docker restart <container-name>

# Or restart AppHost (Aspire will recreate containers)
```

#### Issue: "Cannot Resolve Service References"

**Symptoms**: Blazor UI cannot connect to MongoDB or Redis

**Solution**:

- Verify health checks pass in Aspire dashboard
- Check connection strings in logs
- Ensure ServiceDefaults is registered in UI project

```csharp
builder.AddServiceDefaults();
```

### Health Checks Integration

AppHost registers health checks for MongoDB and Redis:

```csharp
builder.Services.AddHealthChecks()
  .AddCheck<HealthChecks.MongoDbHealthCheck>("mongodb")
  .AddCheck<HealthChecks.RedisHealthCheck>("redis");
```

These checks run continuously. If a service fails its health check:

- Aspire marks it as "Unhealthy"
- The dashboard alerts developers
- Dependent services may fail to connect

See [Health-Checks.md](Health-Checks.md) for detailed health check behavior.

### Stopping AppHost

Press `Ctrl+C` in the terminal running AppHost. This gracefully shuts down:

1. All containers (MongoDB, Redis)
2. The Blazor UI
3. The Aspire orchestrator

Data persists in Docker volumes and is restored on next startup.

### Clearing Volumes for Fresh Start

To reset all data and start fresh:

```bash
# Stop all containers
docker stop $(docker ps -q)

# Remove containers
docker rm $(docker ps -aq)

# Remove named volumes (optional)
docker volume rm $(docker volume ls -q)

# Restart AppHost
dotnet run --project src/AppHost/AppHost.csproj
```

**Warning**: This removes all development data. Use only for testing/cleanup.
