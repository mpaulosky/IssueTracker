# Configuration Guide

This guide describes how to configure Issue Tracker for different environments.

## Configuration Files

### Development: `appsettings.Development.json`

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
  },
  "AllowedHosts": "*"
}
```

### Production: `appsettings.Production.json`

```json
{
  "ConnectionStrings": {
    "MongoDB": "mongodb://mongo-production:27017",
    "DatabaseName": "IssueTrackerProd"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Error"
    }
  },
  "AllowedHosts": "yourdomain.com"
}
```

## Environment Variables

You can override settings using environment variables:

### MongoDB Connection

```bash
# Linux/Mac
export ConnectionStrings__MongoDB="mongodb://localhost:27017"
export ConnectionStrings__DatabaseName="IssueTrackerDb"

# Windows (PowerShell)
$env:ConnectionStrings__MongoDB="mongodb://localhost:27017"
$env:ConnectionStrings__DatabaseName="IssueTrackerDb"

# Windows (CMD)
set ConnectionStrings__MongoDB=mongodb://localhost:27017
set ConnectionStrings__DatabaseName=IssueTrackerDb
```

### Using Docker Compose

Environment variables can be set in `docker-compose.yml`:

```yaml
version: '3.8'
services:
  app:
    environment:
      - ConnectionStrings__MongoDB=mongodb://mongo:27017
      - ConnectionStrings__DatabaseName=IssueTrackerDb
      - ASPNETCORE_ENVIRONMENT=Production
  
  mongo:
    image: mongo:latest
    ports:
      - "27017:27017"
    volumes:
      - mongo-data:/data/db

volumes:
  mongo-data:
```

## Configuration Options

### Database Settings

| Setting | Description | Default | Required |
|---------|-------------|---------|----------|
| `ConnectionStrings:MongoDB` | MongoDB connection string | `mongodb://localhost:27017` | Yes |
| `ConnectionStrings:DatabaseName` | Database name | `IssueTrackerDb` | Yes |

### Logging Settings

| Setting | Description | Values |
|---------|-------------|--------|
| `Logging:LogLevel:Default` | Default log level | `Trace`, `Debug`, `Information`, `Warning`, `Error`, `Critical` |
| `Logging:LogLevel:Microsoft.AspNetCore` | ASP.NET Core log level | Same as above |

### Application Settings

| Setting | Description | Default |
|---------|-------------|---------|
| `AllowedHosts` | Allowed host headers | `*` (all) |
| `ASPNETCORE_ENVIRONMENT` | Runtime environment | `Development` |
| `ASPNETCORE_URLS` | URLs to listen on | `https://localhost:5001;http://localhost:5000` |

## User Secrets (Development)

For sensitive data in development, use .NET User Secrets:

### Initialize User Secrets

```bash
cd src/IssueTracker.UI
dotnet user-secrets init
```

### Set Secrets

```bash
dotnet user-secrets set "ConnectionStrings:MongoDB" "mongodb://localhost:27017"
dotnet user-secrets set "ConnectionStrings:DatabaseName" "IssueTrackerDb"
```

### List Secrets

```bash
dotnet user-secrets list
```

## Azure Configuration

### Using Azure App Configuration

1. Create an Azure App Configuration resource
2. Add connection string to environment:

```bash
export ConnectionStrings__AppConfig="Endpoint=https://your-config.azconfig.io;..."
```

3. Update `Program.cs`:

```csharp
builder.Configuration.AddAzureAppConfiguration(options =>
{
    options.Connect(builder.Configuration["ConnectionStrings:AppConfig"])
           .ConfigureRefresh(refresh =>
           {
               refresh.Register("Settings:Sentinel", refreshAll: true);
           });
});
```

### Using Azure Key Vault

1. Create an Azure Key Vault
2. Update `Program.cs`:

```csharp
var keyVaultEndpoint = new Uri(builder.Configuration["KeyVaultEndpoint"]);
builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());
```

## MongoDB Connection Strings

### Local Development

```
mongodb://localhost:27017
```

### Docker Container

```
mongodb://mongo:27017
```

### MongoDB Atlas (Cloud)

```
mongodb+srv://username:password@cluster.mongodb.net/database?retryWrites=true&w=majority
```

### Replica Set

```
mongodb://host1:27017,host2:27017,host3:27017/?replicaSet=rs0
```

### With Authentication

```
mongodb://username:password@localhost:27017/?authSource=admin
```

## Configuration Best Practices

### 1. Never Commit Secrets

- Use User Secrets for development
- Use environment variables for production
- Add `appsettings.*.json` to `.gitignore` if it contains secrets

### 2. Use Different Databases Per Environment

```json
{
  "ConnectionStrings": {
    "DatabaseName": "IssueTracker_Development"  // Dev
    // "IssueTracker_Staging"                   // Staging
    // "IssueTracker_Production"                // Production
  }
}
```

### 3. Validate Configuration on Startup

Add to `Program.cs`:

```csharp
var mongoConnection = builder.Configuration["ConnectionStrings:MongoDB"];
if (string.IsNullOrEmpty(mongoConnection))
{
    throw new InvalidOperationException("MongoDB connection string is not configured");
}
```

### 4. Use Dependency Injection

Register configuration objects:

```csharp
builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("ConnectionStrings"));
```

Access in services:

```csharp
public class MyService
{
    private readonly DatabaseSettings _settings;
    
    public MyService(IOptions<DatabaseSettings> settings)
    {
        _settings = settings.Value;
    }
}
```

## Troubleshooting

### Can't Connect to MongoDB

1. Check connection string format
2. Verify MongoDB is running: `docker ps` or `mongod --version`
3. Check network connectivity
4. Verify authentication credentials

### Configuration Not Loading

1. Check file name matches environment: `appsettings.{Environment}.json`
2. Verify `ASPNETCORE_ENVIRONMENT` is set correctly
3. Check file is set to "Copy if newer" in project properties
4. Ensure JSON is valid (use a JSON validator)

## Related Documentation

- [Getting Started](getting-started.md)
- [Deployment Guide](deployment.md)
- [Environment Variables](environment-variables.md)
