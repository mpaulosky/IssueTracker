# Getting Started with Issue Tracker

This guide will help you get the Issue Tracker application up and running on your local machine.

## Prerequisites

Before you begin, ensure you have the following installed:

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or later
- [MongoDB](https://www.mongodb.com/try/download/community) (or use Docker)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (optional, for containerized MongoDB)
- [Git](https://git-scm.com/downloads)
- A code editor (recommended: [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/))

## Installation

### 1. Clone the Repository

```bash
git clone https://github.com/mpaulosky/IssueTracker.git
cd IssueTracker
```

### 2. Set Up MongoDB

#### Option A: Using Docker (Recommended)

```bash
docker-compose up -d
```

This will start a MongoDB instance on `localhost:27017`.

#### Option B: Local MongoDB Installation

1. Install MongoDB Community Edition
2. Start the MongoDB service
3. Ensure it's running on `localhost:27017`

### 3. Configure the Application

Create or update the `appsettings.Development.json` file in `src/IssueTracker.UI/`:

```json
{
  "ConnectionStrings": {
    "MongoDB": "mongodb://localhost:27017",
    "DatabaseName": "IssueTrackerDb"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### 4. Restore Dependencies

```bash
dotnet restore
```

### 5. Build the Solution

```bash
dotnet build
```

### 6. Run the Application

```bash
dotnet run --project src/IssueTracker.UI/IssueTracker.UI.csproj
```

Or use the VS Code task:

```bash
# In VS Code, press Ctrl+Shift+P and select "Tasks: Run Task" > "watch"
```

### 7. Access the Application

Open your browser and navigate to:

```
https://localhost:5001
```

or

```
http://localhost:5000
```

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

### MongoDB Connection Issues

If you can't connect to MongoDB:

1. Verify MongoDB is running: `docker ps` or check your local MongoDB service
2. Check the connection string in `appsettings.Development.json`
3. Ensure port 27017 is not blocked by firewall

### Build Errors

If you encounter build errors:

1. Ensure you have .NET 9 SDK installed: `dotnet --version`
2. Clean the solution: `dotnet clean`
3. Restore packages: `dotnet restore`
4. Rebuild: `dotnet build`

### Port Already in Use

If ports 5000/5001 are already in use:

1. Change the ports in `src/IssueTracker.UI/Properties/launchSettings.json`
2. Or stop the conflicting application

## Getting Help

- Check the [documentation index](index.md)
- Search [existing issues](https://github.com/mpaulosky/IssueTracker/issues)
- Create a [new issue](https://github.com/mpaulosky/IssueTracker/issues/new)
- Start a [discussion](https://github.com/mpaulosky/IssueTracker/discussions)
