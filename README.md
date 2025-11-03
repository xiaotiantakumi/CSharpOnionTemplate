# C# Onion Architecture Template

A clean, modern .NET 8 template implementing Onion Architecture (Clean Architecture) with CQRS pattern using MediatR.

## Available Templates

This repository provides two template variants, each in a separate branch:

### aspnetcore-webapi Branch

- **ASP.NET Core Web API** implementation
- Traditional web application with controllers
- Swagger/OpenAPI documentation
- Best for: Traditional web APIs, MVC applications

### azure-functions Branch (Current)

- **Azure Functions v4** with Isolated Worker Model
- Serverless HTTP trigger functions
- ASP.NET Core integration for better performance
- Best for: Serverless applications, event-driven architectures, Azure deployments

## Features

- **Onion Architecture**: Clean separation of concerns with Domain, Application, Infrastructure, and Functions layers
- **CQRS Pattern**: Command Query Responsibility Segregation using MediatR
- **Entity Framework Core**: Data access with SQLite (default) or SQL Server support
- **Comprehensive Testing**: Unit tests, integration tests, and functional tests
- **Modern .NET 8**: Latest .NET features and performance improvements
- **Azure Functions v4**: Isolated Worker Model with .NET 8 support
- **ASP.NET Core Integration**: Enhanced performance for HTTP triggers
- **Dependency Injection**: Proper DI container setup

## Project Structure

```
Template.sln
├── src/
│   ├── Template.Domain/             # Domain layer (Core business logic)
│   ├── Template.Application/        # Application layer (Use cases, CQRS)
│   ├── Template.Infrastructure/     # Infrastructure layer (Persistence, external services)
│   └── Template.Functions/          # Presentation layer (Azure Functions, DI composition root)
│       └── Functions/               # HTTP trigger functions
└── tests/
    ├── Template.Domain.UnitTests/
    ├── Template.Application.UnitTests/
    ├── Template.Infrastructure.IntegrationTests/
    └── Template.Functions.FunctionalTests/
```

## Getting Started

### Prerequisites

- .NET 8 SDK
- Azure Functions Core Tools v4 (for local development)
- SQLite (default) or SQL Server (LocalDB or full instance)
- Visual Studio 2022 or VS Code

### Installation

#### Option 1: Install from GitHub (Recommended)

**For Azure Functions version:**

```bash
# Install template from azure-functions branch
dotnet new install <repository-url> --branch azure-functions

# Create a new project
dotnet new onion-functions -n MyAwesomeProject
```

**For ASP.NET Core Web API version:**

```bash
# Install template from aspnetcore-webapi branch
dotnet new install <repository-url> --branch aspnetcore-webapi

# Create a new project
dotnet new onion -n MyAwesomeProject
```

#### Option 2: Install from Local Directory

1. Clone the repository and checkout the desired branch:

```bash
git clone <repository-url>
cd CSharpOnionTemplate
git checkout azure-functions  # or aspnetcore-webapi
```

2. Install the template:

```bash
dotnet new install .
```

3. Create a new project:

```bash
# For Azure Functions version
dotnet new onion-functions -n MyAwesomeProject

# For ASP.NET Core Web API version
dotnet new onion -n MyAwesomeProject
```

4. Navigate to the project directory:

```bash
cd MyAwesomeProject
```

5. Restore packages:

```bash
dotnet restore
```

6. Update the connection string in `local.settings.json` (Azure Functions) or `appsettings.json` (Web API):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=MyDb.db"
  }
}
```

7. Run the application:

**For Azure Functions:**

```bash
# Using Azure Functions Core Tools
func start --project src/MyAwesomeProject.Functions

# Or using dotnet run
dotnet run --project src/MyAwesomeProject.Functions
```

**For ASP.NET Core Web API:**

```bash
dotnet run --project src/MyAwesomeProject.Web
```

### Database Setup

1. Create and run migrations:

**For Azure Functions:**

```bash
dotnet ef migrations add InitialCreate --project src/MyAwesomeProject.Infrastructure --startup-project src/MyAwesomeProject.Functions
dotnet ef database update --project src/MyAwesomeProject.Infrastructure --startup-project src/MyAwesomeProject.Functions
```

**For ASP.NET Core Web API:**

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

### Functions Layer (Azure Functions)

- **Functions**: HTTP trigger functions with [Function] and [HttpTrigger] attributes
- **Middleware**: Azure Functions worker middleware for cross-cutting concerns
- **Dependency Injection**: Service registration in Program.cs
- **Health Checks**: Integrated health check endpoints

### Web Layer (ASP.NET Core Web API - aspnetcore-webapi branch)

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

**For Azure Functions:**
By default, Azure Functions runs on port 7071 locally. Adjust the port as needed.

```bash
# Check overall health
curl http://localhost:7071/api/health

# Check if application is alive
curl http://localhost:7071/api/health/live

# Check if application is ready
curl http://localhost:7071/api/health/ready
```

**For ASP.NET Core Web API:**

```bash
# Check overall health
curl http://localhost:5000/api/health

# Check if application is alive
curl http://localhost:5000/api/health/live

# Check if application is ready
curl http://localhost:5000/api/health/ready
```

## Differences Between Template Variants

| Feature               | Azure Functions Branch                  | ASP.NET Core Web API Branch        |
| --------------------- | --------------------------------------- | ---------------------------------- |
| **Runtime**           | Azure Functions v4 (Isolated Worker)    | ASP.NET Core 8                     |
| **Entry Point**       | HTTP trigger functions                  | Controllers                        |
| **Deployment**        | Serverless (Azure Functions)            | Traditional web hosting            |
| **Scaling**           | Automatic (consumption plan)            | Manual/auto-scaling                |
| **API Documentation** | Optional (OpenAPI extensions)           | Swagger/OpenAPI built-in           |
| **Best For**          | Event-driven, serverless workloads      | Traditional web APIs, MVC apps     |
| **Configuration**     | `host.json`, `local.settings.json`      | `appsettings.json`                 |
| **HTTP Model**        | Functions with ASP.NET Core integration | Controllers with full MVC features |

## Dependencies

### Core Packages

- **MediatR**: CQRS implementation
- **FluentValidation**: Input validation
- **Entity Framework Core**: Data access (SQLite by default)
- **Health Checks**: Application health monitoring
- **Microsoft.Azure.Functions.Worker**: Azure Functions isolated worker model (Functions branch only)
- **Microsoft.Azure.Functions.Worker.Extensions.Http.AspNetCore**: ASP.NET Core integration (Functions branch only)

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

2. **Update Program.cs**:

**For Azure Functions** (`src/Template.Functions/Program.cs`):

```csharp
var connectionString = context.Configuration.GetSection("ConnectionStrings:DefaultConnection").Value
    ?? "Data Source=TemplateDb_Dev.db";
services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
```

**For ASP.NET Core Web API** (`src/Template.Web/Program.cs`):

```csharp
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
```

3. **Update connection string**:

**For Azure Functions** (`src/Template.Functions/local.settings.json`):

```json
{
  "Values": {
    "ConnectionStrings:DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TemplateDb_Dev;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

**For ASP.NET Core Web API** (`src/Template.Web/appsettings.Development.json`):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TemplateDb_Dev;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### Database Migrations

The template includes Entity Framework Core migrations. To manage the database:

**For Azure Functions:**

```bash
# Create a new migration
dotnet tool run dotnet-ef migrations add MigrationName --project src/Template.Infrastructure --startup-project src/Template.Functions

# Update the database
dotnet tool run dotnet-ef database update --project src/Template.Infrastructure --startup-project src/Template.Functions

# Remove the last migration
dotnet tool run dotnet-ef migrations remove --project src/Template.Infrastructure --startup-project src/Template.Functions
```

**For ASP.NET Core Web API:**

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
4. Add migration:

**For Azure Functions:**
`dotnet tool run dotnet-ef migrations add AddNewEntity --project src/Template.Infrastructure --startup-project src/Template.Functions`

**For ASP.NET Core Web API:**
`dotnet tool run dotnet-ef migrations add AddNewEntity --project src/Template.Infrastructure --startup-project src/Template.Web`

### Adding New Commands/Queries

1. Create command/query in `Application/Commands/` or `Application/Queries/`
2. Create handler in `Application/Handlers/`
3. Register in DI container (already configured)

### Adding New Services

1. Create interface in appropriate layer
2. Create implementation in Infrastructure
3. Register in `Program.cs` (for both Functions and Web versions)

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
