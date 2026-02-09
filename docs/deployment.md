# Deployment Guide

This guide covers deploying Issue Tracker to various environments.

## Prerequisites

Before deploying, ensure you have:

- .NET 7 SDK or runtime
- MongoDB instance (or MongoDB Atlas)
- Server/hosting platform
- SSL certificate (for HTTPS)

## Deployment Options

### 1. Docker Deployment (Recommended)

The easiest way to deploy Issue Tracker is using Docker.

#### Build Docker Image

```bash
# From the repository root
docker build -t issuetracker:latest -f src/IssueTracker.UI/Dockerfile .
```

#### Run with Docker Compose

```bash
docker-compose up -d
```

This starts both the application and MongoDB.

#### Docker Compose Configuration

```yaml
version: '3.8'

services:
  web:
    build:
      context: .
      dockerfile: src/IssueTracker.UI/Dockerfile
    ports:
      - "8080:80"
      - "8443:443"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__MongoDB=mongodb://mongo:27017
      - ConnectionStrings__DatabaseName=IssueTrackerDb
    depends_on:
      - mongo
    restart: unless-stopped

  mongo:
    image: mongo:latest
    ports:
      - "27017:27017"
    volumes:
      - mongo-data:/data/db
    restart: unless-stopped

volumes:
  mongo-data:
```

### 2. Azure App Service

#### Using Azure CLI

```bash
# Login to Azure
az login

# Create resource group
az group create --name IssueTrackerRG --location eastus

# Create App Service plan
az appservice plan create \
  --name IssueTrackerPlan \
  --resource-group IssueTrackerRG \
  --sku B1 \
  --is-linux

# Create web app
az webapp create \
  --name issuetracker-app \
  --resource-group IssueTrackerRG \
  --plan IssueTrackerPlan \
  --runtime "DOTNET|9.0"

# Configure app settings
az webapp config appsettings set \
  --name issuetracker-app \
  --resource-group IssueTrackerRG \
  --settings ConnectionStrings__MongoDB="your-connection-string" \
              ConnectionStrings__DatabaseName="IssueTrackerDb"

# Deploy (from local folder)
az webapp deployment source config-zip \
  --name issuetracker-app \
  --resource-group IssueTrackerRG \
  --src publish.zip
```

#### Using Visual Studio

1. Right-click the `IssueTracker.UI` project
2. Select **Publish**
3. Choose **Azure**
4. Select **Azure App Service (Linux)**
5. Create new or select existing App Service
6. Click **Publish**

### 3. Linux Server (Ubuntu/Debian)

#### Install .NET Runtime

```bash
# Add Microsoft package repository
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

# Install .NET runtime
sudo apt-get update
sudo apt-get install -y aspnetcore-runtime-7.0
```

#### Deploy Application

```bash
# Publish the application
dotnet publish src/IssueTracker.UI/IssueTracker.UI.csproj \
  -c Release \
  -o /var/www/issuetracker

# Set permissions
sudo chown -R www-data:www-data /var/www/issuetracker
```

#### Configure Systemd Service

Create `/etc/systemd/system/issuetracker.service`:

```ini
[Unit]
Description=Issue Tracker Application
After=network.target

[Service]
WorkingDirectory=/var/www/issuetracker
ExecStart=/usr/bin/dotnet /var/www/issuetracker/IssueTracker.UI.dll
Restart=always
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=issuetracker
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=ConnectionStrings__MongoDB=mongodb://localhost:27017
Environment=ConnectionStrings__DatabaseName=IssueTrackerDb

[Install]
WantedBy=multi-user.target
```

#### Start Service

```bash
sudo systemctl enable issuetracker
sudo systemctl start issuetracker
sudo systemctl status issuetracker
```

#### Configure Nginx Reverse Proxy

Create `/etc/nginx/sites-available/issuetracker`:

```nginx
server {
    listen 80;
    server_name yourdomain.com;
    
    location / {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
```

Enable and restart Nginx:

```bash
sudo ln -s /etc/nginx/sites-available/issuetracker /etc/nginx/sites-enabled/
sudo nginx -t
sudo systemctl restart nginx
```

### 4. Windows Server (IIS)

#### Install .NET Hosting Bundle

Download and install the [.NET Hosting Bundle](https://dotnet.microsoft.com/download/dotnet/7.0).

#### Publish Application

```powershell
dotnet publish src/IssueTracker.UI/IssueTracker.UI.csproj `
  -c Release `
  -o C:\inetpub\wwwroot\issuetracker
```

#### Configure IIS

1. Open IIS Manager
2. Create new Application Pool:
   - Name: `IssueTrackerPool`
   - .NET CLR version: `No Managed Code`
3. Create new Website:
   - Name: `IssueTracker`
   - Application Pool: `IssueTrackerPool`
   - Physical path: `C:\inetpub\wwwroot\issuetracker`
   - Binding: Port 80 (or 443 with SSL)
4. Set environment variables in `web.config` or system

## Database Setup

### MongoDB Atlas (Cloud)

1. Create account at [MongoDB Atlas](https://www.mongodb.com/cloud/atlas)
2. Create a cluster
3. Create database user
4. Whitelist IP addresses
5. Get connection string
6. Update application configuration

### Self-Hosted MongoDB

#### Docker

```bash
docker run -d \
  --name mongodb \
  -p 27017:27017 \
  -v mongo-data:/data/db \
  --restart unless-stopped \
  mongo:latest
```

#### Linux Installation

```bash
# Import MongoDB public key
wget -qO - https://www.mongodb.org/static/pgp/server-6.0.asc | sudo apt-key add -

# Create list file
echo "deb [ arch=amd64,arm64 ] https://repo.mongodb.org/apt/ubuntu jammy/mongodb-org/6.0 multiverse" | \
  sudo tee /etc/apt/sources.list.d/mongodb-org-6.0.list

# Install MongoDB
sudo apt-get update
sudo apt-get install -y mongodb-org

# Start MongoDB
sudo systemctl start mongod
sudo systemctl enable mongod
```

## SSL/TLS Configuration

### Using Let's Encrypt (Linux)

```bash
# Install Certbot
sudo apt-get install certbot python3-certbot-nginx

# Obtain certificate
sudo certbot --nginx -d yourdomain.com

# Auto-renewal is configured automatically
```

### Using Azure App Service

Azure App Service provides free SSL certificates:

1. Go to App Service > TLS/SSL settings
2. Click **Private Key Certificates (.pfx)** > **Create App Service Managed Certificate**
3. Select your custom domain
4. Add binding in **Bindings** section

## Environment Variables

Set these in your deployment environment:

| Variable | Description | Example |
|----------|-------------|---------|
| `ASPNETCORE_ENVIRONMENT` | Runtime environment | `Production` |
| `ConnectionStrings__MongoDB` | MongoDB connection | `mongodb://localhost:27017` |
| `ConnectionStrings__DatabaseName` | Database name | `IssueTrackerDb` |
| `ASPNETCORE_URLS` | URLs to bind | `http://0.0.0.0:5000` |

## Health Checks

Add health check endpoint in `Program.cs`:

```csharp
builder.Services.AddHealthChecks()
    .AddMongoDb(
        mongodbConnectionString: builder.Configuration["ConnectionStrings:MongoDB"],
        name: "mongodb");

app.MapHealthChecks("/health");
```

Test the endpoint:

```bash
curl http://yourdomain.com/health
```

## Monitoring & Logging

### Application Insights (Azure)

```csharp
builder.Services.AddApplicationInsightsTelemetry();
```

### Serilog

```bash
dotnet add package Serilog.AspNetCore
```

Configure in `Program.cs`:

```csharp
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));
```

## Performance Optimization

### 1. Enable Response Compression

```csharp
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});
```

### 2. Enable Response Caching

```csharp
builder.Services.AddResponseCaching();
app.UseResponseCaching();
```

### 3. Configure MongoDB Connection Pool

```csharp
var settings = MongoClientSettings.FromConnectionString(connectionString);
settings.MaxConnectionPoolSize = 100;
settings.MinConnectionPoolSize = 10;
```

## Security Checklist

- [ ] Use HTTPS (SSL/TLS)
- [ ] Set strong MongoDB credentials
- [ ] Whitelist MongoDB IP addresses
- [ ] Use environment variables for secrets
- [ ] Enable CORS only for trusted origins
- [ ] Implement rate limiting
- [ ] Keep dependencies updated
- [ ] Enable security headers
- [ ] Regular backups
- [ ] Monitor logs for suspicious activity

## Backup Strategy

### MongoDB Backup

```bash
# Create backup
mongodump --uri="mongodb://localhost:27017" --out=/backups/$(date +%Y%m%d)

# Restore backup
mongorestore --uri="mongodb://localhost:27017" /backups/20240209
```

### Automated Backups (Cron)

```bash
# Add to crontab
0 2 * * * mongodump --uri="mongodb://localhost:27017" --out=/backups/$(date +\%Y\%m\%d)
```

## Scaling

### Horizontal Scaling

Deploy multiple instances behind a load balancer:

```
Load Balancer
    ├── Instance 1
    ├── Instance 2
    └── Instance 3
         └── Shared MongoDB
```

### Database Scaling

Use MongoDB replica sets for high availability:

```
Primary MongoDB ← Read/Write
    ├── Secondary 1 ← Read
    └── Secondary 2 ← Read
```

## Troubleshooting

### Application Won't Start

1. Check logs: `journalctl -u issuetracker -n 50`
2. Verify .NET runtime: `dotnet --list-runtimes`
3. Check permissions on application directory
4. Verify MongoDB connection

### High Memory Usage

1. Enable garbage collection logging
2. Use profiling tools
3. Check for memory leaks
4. Configure connection pool sizes

### Slow Performance

1. Enable response caching
2. Optimize database queries
3. Add database indexes
4. Use CDN for static files

## Related Documentation

- [Configuration](configuration.md)
- [Environment Variables](environment-variables.md)
- [Docker Setup](docker.md)
- [Architecture](architecture.md)
