# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

.NET 10.0 microservices solution for an e-learning platform with e-commerce capabilities. The solution follows a hybrid architecture: vertical slice for simple services (Catalog, Basket, Discount, File, Payment) and clean architecture/DDD for complex services (Order).

## Solution Structure

```
/src/
├── services/
│   ├── catalog/       - Catalog.Api (MongoDB, vertical slice)
│   ├── basket/        - Basket.Api (Redis, vertical slice)
│   ├── discount/      - Discount.API (MongoDB, vertical slice)
│   ├── file/          - File.Api (vertical slice)
│   ├── payment/       - Payment.Api (vertical slice)
│   ├── order/
│   │   ├── api/       - Order.Api
│   │   ├── core/      - Order.Application, Order.Domain (DDD)
│   │   └── infrastructure/ - Order.Persistence (SQL Server)
│   └── gateway/       - Gateway (YARP reverse proxy)
├── shared/
│   ├── Microservices.Shared  - Common patterns (ServiceResult, filters, extensions)
│   └── Microservice.Bus      - MassTransit/Kafka configuration
└── ui/
    └── MicroserviceWebApp    - ASP.NET Razor Pages frontend
```

## Running the Solution

### Start Infrastructure

```bash
# Start all infrastructure services (databases, Kafka, Keycloak)
docker-compose up -d

# View logs
docker-compose logs -f

# Stop all services
docker-compose down
```

### Run Individual Services

Each microservice runs on a specific port:

```bash
# Catalog API (port 5100)
cd Microservice.Catalog.Api
dotnet run

# Basket API (port 5223)
cd Microservice.Basket.Api
dotnet run

# Discount API (port 5196)
cd Microservice.Discount.API
dotnet run

# File API (port 5237)
cd Microservice.File.Api
dotnet run

# Order API (port 5174)
cd Microservice.Order.Api
dotnet run

# Payment API (port 5169)
cd Microservice.Payment.Api
dotnet run

# Gateway (port 5000)
cd Microservice.Gateway
dotnet run

# Web UI (port 5xxx - check launchSettings.json)
cd MicroserviceWebApp
dotnet run
```

### Build All Projects

```bash
# From solution root
dotnet build

# Restore dependencies
dotnet restore
```

## Infrastructure Services

### Databases

- **MongoDB (Catalog)**: localhost:27017 (UI: localhost:8081)
  - Database: CatalogDb
  - Credentials: myuser/mypassword

- **MongoDB (Discount)**: localhost:27018 (UI: localhost:8083)
  - Credentials: myuser/mypassword

- **Redis (Basket)**: localhost:6379 (UI: localhost:8082)
  - Password: mypassword
  - UI credentials: myuser/mypassword

- **SQL Server (Order)**: localhost:1433
  - SA password: MyP@ssw0rd123

- **PostgreSQL (Keycloak)**: localhost:5432
  - Database: keycloak_db
  - Credentials: keycloak_user/mypassword

### Message Broker

- **Kafka**: localhost:9092 (internal), localhost:9094 (external)
- **Kafka UI**: localhost:8084

### Authentication

- **Keycloak**: localhost:8080
  - Admin credentials: admin/password
  - Realm: microserviceTenant
  - Audience: gateway.api

## Architecture Patterns

### Vertical Slice Architecture (Catalog, Basket, Discount, File, Payment)

Services organized by features, not technical layers:

```
Features/
├── FeatureName/
│   ├── Entity.cs                    # Domain model
│   ├── EntityMapping.cs             # AutoMapper profile
│   ├── EntityEndpointExt.cs         # Route group registration
│   ├── Create/
│   │   ├── CreateCommand.cs         # IrequestByServiceResult<T>
│   │   ├── CreateCommandHandler.cs  # IRequestHandler<TCommand, ServiceResult<T>>
│   │   ├── CreateEndpoint.cs        # Minimal API endpoint
│   │   ├── CreateValidator.cs       # FluentValidation
│   │   └── CreateResponse.cs        # Response DTO
│   ├── Update/
│   ├── Delete/
│   └── GetAll/
```

**Key Patterns:**
- CQRS with MediatR (commands/queries implement `IrequestByServiceResult<T>`)
- ServiceResult wrapping for consistent error handling
- Endpoint filters for validation (`ValidationFilter<T>`)
- Extension methods for service registration (end with `Ext`)
- API versioning via URL segments (`/api/v1/...`)

**Example Endpoint:**
```csharp
public static RouteGroupBuilder CreateXEndpoint(this RouteGroupBuilder groupBuilder)
{
    groupBuilder.MapPost("/",
        async (CreateXCommand command, IMediator mediator) =>
            (await mediator.Send(command)).ToGenericResult())
        .WithName("CreateX")
        .MapToApiVersion(1, 0)
        .AddEndpointFilter<ValidationFilter<CreateXCommand>>();
    return groupBuilder;
}
```

### Clean Architecture / DDD (Order Service)

Organized in traditional layers:

```
Order.Domain/
├── Entitites/           # Domain entities with business logic

Order.Application/
├── Features/            # Use cases (commands/queries)
├── Contracts/           # Interfaces (repositories, Refit clients)
└── BackgroundServices/  # Hosted services

Order.Persistence/
├── Configurations/      # EF Core entity configs
├── Repositories/        # Repository implementations
├── UnitOfWork/          # Unit of Work pattern
└── AppDbContext.cs
```

**Key Patterns:**
- Generic repository pattern (`IGenericRepository<TEntity, TId>`)
- Unit of Work pattern for transactions
- Refit clients for inter-service communication
- Background services for async processing
- SQL Server with EF Core

## ServiceResult Pattern

All MediatR handlers return `ServiceResult<T>` or `ServiceResult`:

```csharp
// Success
ServiceResult<T>.SuccesAsOkay(data)
ServiceResult<T>.SuccesAsCreated(data)
ServiceResult.SuccesAsNoContent()

// Errors
ServiceResult.Error(message, status)
ServiceResult.ErrorAsNotFound()
ServiceResult.ErrorFromValidation(validationResult)
ServiceResult.ErrorFromProblemDetails(apiException)  // For Refit errors

// Convert to IResult for endpoints
(await mediator.Send(command)).ToGenericResult()
```

## Gateway (YARP)

Routes configured in `Microservice.Gateway/appsettings.Development.json`:

- Pattern: `/{version}/resource/{**catch-all}` → `/api/{version}/resource/{**catch-all}`
- Authorization policies: `Password` (user auth) or `ClientCredential` (service-to-service)
- All routes go through Keycloak authentication

**Route Examples:**
- `/v1/course/*` → Catalog API (ClientCredential)
- `/v1/baskets/*` → Basket API (Password)
- `/v1/orders/*` → Order API (Password)

## Shared Libraries

### Microservices.Shared

**CommonServiceExt.AddCommonServiceExt(assembly)** registers:
- MediatR with assembly scanning
- FluentValidation with auto-validation
- AutoMapper with assembly scanning
- HttpContextAccessor
- IIdentityService for user claims

**Other utilities:**
- `ServiceResult<T>` - Result wrapper pattern
- `ValidationFilter<T>` - Endpoint filter for FluentValidation
- `VersionExt` - API versioning setup
- `AuthExt` - JWT Bearer authentication with Keycloak
- `EndpointResultExt` - Convert ServiceResult to IResult

### Microservice.Bus

MassTransit configuration for Kafka messaging:

```csharp
services.AddMassTransitExt(configuration);
```

**Configured events/commands:**
- `CoursePictureUploadedEvent` → Picture-uploaded-events topic
- `UploadCoursePictureCommand` → Order-events topic
- `OrderCreatedEvent` → Order-created-events topic

## Authentication & Authorization

### User Authentication (Password grant)
Used by: Basket, Discount, Order, Payment APIs via Gateway

- Users sign in through MicroserviceWebApp
- Cookie-based auth for web app
- JWT tokens for API calls through Gateway
- Claims accessed via `IIdentityService`

### Service-to-Service (Client Credentials)
Used by: Catalog, File APIs via Gateway

- `ClientAuthenticatedHttpClientHandler` - Adds client credential tokens
- `AuthenticatedHttpClientHandler` - Adds user tokens (if available)
- Both handlers can be chained on Refit clients

**Example Refit setup:**
```csharp
services.AddRefitClient<IPaymentService>()
    .ConfigureHttpClient(cfg => cfg.BaseAddress = new Uri(paymentUrl))
    .AddHttpMessageHandler<AuthenticatedHttpClientHandler>()
    .AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();
```

## Web UI (MicroserviceWebApp)

ASP.NET Razor Pages application:

- Cookie authentication (60 day expiration)
- Refit clients for API communication via Gateway
- Delegating handlers for automatic token management
- Pages organized by feature (Auth/SignIn, Auth/SignUp, Instructor/*)

## API Documentation

All services expose OpenAPI/Swagger in development:

- Scalar UI at `/scalar/v1` (preferred over Swagger UI)
- Start the service and navigate to the endpoint

## Common Conventions

- **Entity IDs**: Use `NewId.NextSequentialGuid()` for MongoDB optimization
- **Primary constructors**: Used throughout (C# 12 feature)
- **Async/await**: All I/O operations are async
- **Validation**: At endpoint level via filters, not in handlers
- **Configuration**: Options pattern with data annotations validation
- **Error handling**: ServiceResult pattern ensures consistent error responses

## Catalog API Specifics

See `Microservice.Catalog.Api/CLAUDE.md` for detailed Catalog API documentation including:
- MongoDB with EF Core setup
- Feature-based organization examples
- Endpoint registration patterns
- Adding new features walkthrough
