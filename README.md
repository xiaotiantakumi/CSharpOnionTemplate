# C# Onion Architecture Template

A clean, modern .NET 8 template implementing Onion Architecture (Clean Architecture) with CQRS pattern using MediatR.

## Features

- **Onion Architecture**: Clean separation of concerns with Domain, Application, Infrastructure, and Web layers
- **CQRS Pattern**: Command Query Responsibility Segregation using MediatR
- **Entity Framework Core**: Data access with SQL Server support
- **Comprehensive Testing**: Unit tests, integration tests, and functional tests
- **Modern .NET 8**: Latest .NET features and performance improvements
- **Swagger/OpenAPI**: Built-in API documentation
- **Dependency Injection**: Proper DI container setup

## Project Structure

```
Template.sln
├── src/
│   ├── Template.Domain/             # Domain layer (Core business logic)
│   ├── Template.Application/        # Application layer (Use cases, CQRS)
│   ├── Template.Infrastructure/     # Infrastructure layer (Persistence, external services)
│   └── Template.Web/                # Presentation layer (API, DI composition root)
└── tests/
    ├── Template.Domain.UnitTests/
    ├── Template.Application.UnitTests/
    ├── Template.Infrastructure.IntegrationTests/
    └── Template.Web.FunctionalTests/
```

## Getting Started

### Prerequisites

- .NET 8 SDK
- SQLite (default) or SQL Server (LocalDB or full instance)
- Visual Studio 2022 or VS Code

### Installation

1. Install the template:
```bash
dotnet new install ./CSharpOnionTemplate
```

2. Create a new project:
```bash
dotnet new onion -n MyAwesomeProject
```

3. Navigate to the project directory:
```bash
cd MyAwesomeProject
```

4. Restore packages:
```bash
dotnet restore
```

5. Update the connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your connection string here"
  }
}
```

6. Run the application:
```bash
dotnet run --project src/MyAwesomeProject.Web
```

### Database Setup

1. Create and run migrations:
```bash
dotnet ef migrations add InitialCreate --project src/MyAwesomeProject.Infrastructure --startup-project src/MyAwesomeProject.Web
dotnet ef database update --project src/MyAwesomeProject.Infrastructure --startup-project src/MyAwesomeProject.Web
```

## Architecture Overview

### Domain Layer
- **Entities**: Core business objects
- **Value Objects**: Immutable objects representing concepts
- **Interfaces**: Repository and service contracts
- **Exceptions**: Domain-specific exceptions
- **Enums**: Domain enumerations

### Application Layer
- **Commands**: Write operations using CQRS
- **Queries**: Read operations using CQRS
- **Handlers**: Command and query handlers
- **DTOs**: Data transfer objects
- **Services**: Application services
- **Interfaces**: Application service contracts

### Infrastructure Layer
- **Data**: Entity Framework DbContext and configurations
- **Repositories**: Data access implementations
- **Services**: External service implementations

### Web Layer
- **Controllers**: API endpoints
- **Middleware**: Custom middleware
- **Filters**: Action filters
- **Extensions**: Service registration extensions

## Testing

The template includes comprehensive testing setup:

- **Unit Tests**: Domain and Application layer tests
- **Integration Tests**: Infrastructure layer tests with in-memory database
- **Functional Tests**: End-to-end API tests

Run all tests:
```bash
dotnet test
```

### Code Coverage

Generate code coverage reports:

```bash
# Run tests with coverage collection
./scripts/run_tests.sh

# Generate HTML coverage report
./scripts/generate_coverage_report.sh

# Run tests and generate coverage report in one command
./scripts/run_tests_with_coverage.sh
```

The coverage report will be generated in the `coverage-report/` directory. Open `coverage-report/index.html` in your browser to view the detailed coverage report.

## Health Endpoints

The application includes comprehensive health check endpoints for monitoring and observability:

### Available Endpoints

- **`GET /api/health`** - Comprehensive health check
  - Returns detailed status of all health checks
  - Includes database connectivity and application status
  - HTTP Status: 200 (Healthy/Degraded), 503 (Unhealthy)

- **`GET /api/health/live`** - Liveness probe
  - Simple check to verify the application is running
  - Always returns 200 OK when the application is alive
  - Used by container orchestrators for liveness checks

- **`GET /api/health/ready`** - Readiness probe
  - Checks if the application is ready to accept requests
  - Depends on database connectivity and other critical services
  - HTTP Status: 200 (Ready), 503 (Not Ready)

### Health Check Response Format

```json
{
  "status": "Healthy|Degraded|Unhealthy",
  "totalDuration": "00:00:00.4192601",
  "checks": [
    {
      "name": "database",
      "status": "Healthy|Degraded|Unhealthy",
      "duration": "00:00:00.2674663",
      "description": "Optional description",
      "exception": "Exception message if any"
    }
  ]
}
```

### Usage Examples

```bash
# Check overall health
curl http://localhost:5000/api/health

# Check if application is alive
curl http://localhost:5000/api/health/live

# Check if application is ready
curl http://localhost:5000/api/health/ready
```

## Dependencies

### Core Packages
- **MediatR**: CQRS implementation
- **FluentValidation**: Input validation
- **Entity Framework Core**: Data access (SQLite by default)
- **Swagger/OpenAPI**: API documentation
- **Health Checks**: Application health monitoring

### Testing Packages
- **xUnit**: Testing framework
- **FluentAssertions**: Assertion library
- **Moq**: Mocking framework
- **Microsoft.AspNetCore.Mvc.Testing**: Integration testing

## Database Configuration

### Default Setup (SQLite)
The template is configured to use SQLite by default, which works across all platforms (Windows, macOS, Linux).

**Connection String:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=TemplateDb_Dev.db"
  }
}
```

### Switching to SQL Server
If you prefer to use SQL Server, update the following files:

1. **Update Infrastructure project** (`src/Template.Infrastructure/Template.Infrastructure.csproj`):
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0" />
```

2. **Update Program.cs** (`src/Template.Web/Program.cs`):
```csharp
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
```

3. **Update connection string** (`src/Template.Web/appsettings.Development.json`):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TemplateDb_Dev;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### Database Migrations
The template includes Entity Framework Core migrations. To manage the database:

```bash
# Create a new migration
dotnet tool run dotnet-ef migrations add MigrationName --project src/Template.Infrastructure --startup-project src/Template.Web

# Update the database
dotnet tool run dotnet-ef database update --project src/Template.Infrastructure --startup-project src/Template.Web

# Remove the last migration
dotnet tool run dotnet-ef migrations remove --project src/Template.Infrastructure --startup-project src/Template.Web
```

## Customization

### Adding New Entities

1. Create entity in `Domain/Entities/`
2. Add DbSet to `ApplicationDbContext`
3. Create configuration in `Infrastructure/Data/Configurations/`
4. Add migration: `dotnet tool run dotnet-ef migrations add AddNewEntity --project src/Template.Infrastructure --startup-project src/Template.Web`

### Adding New Commands/Queries

1. Create command/query in `Application/Commands/` or `Application/Queries/`
2. Create handler in `Application/Handlers/`
3. Register in DI container (already configured)

### Adding New Services

1. Create interface in appropriate layer
2. Create implementation in Infrastructure
3. Register in `Program.cs`

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests for new functionality
5. Submit a pull request

## License

This template is provided as-is for educational and development purposes.

## Support

For issues and questions, please create an issue in the repository.
