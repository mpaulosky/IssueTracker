# Configuration Guide

This guide describes how to configure Issue Tracker for different environments.

## Configuration Files

### Development: `appsettings.Development.json`

```json
{
  "DetailedErrors": true,
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "MongoDbSettings": {
    "ConnectionStrings": "mongodb://localhost:27017/devissuetracker?authSource=admin",
    "DatabaseName": "devissuetracker"
  }
}
```

**Note:** The development settings file contains a placeholder connection string without embedded credentials. For MongoDB instances requiring authentication, configure credentials using User Secrets or environment variables (see below).

### Production: `appsettings.Production.json`

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Error"
    }
  },
  "MongoDbSettings": {
    "ConnectionStrings": "mongodb://mongo-production:27017/issuetrackerProd?authSource=admin",
    "DatabaseName": "issuetrackerProd"
  },
  "AllowedHosts": "yourdomain.com"
}
```

**Important:** Never commit production credentials to source control. Use environment variables or a secure configuration management service.

## Environment Variables

You can override settings using environment variables:

### MongoDB Connection

```bash
# Linux/Mac
export MongoDbSettings__ConnectionStrings="mongodb://username:password@localhost:27017/devissuetracker?authSource=admin"
export MongoDbSettings__DatabaseName="devissuetracker"

# Windows (PowerShell)
$env:MongoDbSettings__ConnectionStrings="mongodb://username:password@localhost:27017/devissuetracker?authSource=admin"
$env:MongoDbSettings__DatabaseName="devissuetracker"

# Windows (CMD)
set MongoDbSettings__ConnectionStrings=mongodb://username:password@localhost:27017/devissuetracker?authSource=admin
set MongoDbSettings__DatabaseName=devissuetracker
```

### Using Docker Compose

Environment variables can be set in `docker-compose.yml`:

```yaml
version: '3.8'
services:
  app:
    environment:
      - MongoDbSettings__ConnectionStrings=mongodb://mongo:27017/issuetracker?authSource=admin
      - MongoDbSettings__DatabaseName=issuetracker
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
| `MongoDbSettings:ConnectionStrings` | MongoDB connection string | `mongodb://localhost:27017/devissuetracker?authSource=admin` | Yes |
| `MongoDbSettings:DatabaseName` | Database name | `devissuetracker` | Yes |

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

For sensitive data in development (such as MongoDB credentials), use .NET User Secrets:

### Initialize User Secrets

```bash
cd src/UI/IssueTracker.UI
dotnet user-secrets init
```

### Set Secrets

```bash
# Set MongoDB connection string with credentials
dotnet user-secrets set "MongoDbSettings:ConnectionStrings" "mongodb://username:password@localhost:27017/devissuetracker?authSource=admin"

# Set database name
dotnet user-secrets set "MongoDbSettings:DatabaseName" "devissuetracker"
```

### List Secrets

```bash
dotnet user-secrets list
```

### Remove a Secret

```bash
dotnet user-secrets remove "MongoDbSettings:ConnectionStrings"
```

**Note:** User secrets are stored outside the project directory and are never committed to source control. This is the recommended approach for local development with authenticated MongoDB instances.

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

- **Always** use User Secrets for development credentials
- Use environment variables or secure vaults for production
- The `appsettings.Development.json` file should never contain credentials
- Add sensitive config files to `.gitignore` if they contain secrets

### 2. Use Different Databases Per Environment

```json
{
  "MongoDbSettings": {
    "DatabaseName": "issuetracker_development"  // Dev
    // "issuetracker_staging"                    // Staging
    // "issuetracker_production"                 // Production
  }
}
```

### 3. Validate Configuration on Startup

Add to `Program.cs`:

```csharp
var mongoConnection = builder.Configuration["MongoDbSettings:ConnectionStrings"];
if (string.IsNullOrEmpty(mongoConnection))
{
    throw new InvalidOperationException("MongoDB connection string is not configured");
}
```

### 4. Use Dependency Injection

Register configuration objects:

```csharp
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));
```

Access in services:

```csharp
public class MyService
{
    private readonly MongoDbSettings _settings;
    
    public MyService(IOptions<MongoDbSettings> settings)
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
