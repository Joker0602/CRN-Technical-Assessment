# CRN Technical Assessment API

A clean architecture ASP.NET Core Web API with JWT authentication, Entity Framework Core, Docker support, and CI/CD via GitHub Actions.

---

## Table of Contents

- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [Running with Docker](#running-with-docker)
- [CI/CD Pipeline](#cicd-pipeline)
- [API Documentation](#api-documentation)
- [Authentication](#authentication)

---

## Tech Stack

- **Runtime**: .NET 8
- **Framework**: ASP.NET Core Web API
- **Database**: SQL Server (LocalDB for dev, SQL Server 2022 for prod)
- **ORM**: Entity Framework Core
- **Auth**: JWT Bearer Authentication
- **Validation**: FluentValidation
- **API Versioning**: Asp.Versioning
- **Documentation**: Swagger / OpenAPI
- **Containerization**: Docker + Docker Compose
- **CI/CD**: GitHub Actions

---

## Project Structure

```
Solution/
├── src/
│   ├── API/                        # ASP.NET Core Web API project
│   │   ├── Controllers/            # API controllers
│   │   ├── Filters/                # Action filters for cross-cutting concerns
│   │   ├── Middleware/             # Custom middleware (exception handling etc.)
│   │   ├── Extensions/             # Extension methods for DI and app configuration
│   │   ├── Program.cs              # Application entry point and configuration
│   │   ├── appsettings.json                # Base configuration (shared)
│   │   ├── appsettings.Development.json    # Development overrides
│   │   └── appsettings.Production.json     # Production overrides
│   ├── Application/                # Application logic layer
│   │   ├── DTOs/                   # Data Transfer Objects
│   │   ├── Interfaces/             # Service interfaces
│   │   ├── Mapping/                # AutoMapper profiles
│   │   ├── Services/               # Service implementations
│   │   └── Validators/             # FluentValidation rules
│   ├── Domain/                     # Core domain layer
│   │   ├── Entities/               # Domain models
│   │   ├── Enums/                  # Enumeration types
│   │   ├── Events/                 # Domain events
│   │   └── Exceptions/             # Custom domain exceptions
│   └── Infrastructure/             # Infrastructure layer
│       ├── Data/
│       │   ├── Configurations/     # EF Core entity configurations
│       │   ├── Repositories/       # Repository implementations
│       │   ├── ApplicationDbContext.cs
│       │   └── UnitOfWork.cs
│       ├── Identity/               # Authentication and authorization
│       ├── Logging/                # Logging infrastructure
│       └── Services/               # External service integrations
├── tests/
│   ├── API.Tests/                  # Integration tests
│   ├── Application.Tests/          # Unit tests for application layer
│   └── Infrastructure.Tests/       # Unit tests for infrastructure layer
├── docker-compose.yml              # Production Docker Compose
├── docker-compose.override.yml     # Development Docker Compose overrides
└── .github/
    └── workflows/
        └── deploy.yml              # GitHub Actions CI/CD pipeline
```

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or LocalDB
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (optional)

### Local Development (without Docker)

1. **Clone the repository**

```bash
git clone https://github.com/your-username/CRN.TechnicalAssessment.git
cd CRN.TechnicalAssessment
```

2. **Set environment to Development**

```bash
# Windows
set ASPNETCORE_ENVIRONMENT=Development

# macOS/Linux
export ASPNETCORE_ENVIRONMENT=Development
```

3. **Restore and run**

```bash
cd src/API
dotnet restore
dotnet run
```

4. **Apply database migrations**

Migrations run automatically on startup via `db.Database.Migrate()` in `Program.cs`.

Or run manually:

```bash
dotnet ef database update --project ../Infrastructure --startup-project .
```

5. **Open Swagger UI**

```
https://localhost:5000/swagger
```

---

## Configuration

The project uses environment-specific `appsettings` files that merge at runtime:

| File | Purpose |
|---|---|
| `appsettings.json` | Shared base settings (Jwt Issuer/Audience, Logging, AllowedHosts) |
| `appsettings.Development.json` | LocalDB connection string, debug logging |
| `appsettings.Production.json` | SQL Server connection string, warning-level logging |

### Environment Variables (override any appsettings value)

```bash
ConnectionStrings__DefaultConnection=...
Jwt__Key=...
Jwt__Issuer=CRNAPI
Jwt__Audience=CRNUsers
ASPNETCORE_ENVIRONMENT=Production
```

> **Security**: Never commit secrets to source control. Use environment variables or a secrets manager in production.

---

## Running with Docker

### Development

```bash
docker compose up --build
```

Uses `docker-compose.override.yml` automatically — sets `ASPNETCORE_ENVIRONMENT=Development` and maps port `5000:80`.

### Production

```bash
docker compose -f docker-compose.yml up -d --build
```

Sets `ASPNETCORE_ENVIRONMENT=Production`, starts SQL Server container, and applies migrations on startup.

### Services

| Service | Port | Description |
|---|---|---|
| `api` | `80` | ASP.NET Core Web API |
| `sqlserver` | `1433` | SQL Server 2022 |

---

## CI/CD Pipeline

The GitHub Actions workflow (`.github/workflows/deploy.yml`) triggers on every push to `master`:

```
git push → master
       ↓
Build Docker image
       ↓
Push to Docker Hub
       ↓
SSH into server → docker compose up
       ↓
Production environment live ✅
```

### Required GitHub Secrets

Go to **Settings → Secrets and variables → Actions** and add:

| Secret | Description |
|---|---|
| `DOCKER_USERNAME` | Docker Hub username |
| `DOCKER_PASSWORD` | Docker Hub password |
| `SERVER_HOST` | Production server IP or domain |
| `SERVER_USER` | SSH username (e.g. `ubuntu`) |
| `SERVER_SSH_KEY` | Private SSH key for server access |

---

## API Documentation

Swagger UI is available in **Development** only:

```
http://localhost:5000/swagger
```

API is versioned under `/api/v1/`:

| Method | Endpoint | Description | Auth |
|---|---|---|---|
| POST | `/api/v1/identity/login` | Get JWT token | No |
| GET | `/api/v1/products` | List all products | Yes |
| POST | `/api/v1/products` | Create product | Yes |
| GET | `/api/v1/products/{id}` | Get product by ID | Yes |
| PUT | `/api/v1/products/{id}` | Update product | Yes |
| DELETE | `/api/v1/products/{id}` | Delete product | Yes |
| GET | `/api/v1/items` | List all items | Yes |
| POST | `/api/v1/items` | Create item | Yes |

---

## Authentication

This API uses **JWT Bearer Authentication**.

1. Call `/api/v1/identity/login` with valid credentials to receive a token
2. Click **Authorize** in Swagger UI and enter: `Bearer <your-token>`
3. All protected endpoints will now be accessible

### JWT Configuration

| Setting | Value |
|---|---|
| Issuer | `CRNAPI` |
| Audience | `CRNUsers` |
| Algorithm | `HS256` |
| Expiry | Configured in `IdentityController` |

---

## Running Tests

```bash
# All tests
dotnet test

# Specific layer
dotnet test tests/Application.Tests
dotnet test tests/Infrastructure.Tests
dotnet test tests/API.Tests
```

---

## License

This project is for assessment purposes only.
