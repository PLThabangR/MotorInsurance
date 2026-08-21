


# 🚗 Motor Insurance Microservices Platform

A production-oriented **Motor Insurance Microservices application** built with **ASP.NET Core 8** and designed around **Clean Architecture, Domain-Driven Design (DDD), CQRS, asynchronous messaging, and containerized infrastructure**.

The project simulates a modern insurance platform where customers can obtain quotes, purchase policies, and submit claims through independently deployable services.

The primary goal is to demonstrate how a complex business domain can be decomposed into maintainable, scalable, and independently deployable microservices.

---

## 🚀 Tech Stack

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-12-239120?style=flat&logo=csharp&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-512BD4?style=flat&logo=dotnet&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-15-4169E1?style=flat&logo=postgresql&logoColor=white)
![Redis](https://img.shields.io/badge/Redis-7-DC382D?style=flat&logo=redis&logoColor=white)
![RabbitMQ](https://img.shields.io/badge/RabbitMQ-Messaging-FF6600?style=flat&logo=rabbitmq&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-Containerized-2496ED?style=flat&logo=docker&logoColor=white)
![gRPC](https://img.shields.io/badge/gRPC-Communication-244c5a?style=flat)
![YARP](https://img.shields.io/badge/YARP-API%20Gateway-512BD4?style=flat)
![MediatR](https://img.shields.io/badge/MediatR-CQRS-blue?style=flat)

---

# 📌 Project Overview

The Motor Insurance Platform is designed as a collection of independent microservices responsible for different areas of the insurance domain.

The platform demonstrates workflows such as:

```text
Customer
   │
   ▼
Get Insurance Quote
   │
   ▼
Premium Calculation
   │
   ▼
Quote Created
   │
   ▼
Policy Issued
   │
   ▼
Customer submits Claim
   │
   ▼
Claim Processing
````

The application is intentionally divided into bounded contexts so that each service owns its own business logic and data.

---

# 🏗️ Microservices Architecture

The system is composed of independently deployable services.

```text
                         ┌─────────────────────┐
                         │      Client         │
                         └──────────┬──────────┘
                                    │
                                    ▼
                         ┌─────────────────────┐
                         │    API Gateway      │
                         │       YARP          │
                         └──────────┬──────────┘
                                    │
              ┌─────────────────────┼─────────────────────┐
              │                     │                     │
              ▼                     ▼                     ▼
      ┌───────────────┐     ┌───────────────┐     ┌───────────────┐
      │ Quote Service │     │ Policy Service│     │ Claim Service │
      │               │     │               │     │               │
      │ Quotes        │     │ Policies      │     │ Claims        │
      │ Premiums      │     │ Renewals      │     │ Processing     │
      └───────┬───────┘     └───────┬───────┘     └───────┬───────┘
              │                     │                     │
              ▼                     ▼                     ▼
        ┌───────────┐         ┌───────────┐         ┌───────────┐
        │ PostgreSQL│         │ PostgreSQL│         │ PostgreSQL│
        │ Quote DB  │         │ Policy DB │         │ Claim DB  │
        └───────────┘         └───────────┘         └───────────┘

                         ┌──────────────────┐
                         │     RabbitMQ     │
                         │                  │
                         │ Domain Events    │
                         │ Integration Msgs │
                         └──────────────────┘

                         ┌──────────────────┐
                         │      Redis       │
                         │                  │
                         │ Distributed      │
                         │ Cache            │
                         └──────────────────┘
```

---

# 🧩 Microservices

## 💰 Quote Service

Responsible for creating and managing insurance quotes.

Responsibilities include:

* Creating quotes
* Vehicle information
* Customer information
* Premium calculation
* Quote expiration
* Quote status
* Publishing quote events

Example domain concept:

```text
Quote
 ├── Customer
 ├── Vehicle
 ├── Premium
 └── Status
```

---

## 📄 Policy Service

Responsible for insurance policies.

Responsibilities include:

* Issuing policies
* Policy activation
* Policy cancellation
* Policy renewal
* Policy status
* Premium information

Example:

```text
Policy
 ├── Policy Number
 ├── Customer
 ├── Vehicle
 ├── Premium
 ├── Start Date
 ├── End Date
 └── Status
```

---

## 🚨 Claim Service

Responsible for managing insurance claims.

Responsibilities include:

* Creating claims
* Claim validation
* Claim status
* Claim processing
* Claim approval/rejection
* Claim events

Example:

```text
Claim
 ├── Claim Number
 ├── Policy
 ├── Incident
 ├── Description
 └── Status
```

---

# 🧠 Domain-Driven Design

The application applies **Domain-Driven Design (DDD)** concepts to model the insurance domain.

The objective is to ensure that business rules live within the domain rather than being scattered throughout controllers and infrastructure code.

---

## Aggregate Roots

Important business concepts are represented as Aggregate Roots.

```text
Quote
Policy
Claim
```

For example:

```text
Quote Aggregate
│
├── Customer Information
├── Vehicle Information
├── Premium
└── Quote Status
```

The Aggregate Root controls modifications to the aggregate and protects its business invariants.

---

# 💎 Value Objects

Value Objects are used for concepts that are defined by their values rather than identity.

Examples include:

```text
Premium
VehicleDetails
PolicyNumber
ClaimNumber
```

For example:

```csharp
public record Premium(decimal Amount);
```

This keeps domain concepts explicit instead of representing everything with primitive types.

---

# 📢 Domain Events

The system uses domain and integration events to communicate important business occurrences.

Examples:

```text
QuoteCreated
PolicyIssued
ClaimSubmitted
ClaimApproved
ClaimRejected
```

Example flow:

```text
Quote Created
     │
     ▼
QuoteCreated Event
     │
     ▼
RabbitMQ
     │
     ├──────────────► Policy Service
     │
     └──────────────► Other Subscribers
```

This reduces direct coupling between services.

---

# 🔄 CQRS

The application uses **Command Query Responsibility Segregation** to separate operations that modify state from operations that retrieve data.

### Commands

Examples:

```text
CreateQuoteCommand
IssuePolicyCommand
SubmitClaimCommand
ApproveClaimCommand
```

### Queries

Examples:

```text
GetQuoteByIdQuery
GetPolicyByIdQuery
GetClaimByIdQuery
```

MediatR is used to dispatch commands and queries to their respective handlers.

```text
HTTP Request
     │
     ▼
Controller
     │
     ▼
MediatR
     │
     ├── Command
     │      ↓
     │   Command Handler
     │
     └── Query
            ↓
        Query Handler
```

---

# 🏛️ Clean Architecture

Each microservice follows Clean Architecture principles.

A typical service is structured as:

```text
Quote Service
│
├── Quote.API
│
├── Quote.Application
│
├── Quote.Domain
│
└── Quote.Infrastructure
```

### Domain

Contains:

* Entities
* Value Objects
* Domain Events
* Business Rules
* Aggregates

The Domain layer has no dependency on Infrastructure.

---

### Application

Contains:

* Commands
* Queries
* Handlers
* DTOs
* Interfaces
* Validation
* Application business workflows

---

### Infrastructure

Contains:

* Entity Framework Core
* PostgreSQL
* Redis
* RabbitMQ
* External integrations
* Repository implementations

---

### API

Contains:

* Controllers
* HTTP endpoints
* Dependency Injection
* Middleware
* API configuration

---

# 🗄️ Database per Service

Each microservice owns its own database.

```text
Quote Service
     │
     ▼
QuoteDb


Policy Service
     │
     ▼
PolicyDb


Claim Service
     │
     ▼
ClaimDb
```

Services do not directly access another service's database.

Instead, communication occurs through APIs or messaging.

This helps maintain service autonomy and reduces coupling.

---

# 🐘 PostgreSQL

PostgreSQL is used as the primary relational database.

Entity Framework Core handles:

* Database access
* Migrations
* Entity configuration
* Relationships
* LINQ queries
* Transactions

Each microservice maintains ownership of its persistence model.

---

# ⚡ Redis

Redis is used as a distributed caching solution.

Potential cached data includes:

* Quote information
* Frequently requested policy information
* Reference data
* Temporary data

Example:

```text
Client
  │
  ▼
Quote Service
  │
  ├── Redis ─────► Cached Quote
  │
  └── PostgreSQL ─► Persistent Data
```

Caching helps reduce unnecessary database queries and improve response times.

---

# 📨 RabbitMQ

RabbitMQ provides asynchronous communication between microservices.

Instead of services directly depending on each other:

```text
Quote Service
      │
      │ HTTP call
      ▼
Policy Service
```

the application can use events:

```text
Quote Service
      │
      │ QuoteCreated
      ▼
   RabbitMQ
      │
      ▼
Policy Service
```

This allows services to communicate asynchronously and reduces temporal coupling.

---

# 🚚 MassTransit

MassTransit is used as the messaging abstraction for RabbitMQ.

It simplifies:

* Message publishing
* Message consumption
* Consumers
* Retry policies
* Messaging configuration
* Event-driven communication

Example:

```text
QuoteCreated
      │
      ▼
MassTransit
      │
      ▼
RabbitMQ
      │
      ▼
QuoteCreatedConsumer
```

---

# 🌐 API Gateway

The application uses **YARP (Yet Another Reverse Proxy)** as an API Gateway.

Clients communicate with the gateway rather than directly calling every service.

```text
                     Client
                       │
                       ▼
                ┌─────────────┐
                │ API Gateway │
                │    YARP     │
                └──────┬──────┘
                       │
          ┌────────────┼────────────┐
          ▼            ▼            ▼
       Quote        Policy        Claim
      Service       Service       Service
```

The gateway provides a single entry point into the microservices platform.

---

# 🔌 Service-to-Service Communication

The platform demonstrates different communication styles depending on the use case.

### Synchronous communication

Used when an immediate response is required.

```text
Service A
    │
    │ HTTP / gRPC
    ▼
Service B
    │
    ▼
Response
```

### Asynchronous communication

Used for events and background processing.

```text
Service A
    │
    ▼
RabbitMQ
    │
    ▼
Service B
```

This demonstrates the difference between **request/response communication** and **event-driven communication**.

---

# ⚙️ Dependency Injection

ASP.NET Core's built-in Dependency Injection container is used throughout the application.

Services are registered with appropriate lifetimes:

```csharp
services.AddScoped<IQuoteRepository, QuoteRepository>();
```

Dependencies are injected instead of being created directly by business logic.

---

# 🔐 Resilience & Reliability

The architecture is designed to support resilient distributed systems.

Areas explored include:

* Retry policies
* Timeout handling
* Message retries
* Failure handling
* Idempotent consumers
* Distributed caching
* Health checks

Microservices should assume that network calls and external dependencies can fail.

---

# 🐳 Docker

The complete application can be run using Docker Compose.

Infrastructure services include:

```text
┌──────────────────────────────────────────┐
│              Docker Compose              │
│                                          │
│  ┌─────────────┐    ┌─────────────┐     │
│  │ API Gateway │    │ Quote       │     │
│  │    YARP     │    │ Service     │     │
│  └─────────────┘    └──────┬──────┘     │
│                            │            │
│  ┌─────────────┐           │            │
│  │ Policy      │           │            │
│  │ Service     │           │            │
│  └─────────────┘           │            │
│                            │            │
│  ┌─────────────┐      ┌────▼──────┐     │
│  │ Claim       │      │ PostgreSQL│     │
│  │ Service     │      │           │     │
│  └─────────────┘      └───────────┘     │
│                                          │
│        ┌─────────┐      ┌─────────┐     │
│        │ RabbitMQ│      │  Redis  │     │
│        └─────────┘      └─────────┘     │
│                                          │
└──────────────────────────────────────────┘
```

Start the environment with:

```bash
docker compose up --build
```

---

# 📂 Project Structure

```text
MotorInsurance/
│
├── Gateway/
│   └── MotorInsurance.Gateway/
│
├── Services/
│
│   ├── Quote/
│   │   ├── MotorInsurance.Quote.API/
│   │   ├── MotorInsurance.Quote.Application/
│   │   ├── MotorInsurance.Quote.Domain/
│   │   └── MotorInsurance.Quote.Infrastructure/
│   │
│   ├── Policy/
│   │   ├── MotorInsurance.Policy.API/
│   │   ├── MotorInsurance.Policy.Application/
│   │   ├── MotorInsurance.Policy.Domain/
│   │   └── MotorInsurance.Policy.Infrastructure/
│   │
│   └── Claim/
│       ├── MotorInsurance.Claim.API/
│       ├── MotorInsurance.Claim.Application/
│       ├── MotorInsurance.Claim.Domain/
│       └── MotorInsurance.Claim.Infrastructure/
│
├── BuildingBlocks/
│   ├── Contracts/
│   ├── Messaging/
│   └── Shared/
│
├── docker-compose.yml
└── README.md
```

---

# 🔄 Example Business Flow

## Getting an Insurance Quote

```text
Client
  │
  ▼
API Gateway
  │
  ▼
Quote Service
  │
  ├── Validate Request
  │
  ├── Calculate Premium
  │
  ├── Create Quote
  │
  └── Save to PostgreSQL
  │
  ▼
QuoteCreated
  │
  ▼
RabbitMQ
```

---

## Issuing a Policy

```text
Quote
  │
  ▼
Quote Accepted
  │
  ▼
Policy Service
  │
  ├── Validate Quote
  ├── Create Policy
  ├── Generate Policy Number
  └── Save Policy
  │
  ▼
PolicyIssued
  │
  ▼
RabbitMQ
```

---

## Submitting a Claim

```text
Customer
   │
   ▼
API Gateway
   │
   ▼
Claim Service
   │
   ├── Validate Policy
   ├── Create Claim
   └── Save Claim
   │
   ▼
ClaimSubmitted
   │
   ▼
RabbitMQ
```

---

# 📋 Example API Endpoints

## Quotes

| Method | Endpoint           | Description    |
| ------ | ------------------ | -------------- |
| POST   | `/api/quotes`      | Create a quote |
| GET    | `/api/quotes/{id}` | Get quote      |
| GET    | `/api/quotes`      | Get quotes     |
| PUT    | `/api/quotes/{id}` | Update quote   |

## Policies

| Method | Endpoint             | Description   |
| ------ | -------------------- | ------------- |
| POST   | `/api/policies`      | Issue policy  |
| GET    | `/api/policies/{id}` | Get policy    |
| GET    | `/api/policies`      | Get policies  |
| PUT    | `/api/policies/{id}` | Update policy |

## Claims

| Method | Endpoint           | Description  |
| ------ | ------------------ | ------------ |
| POST   | `/api/claims`      | Submit claim |
| GET    | `/api/claims/{id}` | Get claim    |
| GET    | `/api/claims`      | Get claims   |
| PUT    | `/api/claims/{id}` | Update claim |

---

# 🧪 Testing Strategy

The project is designed to support testing at multiple levels.

### Unit Tests

Test:

* Domain logic
* Value objects
* Premium calculations
* Command handlers
* Query handlers

### Integration Tests

Test:

* Database interactions
* Messaging
* Redis
* Service communication

### End-to-End Tests

Test complete business workflows:

```text
Create Quote
     ↓
Accept Quote
     ↓
Issue Policy
     ↓
Submit Claim
     ↓
Process Claim
```

---

# 📚 Concepts Demonstrated

This project demonstrates practical experience with:

### Architecture

* ✅ Microservices Architecture
* ✅ Clean Architecture
* ✅ Domain-Driven Design
* ✅ Bounded Contexts
* ✅ Aggregates
* ✅ Value Objects
* ✅ Domain Events
* ✅ Integration Events

### ASP.NET Core

* ✅ ASP.NET Core 8
* ✅ REST APIs
* ✅ Dependency Injection
* ✅ Middleware
* ✅ Configuration
* ✅ Health Checks

### Application Architecture

* ✅ CQRS
* ✅ MediatR
* ✅ Command Handlers
* ✅ Query Handlers
* ✅ Pipeline Behaviors
* ✅ DTOs
* ✅ Validation

### Data

* ✅ Entity Framework Core
* ✅ PostgreSQL
* ✅ Database Migrations
* ✅ Repository Pattern
* ✅ Redis
* ✅ Distributed Caching

### Distributed Systems

* ✅ RabbitMQ
* ✅ MassTransit
* ✅ Event-driven Architecture
* ✅ Asynchronous Messaging
* ✅ gRPC
* ✅ Service-to-Service Communication
* ✅ API Gateway
* ✅ YARP

### DevOps

* ✅ Docker
* ✅ Docker Compose
* ✅ Containerized Services
* ✅ Service Networking
* ✅ Environment Configuration

---

# 🚧 Roadmap

The project is continuously evolving.

Planned improvements include:

* [ ] Complete Quote Service
* [ ] Complete Policy Service
* [ ] Complete Claim Service
* [ ] Implement API Gateway routing
* [ ] Implement RabbitMQ integration
* [ ] Add MassTransit consumers
* [ ] Implement gRPC communication
* [ ] Add Redis caching
* [ ] Add centralized exception handling
* [ ] Add structured logging
* [ ] Add Serilog
* [ ] Add OpenTelemetry
* [ ] Add distributed tracing
* [ ] Add health checks
* [ ] Add unit tests
* [ ] Add integration tests
* [ ] Add Testcontainers
* [ ] Add authentication & authorization
* [ ] Add JWT
* [ ] Add CI/CD
* [ ] Deploy to Azure
* [ ] Kubernetes deployment

---

# 🎯 Learning Objectives

This project was built to gain practical experience with designing and developing distributed .NET applications.

The primary objectives are:

1. Understand how to decompose a large business domain into bounded contexts.
2. Apply Clean Architecture to individual microservices.
3. Model business logic using Domain-Driven Design.
4. Implement CQRS with MediatR.
5. Implement asynchronous communication using RabbitMQ.
6. Understand synchronous vs asynchronous service communication.
7. Implement distributed caching with Redis.
8. Use PostgreSQL with Entity Framework Core.
9. Containerize microservices with Docker.
10. Understand API Gateway patterns.
11. Build resilient distributed systems.
12. Develop software that can evolve independently as the system grows.

---

# 💡 Architectural Philosophy

The goal of this project is not to create microservices simply for the sake of using microservices.

The architecture is designed around **business boundaries and independent responsibilities**.

```text
                Business Domain
                       │
        ┌──────────────┼──────────────┐
        ▼              ▼              ▼
      Quotes         Policies        Claims
        │              │              │
        ▼              ▼              ▼
    Quote DB       Policy DB       Claim DB
```

Each service owns its domain logic and data while communicating with other services through well-defined contracts.

This allows individual services to evolve independently while maintaining clear boundaries between business capabilities.

---

# 👨‍💻 Author

**Your Name**

Software Developer | C# | .NET | ASP.NET Core | Microservices | Docker

This project is part of my journey toward becoming a professional .NET backend developer, with a focus on scalable application architecture and distributed systems.

---

# ⭐ Project Status

🚧 **Active Development**

The architecture and implementation are continuously evolving as new concepts and production-grade practices are introduced.

If you find this project useful, consider giving it a ⭐.

```

### One important recommendation for your portfolio

For this project, I would **not claim that every technology listed is "implemented" if you haven't implemented it yet**.

For example, if you've designed the architecture for RabbitMQ, gRPC, Redis and YARP but haven't finished them, change the README wording from:

> "The application uses RabbitMQ..."

to:

> "The application is being developed to use RabbitMQ..."

and keep them under the **Roadmap**.

That's actually better for a portfolio because a recruiter can see that you're **building incrementally rather than putting technologies in a README just to make the stack look impressive**.

For your GitHub profile, a strong repository description would be:

> **Production-oriented .NET 8 motor insurance microservices platform demonstrating Clean Architecture, DDD, CQRS/MediatR, PostgreSQL, Redis, RabbitMQ/MassTransit, gRPC, YARP API Gateway and Docker.**
```
