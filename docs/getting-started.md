# Getting Started with Issue Tracker

This guide will help you get the Issue Tracker application up and running on your local machine.

## Prerequisites

Before you begin, ensure you have the following installed:

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) or later
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (required for local development — manages MongoDB via Aspire)
- [Git](https://git-scm.com/downloads)
- A code editor (recommended: [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/))

## Installation

### 1. Clone the Repository

```bash
git clone https://github.com/mpaulosky/IssueTracker.git
cd IssueTracker
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Run the Application

The application uses **.NET Aspire** for local orchestration. Simply run the AppHost project, which manages MongoDB and launches the Blazor UI:

```bash
dotnet run --project src/AppHost/IssueTracker.AppHost/IssueTracker.AppHost.csproj
```

Or use the VS Code task:

```bash
# In VS Code, press Ctrl+Shift+P and select "Tasks: Run Task" > "watch"
```

This single command:

- Starts the Aspire orchestrator
- Provisions a MongoDB container (`devissuetracker` database) with default dev credentials
- Launches the Blazor UI on port 5000 (HTTP) / 5001 (HTTPS)
- Applies health checks before marking services ready
- Provides a dashboard at `http://localhost:15000` (Aspire console)

### 4. Access the Application

Open your browser and navigate to:

```
https://localhost:5001
```

or

```
http://localhost:5000
```

### 5. View Aspire Dashboard

While the app is running, access the .NET Aspire dashboard at:

```
http://localhost:15000
```

This dashboard shows:

- Real-time logs from all services
- Health status of Blazor UI and MongoDB
- Resource metrics (CPU, memory)
- Environment configuration

## Running Tests

### Run All Tests

```bash
dotnet test
```

### Run Specific Test Project

```bash
# Unit tests only
dotnet test tests/IssueTracker.CoreBusiness.Tests.Unit

# Integration tests (requires Docker)
dotnet test tests/IssueTracker.PlugIns.Tests.Integration
```

### Run Tests with Coverage

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

## Project Structure

```
IssueTracker/
├── src/
│   ├── CoreBusiness/          # Domain models and business logic
│   ├── PlugIns/               # Data access implementations
│   ├── Services/              # Application services
│   └── UI/                    # Blazor UI components
├── tests/
│   ├── *.Tests.Unit/          # Unit tests
│   └── *.Tests.Integration/   # Integration tests
└── docs/                      # Documentation
```

For a detailed breakdown, see [Project Structure](project-structure.md).

## Next Steps

- Review the [Architecture Overview](architecture.md)
- Learn about [Testing](testing.md)
- Read the [Contributing Guide](CONTRIBUTING.md)
- Explore the [API Reference](REFERENCES.md)

## Troubleshooting

### Aspire Startup Issues

If the Aspire orchestrator fails to start:

1. Ensure Docker Desktop is running: `docker ps`
2. Check available disk space (Aspire containers require ~500MB)
3. Review logs in the Aspire dashboard at `http://localhost:15000`
4. If MongoDB health check times out, increase the timeout in `src/AppHost/IssueTracker.AppHost/Program.cs`

### MongoDB Connection Issues

If MongoDB container fails to initialize:

1. Verify Docker is running and can create containers
2. Check if port 27017 is already in use: `netstat -an | findstr 27017`
3. View MongoDB logs: `docker logs <container-id>`
4. Delete the MongoDB container and let Aspire recreate it: `docker container rm issuetracker-mongodb`

### Build Errors

If you encounter build errors:

1. Ensure you have .NET 10 SDK installed: `dotnet --version`
2. Clean the solution: `dotnet clean`
3. Restore packages: `dotnet restore`
4. Rebuild: `dotnet build`

### Port Already in Use

If ports 5000/5001 are already in use:

1. Stop the conflicting application
2. Or modify the port configuration in `src/AppHost/IssueTracker.AppHost/Program.cs`
3. Update your browser URL to match the new port

### Slow Startup

AppHost orchestration typically takes 10-15 seconds on first startup while:

- Docker images are pulled/cached
- MongoDB container initializes
- Health checks pass
- Blazor runtime initializes

This is normal. Subsequent runs are faster once images are cached.

## Getting Help

- Check the [documentation index](index.md)
- Search [existing issues](https://github.com/mpaulosky/IssueTracker/issues)
- Create a [new issue](https://github.com/mpaulosky/IssueTracker/issues/new)
- Start a [discussion](https://github.com/mpaulosky/IssueTracker/discussions)
