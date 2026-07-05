# Microservice E-Learning Platform

<!-- CI workflow test: verifying build + auto-merge pipeline -->
<!-- CD workflow test: verifying self-hosted docker-compose deploy on master push -->


A .NET 10.0 microservices solution for an e-learning platform with e-commerce capabilities. The solution uses a hybrid architecture: **vertical slice** for simple services and **clean architecture / DDD** for the Order service.

## Architecture Overview

```
┌──────────────────────────────────────────────────────────────┐
│                    MicroserviceWebApp (Razor Pages)          │
└───────────────────────────┬──────────────────────────────────┘
                            │
┌───────────────────────────▼──────────────────────────────────┐
│              Gateway (YARP Reverse Proxy :5000)               │
│           Keycloak JWT Authentication & Authorization         │
└──┬──────┬──────┬───────┬────────┬──────────┬─────────────────┘
   │      │      │       │        │          │
 Catalog Basket Discount File   Order    Payment
 :5100  :5223  :5196  :5237   :5174     :5169
   │      │      │              │
 MongoDB Redis MongoDB       SQL Server
```

## Services

| Service | Port | Database | Architecture | Auth Policy |
|---|---|---|---|---|
| Catalog.Api | 5100 | MongoDB | Vertical Slice | ClientCredential |
| Basket.Api | 5223 | Redis | Vertical Slice | Password |
| Discount.API | 5196 | MongoDB | Vertical Slice | Password |
| File.Api | 5237 | — | Vertical Slice | ClientCredential |
| Order.Api | 5174 | SQL Server | Clean Arch / DDD | Password |
| Payment.Api | 5169 | In-Memory | Vertical Slice | Password |
| Gateway | 5000 | — | YARP Proxy | — |

## Tech Stack

- **.NET 10.0** — all services
- **MediatR** — CQRS pattern
- **FluentValidation** — request validation
- **AutoMapper** — object mapping
- **Refit** — typed HTTP clients for inter-service communication
- **MassTransit + Kafka** — async event-driven messaging
- **Keycloak** — centralized authentication (OIDC / JWT)
- **YARP** — API gateway and reverse proxy
- **Entity Framework Core** — SQL Server (Order), MongoDB (Catalog, Discount)
- **StackExchange.Redis** — Basket caching
- **Scalar** — OpenAPI UI (at `/scalar/v1`)

## Infrastructure

Start all infrastructure services with Docker Compose:

```bash
docker-compose up -d
```

| Service | URL | Credentials |
|---|---|---|
| MongoDB — Catalog | localhost:27017 | myuser / mypassword |
| MongoDB — Catalog UI | localhost:8081 | myuser / mypassword |
| MongoDB — Discount | localhost:27018 | myuser / mypassword |
| MongoDB — Discount UI | localhost:8083 | myuser / mypassword |
| Redis — Basket | localhost:6379 | password: mypassword |
| Redis UI | localhost:8082 | myuser / mypassword |
| SQL Server — Order | localhost:1433 | sa / MyP@ssw0rd123 |
| PostgreSQL — Keycloak | localhost:5432 | keycloak_user / mypassword |
| Keycloak | localhost:8080 | admin / password |
| Kafka | localhost:9092 (internal) / 9094 (external) | — |
| Kafka UI | localhost:8084 | — |

**Keycloak realm:** `microserviceTenant` | **Audience:** `gateway.api`

## Getting Started

### Prerequisites

- .NET 10.0 SDK
- Docker & Docker Compose

### 1. Start Infrastructure

```bash
docker-compose up -d
```

### 2. Build the Solution

```bash
dotnet restore
dotnet build
```

### 3. Run Services

Run each service in a separate terminal:

```bash
cd Microservice.Catalog.Api && dotnet run      # :5100
cd Microservice.Basket.Api && dotnet run       # :5223
cd Microservice.Discount.API && dotnet run     # :5196
cd Microservice.File.Api && dotnet run         # :5237
cd Microservice.Order.Api && dotnet run        # :5174
cd Microservice.Payment.Api && dotnet run      # :5169
cd Microservice.Gateway && dotnet run          # :5000
cd MicroserviceWebApp && dotnet run            # :5xxx
```

### 4. Access the API

All requests go through the Gateway at `http://localhost:5000`.

| Resource | Gateway Route |
|---|---|
| Courses | `GET /v1/course` |
| Categories | `GET /v1/categories` |
| Baskets | `GET /v1/baskets` |
| Discounts | `GET /v1/discount` |
| Orders | `GET /v1/orders` |
| Payments | `GET /v1/payments` |
| Files | `GET /v1/files` |

API documentation (Scalar UI) is available on each service at `/scalar/v1`.

## Project Structure

```
Microservice.sln
├── Microservice.Catalog.Api/         # Courses & categories (MongoDB)
├── Microservice.Basket.Api/          # Shopping basket (Redis)
├── Microservice.Discount.API/        # Discounts & coupons (MongoDB)
├── Microservice.File.Api/            # File uploads
├── Microservice.Order.Api/           # Orders — API layer
├── Microservice.Order.Application/   # Orders — use cases & interfaces
├── Microservice.Order.Domain/        # Orders — domain entities & logic
├── Microservice.Order.Persistence/   # Orders — EF Core / SQL Server
├── Microservice.Payment.Api/         # Payments
├── Microservice.Gateway/             # YARP reverse proxy
├── MicroserviceWebApp/               # Razor Pages frontend
├── Shared/
│   └── Microservices.Shared/         # ServiceResult, filters, extensions
├── Microservice.Bus/                 # MassTransit / Kafka config
└── Microservice.ServiceDefaults/     # Common service defaults
```

## Architecture Patterns

### Vertical Slice (Catalog, Basket, Discount, File, Payment)

Features are self-contained slices:

```
Features/
└── Course/
    ├── Course.cs                 # Domain model
    ├── CourseMapping.cs          # AutoMapper profile
    ├── CourseEndpointExt.cs      # Route group registration
    ├── Create/
    │   ├── CreateCourseCommand.cs
    │   ├── CreateCourseCommandHandler.cs
    │   ├── CreateCourseEndpoint.cs
    │   ├── CreateCourseValidator.cs
    │   └── CreateCourseResponse.cs
    └── ...
```

### Clean Architecture / DDD (Order)

```
Order.Domain/         # Entities, value objects, domain events
Order.Application/    # Use cases (commands/queries), contracts, background services
Order.Persistence/    # EF Core configurations, repositories, unit of work
Order.Api/            # Minimal API endpoints
```

### ServiceResult Pattern

All MediatR handlers return `ServiceResult<T>`:

```csharp
// In handlers
return ServiceResult<T>.SuccesAsOkay(data);
return ServiceResult<T>.SuccesAsCreated(data);
return ServiceResult.ErrorAsNotFound();
return ServiceResult.Error("message", HttpStatusCode.BadRequest);

// In endpoints
(await mediator.Send(command)).ToGenericResult()
```

### Kafka Events

| Event | Topic |
|---|---|
| `CoursePictureUploadedEvent` | `picture-uploaded-events` |
| `UploadCoursePictureCommand` | `order-events` |
| `OrderCreatedEvent` | `order-created-events` |

## Authentication

- **Password grant** — end-user authentication via Keycloak; used by Basket, Discount, Order, Payment
- **Client Credentials** — service-to-service auth; used by Catalog and File APIs
- Web app uses cookie authentication (60-day expiration) and passes JWT tokens to the Gateway

## Common Conventions

- Entity IDs: `NewId.NextSequentialGuid()` (sequential GUIDs for MongoDB optimization)
- Primary constructors (C# 12)
- All I/O is async/await
- Validation at endpoint level via `ValidationFilter<T>`, not inside handlers
- Configuration via Options pattern with data annotation validation
- Extension methods for service registration end with `Ext`
- API versioned via URL path: `/api/v1/...`
