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
template.sln
├── src/
│   ├── template.Domain/             # Domain layer (Core business logic)
│   ├── template.Application/        # Application layer (Use cases, CQRS)
│   ├── template.Infrastructure/     # Infrastructure layer (Persistence, external services)
│   └── template.Web/                # Presentation layer (API, DI composition root)
└── tests/
    ├── template.Domain.UnitTests/
    ├── template.Application.UnitTests/
    ├── template.Infrastructure.IntegrationTests/
    └── template.Web.FunctionalTests/
```

## Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server (LocalDB or full instance)
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

## Dependencies

### Core Packages
- **MediatR**: CQRS implementation
- **FluentValidation**: Input validation
- **Entity Framework Core**: Data access
- **Swagger/OpenAPI**: API documentation

### Testing Packages
- **xUnit**: Testing framework
- **FluentAssertions**: Assertion library
- **Moq**: Mocking framework
- **Microsoft.AspNetCore.Mvc.Testing**: Integration testing

## Customization

### Adding New Entities

1. Create entity in `Domain/Entities/`
2. Add DbSet to `ApplicationDbContext`
3. Create configuration in `Infrastructure/Data/Configurations/`
4. Add migration: `dotnet ef migrations add AddNewEntity`

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
