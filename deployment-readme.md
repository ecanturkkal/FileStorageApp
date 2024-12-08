# Deployment Guide

## Prerequisites
- .NET 8.0 SDK
- Azure Subscription
- GitHub Account
- SQL Server or Azure SQL Database

## Configuration Steps

### 1. GitHub Secrets Setup
Configure the following secrets in your GitHub repository:
- `AZURE_WEBAPP_PUBLISH_PROFILE`: Azure Web App publish profile
- `DATABASE_CONNECTION_STRING`: Database connection string
- `AZURE_TENANT_ID`: Azure Active Directory Tenant ID
- `AZURE_APPLICATION_ID`: Application registration ID

### 2. Local Development
1. Clone the repository
2. Install .NET 8.0 SDK
3. Restore dependencies:
   ```bash
   dotnet restore
   ```

### 3. Database Migration
Run database migrations:
```bash
dotnet ef database update
```

### 4. Azure Deployment
The GitHub Actions workflow will automatically:
- Build the application
- Run unit tests
- Migrate the database
- Deploy to Azure App Service

## Environment Configuration
Modify `appsettings.json` or use Azure App Service configuration to set:
- Connection strings
- Azure-specific settings
- Logging levels

## Troubleshooting
- Ensure all GitHub secrets are correctly set
- Verify database connection strings
- Check Azure App Service configuration
