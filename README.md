# Enterprise React Workbench - Microservices + CQRS + EF Core + PostgreSQL + Azure Key Vault

This is a production-style full-stack solution based on the earlier `enterprise-react-workbench-fullstack` app.

## Architecture

```text
client/ React Vite + Axios
services/
  IdentityService/       Login, Register, JWT token
  UserService/           User CRUD using CQRS + EF Core + PostgreSQL
  DashboardService/      Dashboard read API using CQRS queries
  NotificationService/   Notification API using CQRS + EF Core + PostgreSQL
infra/
  postgres/init.sql      Schemas and seed data
  docker-compose.yml     Local run with PostgreSQL and all APIs
  azure/deploy-azure.sh  Azure deployment script
  k8s/                   Kubernetes manifests 
 localsetup              psql -U postgres -d workbenchdb -f database-init.sql

```

## Tech Stack

- React + Vite + Axios
- .NET 8 Web API
- CQRS using MediatR
- EF Core with PostgreSQL provider
- PostgreSQL schemas with seed data
- Azure Key Vault configuration provider
- Managed Identity-ready configuration
- Dockerfiles for services
- Docker Compose for local development
- Azure Container Apps deployment script

## Local run

### 1. Run infrastructure and services

```bash
cd infra
docker compose up --build
```

This starts:

```text
PostgreSQL: localhost:5432
Identity API: http://localhost:5001/swagger
User API: http://localhost:5002/swagger
Dashboard API: http://localhost:5003/swagger
Notification API: http://localhost:5004/swagger
React client: http://localhost:5173
```

### 2. Default login

```text
Email: prashanth@enterprise.com
Password: Password@123
```

## Azure Key Vault secret names

For local Docker Compose, connection strings are passed via environment variables.

For Azure, store this secret in Key Vault:

```text
ConnectionStrings--Postgres
```

Example value:

```text
Host=<server>.postgres.database.azure.com;Port=5432;Database=workbenchdb;Username=<user>;Password=<password>;SSL Mode=Require;Trust Server Certificate=true
```

The services read Key Vault using:

```csharp
builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUri), new DefaultAzureCredential());
```

## Important notes

- For real production, replace demo password hashing with ASP.NET Core Identity or a strong password hashing strategy.
- Add API Gateway or Azure API Management in front of services for production.
- Add distributed tracing, centralized logging, and resilience policies.
