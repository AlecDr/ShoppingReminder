# Shopping Reminder - Setup Guide

This guide will help you set up the Shopping Reminder API for local development.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [PostgreSQL 15+](https://www.postgresql.org/download/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/)

## Getting Started

### 1. Clone the Repository
```bash
git clone https://github.com/yourusername/shopping-reminder.git
cd shopping-reminder
```

### 2. Install PostgreSQL

**Option A: Local Installation**
- Download and install PostgreSQL from https://www.postgresql.org/download/
- Remember your postgres user password

**Option B: Docker**
```bash
docker run --name postgres-shopping \
  -e POSTGRES_PASSWORD=yourpassword \
  -e POSTGRES_DB=ShoppingReminderDb \
  -p 5432:5432 \
  -d postgres:15
```

### 3. Configure Database Connection

Set your connection string using **User Secrets** (never commit passwords to Git):
```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=ShoppingReminderDb;Username=postgres;Password=YOUR_PASSWORD" --project ShoppingReminder.API/ShoppingReminder.API.csproj
```

**Important:** Replace `YOUR_PASSWORD` with your actual PostgreSQL password.

To verify your secrets:
```bash
dotnet user-secrets list --project ShoppingReminder.API/ShoppingReminder.API.csproj
```

### 4. Configure JWT Settings

Set JWT configuration using User Secrets:
```bash
# Generate a strong secret (minimum 32 characters)
dotnet user-secrets set "JwtSettings:Secret" "your-super-secret-jwt-key-with-at-least-32-characters-here" --project ShoppingReminder.API/ShoppingReminder.API.csproj

dotnet user-secrets set "JwtSettings:Issuer" "ShoppingReminderAPI" --project ShoppingReminder.API/ShoppingReminder.API.csproj

dotnet user-secrets set "JwtSettings:Audience" "ShoppingReminderApp" --project ShoppingReminder.API/ShoppingReminder.API.csproj

dotnet user-secrets set "JwtSettings:ExpirationInMinutes" "60" --project ShoppingReminder.API/ShoppingReminder.API.csproj

dotnet user-secrets set "JwtSettings:RefreshTokenExpirationInDays" "7" --project ShoppingReminder.API/ShoppingReminder.API.csproj
```

**Tip:** Generate a strong JWT secret using PowerShell:
```powershell
$bytes = New-Object byte[] 32
[Security.Cryptography.RandomNumberGenerator]::Create().GetBytes($bytes)
[Convert]::ToBase64String($bytes)
```

### 5. Restore Dependencies
```bash
dotnet restore
```

### 6. Build the Solution
```bash
dotnet build
```

### 7. Run Database Migrations

Create the database and tables:
```bash
dotnet ef database update --project ShoppingReminder.Infrastructure/ShoppingReminder.Infrastructure.csproj
```

To verify tables were created:
```bash
psql -U postgres -d ShoppingReminderDb -c "\dt"
```

You should see:
- Users
- Groups
- GroupMembers
- Invitations
- ShoppingLists
- ShoppingItems
- PurchaseHistories
- __EFMigrationsHistory

### 8. Run the Application
```bash
cd ShoppingReminder.API
dotnet run
```

The API will be available at:
- HTTPS: `https://localhost:7001`
- HTTP: `http://localhost:5001`
- Swagger UI: `https://localhost:7001/swagger`

## Development Workflow

### Creating a New Migration

After modifying entities:
```bash
dotnet ef migrations add YourMigrationName --project ShoppingReminder.Infrastructure/ShoppingReminder.Infrastructure.csproj
```

### Applying Migrations
```bash
dotnet ef database update --project ShoppingReminder.Infrastructure/ShoppingReminder.Infrastructure.csproj
```

### Reverting a Migration
```bash
# Revert to previous migration
dotnet ef database update PreviousMigrationName --project ShoppingReminder.Infrastructure/ShoppingReminder.Infrastructure.csproj

# Remove the last migration (only if not applied)
dotnet ef migrations remove --project ShoppingReminder.Infrastructure/ShoppingReminder.Infrastructure.csproj
```

### View Migration SQL
```bash
dotnet ef migrations script --project ShoppingReminder.Infrastructure/ShoppingReminder.Infrastructure.csproj
```

## Running Tests

### Unit Tests
```bash
dotnet test tests/ShoppingReminder.UnitTests
```

### Integration Tests
```bash
dotnet test tests/ShoppingReminder.IntegrationTests
```

### All Tests with Coverage
```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

## Production Deployment

### Environment Variables

In production, use environment variables instead of User Secrets:

**Linux/Mac:**
```bash
export ConnectionStrings__DefaultConnection="Host=prod-server;Database=ShoppingReminderDb;Username=appuser;Password=SecurePassword"
export JwtSettings__Secret="production-jwt-secret-key-32-chars-minimum"
export JwtSettings__Issuer="ShoppingReminderAPI"
export JwtSettings__Audience="ShoppingReminderApp"
```

**Windows:**
```powershell
$env:ConnectionStrings__DefaultConnection="Host=prod-server;Database=ShoppingReminderDb;Username=appuser;Password=SecurePassword"
$env:JwtSettings__Secret="production-jwt-secret-key-32-chars-minimum"
$env:JwtSettings__Issuer="ShoppingReminderAPI"
$env:JwtSettings__Audience="ShoppingReminderApp"
```

**Docker Compose:**
```yaml
version: '3.8'
services:
  api:
    image: shopping-reminder-api
    environment:
      - ConnectionStrings__DefaultConnection=Host=postgres;Database=ShoppingReminderDb;Username=appuser;Password=SecurePassword
      - JwtSettings__Secret=production-jwt-secret-key-32-chars-minimum
      - JwtSettings__Issuer=ShoppingReminderAPI
      - JwtSettings__Audience=ShoppingReminderApp
    ports:
      - "5000:8080"
```

**Azure App Service:**

In Azure Portal → Configuration → Application Settings:
- `ConnectionStrings__DefaultConnection`
- `JwtSettings__Secret`
- `JwtSettings__Issuer`
- `JwtSettings__Audience`

### Database Migration in Production

**Option 1: During deployment (automated)**
```bash
dotnet ef database update --project ShoppingReminder.Infrastructure/ShoppingReminder.Infrastructure.csproj
```

**Option 2: Generate SQL script (recommended for production)**
```bash
dotnet ef migrations script --project ShoppingReminder.Infrastructure/ShoppingReminder.Infrastructure.csproj --output migration.sql
```
Then review and execute the SQL manually.

## Troubleshooting

### "Connection string not found"

Make sure you've set the connection string in User Secrets:
```bash
dotnet user-secrets list --project ShoppingReminder.API/ShoppingReminder.API.csproj
```

If empty, set it again following step 3.

### "Could not connect to the database"

Check that:
1. PostgreSQL is running: `psql -U postgres -c "SELECT version();"`
2. The connection string is correct (host, port, username, password)
3. The database user has permissions to create databases

### "Method not found" or version conflicts

Ensure all EntityFrameworkCore packages are version 10.0.1:
```bash
dotnet list package
```

Update if needed:
```bash
dotnet add package Microsoft.EntityFrameworkCore --version 10.0.1
```

### Migrations not applying

Make sure you're running commands from the solution root directory and specifying the correct project:
```bash
# From C:\path\to\ShoppingReminder\
dotnet ef database update --project ShoppingReminder.Infrastructure/ShoppingReminder.Infrastructure.csproj
```

## Useful Commands

### Clean and Rebuild
```bash
dotnet clean
dotnet restore
dotnet build
```

### View User Secrets
```bash
dotnet user-secrets list --project ShoppingReminder.API/ShoppingReminder.API.csproj
```

### Remove a Secret
```bash
dotnet user-secrets remove "SecretKey" --project ShoppingReminder.API/ShoppingReminder.API.csproj
```

### Clear All Secrets
```bash
dotnet user-secrets clear --project ShoppingReminder.API/ShoppingReminder.API.csproj
```

### Check .NET Version
```bash
dotnet --version
dotnet --list-sdks
```

### Database Commands (psql)
```bash
# Connect to database
psql -U postgres -d ShoppingReminderDb

# List tables
\dt

# Describe table
\d Users

# View data
SELECT * FROM "Users";

# Exit
\q
```

## Project Structure
```
ShoppingReminder/
├── src/
│   ├── ShoppingReminder.Domain/          # Business entities and logic
│   ├── ShoppingReminder.Application/     # Use cases (CQRS)
│   ├── ShoppingReminder.Infrastructure/  # Database, external services
│   └── ShoppingReminder.API/            # REST API controllers
├── tests/
│   ├── ShoppingReminder.UnitTests/
│   └── ShoppingReminder.IntegrationTests/
├── README.md
├── SETUP.md
└── ShoppingReminder.sln
```

## Getting Help

- Check the [README.md](README.md) for project overview
- Review API documentation at `/swagger` when running
- Check [issues](https://github.com/yourusername/shopping-reminder/issues) for known problems

## Security Notes

- ⚠️ **Never commit** `appsettings.Development.json` with passwords
- ⚠️ **Never commit** connection strings or JWT secrets
- ✅ Always use **User Secrets** for local development
- ✅ Always use **Environment Variables** or **Key Vault** for production
- ✅ Generate strong, random JWT secrets (minimum 32 characters)
- ✅ Use different secrets for development and production

## Next Steps

1. ✅ Set up database and migrations
2. ✅ Configure secrets
3. 🔄 Implement authentication (register/login)
4. 🔄 Create group management endpoints
5. 🔄 Build shopping list features
6. 🔄 Add tests
7. 🔄 Deploy to production

Happy coding! 🚀