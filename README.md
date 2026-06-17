# 🎟️ EventHouse Management API

<p align="center">
  <img src="https://img.shields.io/badge/.NET-8.0-512bd4.svg" />
  <img src="https://img.shields.io/badge/PostgreSQL-16-blue.svg" />
  <img src="https://img.shields.io/badge/Docker-Container-2496ed.svg" />
  <img src="https://github.com/FEDERIN/eventhouse-management-api-demo/actions/workflows/ci.yml/badge.svg" />
</p>

<p align="center">
  <b>Cloud-Native • Clean Architecture • Observability-First • Production-Ready</b>
</p>

---

## 📖 Overview

**EventHouse Management API** is a modern backend system built with **.NET 8**, applying **DDD**, **CQRS**, and **Clean Architecture** principles.

It is designed as a **production-grade portfolio project**, showcasing scalability, observability, and modern cloud-native engineering practices.

---

## ✨ Key Features

| Category | Description |
|----------|-------------|
| 🏗 Architecture | Clean Architecture + DDD + CQRS |
| 🐘 Database | PostgreSQL 16 with EF Core |
| 🧪 Testing | Testcontainers + xUnit + Respawn |
| 📊 Observability | OpenTelemetry + Prometheus + Grafana |
| 🛡 Validation | FluentValidation + RFC 9457 errors |
| 🚀 Dev Experience | Docker-first development |

---

## 🧠 Architecture

```mermaid
graph TD
    %% =========================
    %% CLIENTE Y ENTRY POINT
    %% =========================
    Client((Cliente))
    Client --> API[EventHouse.Management.Api]

    %% =========================
    %% ARQUITECTURA INTERNA
    %% =========================
    subgraph Core [Core Domain & Application]
        Application[Application Layer]
        Domain[Domain Layer]
    end

    subgraph Infrastructure [Infrastructure Layer]
        Infra[Infrastructure Implementation]
        Gatekeeper[Idempotency Filter]
    end

    API --> Application
    API --> Infra

    Application --> Domain

    Infra --> Application
    Infra --> Domain

    %% =========================
    %% PERSISTENCIA
    %% =========================
    subgraph Storage [Storage & Data Services]
        Postgres[(PostgreSQL)]

        subgraph IdempotencyStore [Idempotency Storage]
            Redis[(Redis)]
            PgStore[(PostgreSQL Storage)]
        end
    end

    Infra --> Postgres

    Gatekeeper -- Check/Set Key --> Redis
    Gatekeeper -. Alternative .-> PgStore

    %% =========================
    %% OBSERVABILIDAD
    %% =========================
    API -. Telemetry .-> OTel[OpenTelemetry SDK]

    subgraph Observability [Observability Stack]
        Jaeger[Jaeger]
        Prometheus[Prometheus]
        Grafana[Grafana]
    end

    OTel --> Jaeger
    OTel --> Prometheus
    Prometheus --> Grafana

    %% =========================
    %% ESTILOS
    %% =========================
    style API fill:#f06292,stroke:#880e4f,color:#fff

    style Core fill:#e3f2fd,stroke:#2196f3
    style Infrastructure fill:#fff3e0,stroke:#ff9800

    style Storage fill:#f3e5f5,stroke:#9c27b0
    style Observability fill:#e8f5e9,stroke:#4caf50
```

---

## 🏗 Tech Stack

- .NET 8
- ASP.NET Core Web API
- PostgreSQL 16
- Entity Framework Core
- MediatR
- FluentValidation
- OpenTelemetry
- Docker & Docker Compose
- xUnit + Testcontainers

---

## 🚀 Getting Started

### 🐳 Run with Docker

```bash
docker-compose up -d --build
```

---

### 💻 Run Locally

```powershell
$Env:Auth__DevSecret="EVENTHOUSE_LOCAL_DEV_SECRET_32_CHARS_MINIMUM!!"
dotnet run --project EventHouse.Management.Api
```

---

## 🗄️ Database

### Create Migration

```bash
dotnet ef migrations add <MigrationName> --project EventHouse.Management.Infrastructure --startup-project EventHouse.Management.Api --output-dir Persistence/Migrations
```

### Update Database

```bash
dotnet ef database update --project EventHouse.Management.Infrastructure --startup-project EventHouse.Management.Api
```

---

## 🧪 Testing Strategy

- Unit Tests → Domain logic validation
- Integration Tests → PostgreSQL via Testcontainers
- Fast resets → Respawn
- Shared fixtures → optimized execution

---

## 📊 Observability

- 🔭 Distributed tracing → OpenTelemetry + Jaeger
- 📈 Metrics → Prometheus + Grafana
- 🩺 Health checks → `/health`
- 🧾 Correlation IDs → `X-Correlation-Id`

---

## 📁 Project Highlights

- PostgreSQL-first migration (from SQLite)
- Snake_case naming strategy
- Microsecond precision handling (`10^-6`)
- Shared test infrastructure project
- High-performance integration testing pipeline

---

## ⚡ Goals

This project demonstrates:

- Senior-level backend architecture
- Cloud-native design principles
- Realistic production testing strategies
- Scalable system design

---

## 📌 License

This project is for educational and portfolio purposes.
