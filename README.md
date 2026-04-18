# 🎟️ EventHouse Management API (Cloud-Native Demo)

![CI Build & Test](https://github.com/TU_USUARIO/TU_REPO/actions/workflows/ci.yml/badge.svg)  
![.NET](https://img.shields.io/badge/.NET-8.0-512bd4.svg)  
![Docker](https://img.shields.io/badge/Docker-Container-2496ed.svg)

A professional-grade REST API built with **.NET 8**, applying **Domain-Driven Design (DDD)**, **Clean Architecture**, and an **Observability-First** approach.

---

## 🏗️ Architecture & Patterns

- 🧱 **Clean Architecture:** Clear separation between Domain, Application, Infrastructure, and API layers  
- 🧠 **DDD & CQRS:** Business logic isolated in the Domain; state changes handled via **MediatR**  
- 🧪 **Testing Excellence:** Unit & Integration tests with **xUnit** and **Bogus**  
- 🛡️ **Robustness:** Validation via **FluentValidation** and standardized errors (**RFC 9457**)

---

## 📊 Observability & Monitoring

- 🛰️ **Distributed Tracing:** Integrated with **OpenTelemetry** and **Jaeger**  
- 📈 **Metrics:** Monitoring via **Prometheus** and **Grafana**  
- 🩺 **Health Checks:** Available at `/health`  
- 🆔 **Correlation IDs:** End-to-end tracing via `X-Correlation-Id` header  

---

## 🚀 Getting Started

### ▶️ Run with Docker (Recommended)

Full environment (**API + Database + Observability stack**) with Docker:

```bash
Copy-Item ".\Data\management.db" ".\sqlite_data\EventHouse.db" -Force
docker-compose -f docker-compose.yml up -d --build
```

### 💻 Run Locally

#### 1. Set Development Secret

```powershell
$Env:Auth__DevSecret="EVENTHOUSE_LOCAL_DEV_SECRET_32_CHARS_MINIMUM!!"
```

#### 2. Run the API

```bash
dotnet run --project EventHouse.Management.Api
```

---

## 🗄️ Database Setup

### Add Migration

```powershell
dotnet ef migrations add addIsConcurrencyToken `
--project EventHouse.Management.Infrastructure `
--startup-project EventHouse.Management.Api `
--output-dir Persistence/Migrations
```

### Apply Migration

```powershell
dotnet ef database update `
--project EventHouse.Management.Infrastructure `
--startup-project EventHouse.Management.Api
```

---

## 🔍 Observability Usage

### Correlation ID Example

```bash
curl -i -H "X-Correlation-Id: demo123" http://localhost:5185/api/v1/artists
```

---

## 🧠 Repository Design Standards

Repositories follow a **Lifecycle-Based Ordering** approach:

1. **Commands First** → (`Add`, `Update`)  
2. **Queries Second** → (`Get`, `Paged`)  
3. **Validation Methods** → (`ExistsAsync`)  

### 🎯 Benefits

- Predictable structure  
- Reduced cognitive load  
- Improved maintainability  

---

## 🧪 Testing Standards

- ⚡ Fail-fast approach  
- 🌐 RFC 9110 compliance  
- 🧼 Factory-based test data (Bogus)  

---

## 📊 Code Coverage

### Run Tests

```bash
dotnet test EventHouse.sln --collect:"XPlat Code Coverage"
```

### Generate Report

```bash
reportgenerator -reports:"EventHouse.Management.Domain.Tests\TestResults\*\coverage.cobertura.xml;EventHouse.Management.Application.Tests\TestResults\*\coverage.cobertura.xml;EventHouse.Management.Api.Tests\TestResults\*\coverage.cobertura.xml;EventHouse.Management.Infrastructure.Tests\TestResults\*\coverage.cobertura.xml" -targetdir:"coverage-report" -reporttypes:Html -filefilters:"-*\\obj\\*;-*RegexGenerator.g.cs"
```

### Open Report

```bash
start coverage-report/index.html
```

---

## 📌 Notes

This project is a **portfolio demonstration** showcasing modern backend architecture and best practices in .NET development.
