# AppHost MongoDB Extensions

This directory contains extensions for the .NET Aspire AppHost that enhance MongoDB hosting capabilities with dashboard management commands.

## MongoDBHostingExtensions

The `MongoDBHostingExtensions` class provides a convenient extension method to register MongoDB with built-in dashboard commands for database management.

### Features

- **Simplified MongoDB Registration**: Register MongoDB with a single method call
- **Health Check Integration**: Automatically configures health checks for MongoDB
- **Data Volume Management**: Sets up persistent data volumes for MongoDB containers
- **Dashboard Commands**: Adds interactive commands to the Aspire dashboard:
  - **Clear All Data**: Drops all collections in the database (useful for testing)
  - **Drop Database**: Drops the entire database with confirmation prompt

### Usage

In your `Program.cs` file:

```csharp
using AppHost.Extensions;

var builder = DistributedApplication.CreateBuilder(args);

// Add MongoDB with management commands
var mongodb = builder.AddMongoDBWithManagement("mongodb");

// Or specify a custom database name
var mongodb = builder.AddMongoDBWithManagement("mongodb", "MyCustomDb");

// Reference from other services
var ui = builder
    .AddProject<Projects.MyApp_UI>("ui")
    .WithReference(mongodb);

var app = builder.Build();
await app.RunAsync();
```

### Dashboard Commands

#### Clear All Data
- **Icon**: Delete (filled)
- **Description**: Clears all collections in the database
- **Use Case**: Quickly reset the database during development/testing
- **Action**: Iterates through all collections and drops them

#### Drop Database
- **Icon**: DeleteForever (filled, highlighted)
- **Description**: Drops the entire database
- **Confirmation**: Prompts for confirmation before executing
- **Use Case**: Complete database reset when needed
- **Action**: Drops the entire database including all collections

### Parameters

**`AddMongoDBWithManagement(string name, string databaseName = "IssueTrackerDb")`**

- `name` (required): The name of the MongoDB resource in Aspire
- `databaseName` (optional): The default database name, defaults to "IssueTrackerDb"

### Exception Handling

Both commands handle:
- `MongoException`: MongoDB-specific errors (connection, authentication, etc.)
- `OperationCanceledException`: User cancellation or timeout

Unexpected exceptions will propagate to allow proper debugging.

### Testing

Unit tests for these extensions are located in `tests/AppHost.Tests/MongoDBHostingExtensionsTests.cs`.

Run tests with:
```bash
dotnet test tests/AppHost.Tests/AppHost.Tests.csproj
```

### Requirements

- .NET 10.0
- Aspire.Hosting.MongoDB 13.0+
- MongoDB.Driver 3.5.0+
- Docker (for MongoDB container)

### Security Considerations

⚠️ **Important**: These dashboard commands are intended for development and testing environments. In production:
- Ensure proper authentication and authorization
- Consider removing or disabling these commands
- Use role-based access control for database operations
- Implement audit logging for destructive operations

### Contributing

When modifying these extensions:
1. Ensure all tests pass
2. Update documentation
3. Follow the existing code style and patterns
4. Add tests for new functionality

### See Also

- [Aspire MongoDB Integration](https://aspire.dev/integrations/databases/mongodb/mongodb-host/)
- [Custom Resource Commands](https://aspire.dev/fundamentals/custom-resource-commands/)
- [MongoDB Driver Documentation](https://www.mongodb.com/docs/drivers/csharp/)
