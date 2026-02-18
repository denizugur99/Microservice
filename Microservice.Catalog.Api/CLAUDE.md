# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a .NET 10.0 microservice (Catalog API) built with ASP.NET Core Minimal APIs, using MongoDB as the database and following vertical slice architecture.

## Commands

```bash
# Build the project
dotnet build

# Run the application
dotnet run

# Restore dependencies
dotnet restore

# Run with specific configuration
dotnet run --configuration Release
```

## Architecture

### Vertical Slice / Feature-Based Organization

The codebase is organized by features under `Features/` directory, not by technical layers. Each feature (e.g., Categories, Courses) contains all related code:

```
Features/
  ├── Categories/
  │   ├── Category.cs                    # Domain entity
  │   ├── CategoryMapping.cs             # AutoMapper profile
  │   ├── CategoryEndpointExt.cs         # Endpoint group registration
  │   ├── Create/
  │   │   ├── CreateCategoryCommand.cs           # MediatR command (IrequestByServiceResult<T>)
  │   │   ├── CreateCategoryCommandHandler.cs    # Command handler
  │   │   ├── CreateCategoryEndpoint.cs          # Minimal API endpoint
  │   │   ├── CreateCategoryCommandValidator.cs  # FluentValidation validator
  │   │   └── CreateCategoryResponse.cs          # Response DTO
  │   ├── GetAll/
  │   ├── GetById/
  │   └── Dtos/
```

### Key Patterns

**1. CQRS with MediatR**
- Commands/queries implement `IrequestByServiceResult<T>` or `IrequestByServiceResult`
- Handlers implement `IRequestHandler<TCommand, ServiceResult<TResponse>>`
- All handler results are wrapped in `ServiceResult<T>` for consistent error handling

**2. ServiceResult Pattern**
- Custom result type that wraps responses with HTTP status codes
- Success methods: `SuccesAsOkay()`, `SuccesAsCreated()`, `SuccesAsNoContent()`
- Error methods: `Error()`, `ErrorAsNotFound()`, `ErrorFromValidation()`
- Converted to IResult via `ToGenericResult()` extension method

**3. Endpoint Registration Pattern**
- Each feature has `*EndpointExt.cs` that creates route groups
- Individual endpoints are in separate files (e.g., `CreateCategoryEndpoint.cs`)
- Endpoints return `RouteGroupBuilder` for fluent chaining
- Pattern: `groupBuilder.MapPost("/", async (Command cmd, IMediator mediator) => (await mediator.Send(cmd)).ToGenericResult())`

**4. Validation**
- FluentValidation validators for each command
- Applied via `ValidationFilter<T>` endpoint filter
- Automatically returns 400 with ProblemDetails for validation errors

**5. Extension Method Convention**
- Service registration methods end with `Ext` (e.g., `AddDatabaseExt()`, `AddOptionsExt()`)
- Keeps Program.cs clean and groups related configuration

**6. API Versioning**
- URL segment versioning (e.g., `/api/v1/categories`)
- Versions configured via `AddVersionExt()` and `AddVersionSetExt()`
- Endpoints specify version with `.MapToApiVersion(1, 0)`

### Database (MongoDB with EF Core)

- Uses MongoDB.EntityFrameworkCore provider
- `AppDbContext` inherits from `DbContext`
- Factory method pattern: `AppDbContext.Create(IMongoDatabase)`
- Entity configurations in `Repositories/*EntityConfig.cs`
- MongoDB connection configured via `MongoOptions` in appsettings

### Shared Infrastructure (Microservices.Shared project)

**CommonServiceExt.AddCommonServiceExt()** registers:
- MediatR with assembly scanning
- FluentValidation with auto-validation
- AutoMapper with assembly scanning
- HttpContextAccessor

## Adding New Features

When adding a new feature/endpoint:

1. Create feature folder under `Features/`
2. Create domain entity in feature root
3. Create operation folder (e.g., `Create/`, `Update/`)
4. Add command class implementing `IrequestByServiceResult<TResponse>`
5. Add command handler implementing `IRequestHandler<TCommand, ServiceResult<TResponse>>`
6. Add validator class inheriting `AbstractValidator<TCommand>`
7. Add endpoint class with static method returning `RouteGroupBuilder`
8. Register endpoint in `*EndpointExt.cs` group builder
9. Wire up endpoint group in `Program.cs`

### Endpoint Template

```csharp
public static class CreateXEndpoint
{
    public static RouteGroupBuilder CreateXGroupItemEndpoint(this RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapPost("/",
            async (CreateXCommand command, IMediator mediator) =>
                (await mediator.Send(command)).ToGenericResult())
            .WithName("CreateX")
            .MapToApiVersion(1, 0)
            .Produces<XResponse>(StatusCodes.Status201Created)
            .AddEndpointFilter<ValidationFilter<CreateXCommand>>();

        return groupBuilder;
    }
}
```

## Configuration

MongoDB connection is configured in `appsettings.Development.json`:
```json
{
  "MongoOptions": {
    "DatabaseName": "CatalogDb",
    "ConnectionString": "mongodb://myuser:mypassword@localhost:27017"
  }
}
```

## API Documentation

- OpenAPI/Swagger available in Development environment
- Scalar UI at `/scalar/v1` endpoint (preferred over Swagger UI)
- Access after running the application

## Important Notes

- Entity IDs are generated using `NewId.NextSequentialGuid()` for MongoDB optimization
- All endpoints use async/await patterns
- Primary constructor syntax is used throughout (C# 12 feature)
- Validation happens at endpoint level via filters, not in handlers
