# 🎟️ EventHouse Management API (Demo)

A demo REST API built with **.NET 8**, applying **Domain-Driven Design (DDD)** and **Clean Architecture** principles.

Includes:

- 🔐 JWT Authentication
- 🧱 Layered Architecture (Domain, Application, Infrastructure, API)
- 📊 Cloud-Native Observability (health checks, tracing, correlation IDs)
- 🐳 Fully containerized environment (Docker)

---

## 🚀 Getting Started

### ▶️ Run with Docker (Recommended)

The full ecosystem (API + Database + Observability stack) is orchestrated via Docker.

From the root directory:

```powershell
docker-compose -f .\eventhouse-management-api\EventHouse.Management.Api\docker-compose.yml up -d --build
```

---

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

### 🗄️ Database Setup

#### Add Migration

```powershell
dotnet ef migrations add addIsConcurrencyToken `
--project EventHouse.Management.Infrastructure `
--startup-project EventHouse.Management.Api `
--output-dir Persistence/Migrations
```

#### Apply Migration

```powershell
dotnet ef database update `
--project EventHouse.Management.Infrastructure `
--startup-project EventHouse.Management.Api
```

---

## 🔍 Observability

### Correlation ID

All responses include an `X-Correlation-Id` header for end-to-end tracing.

You can also provide your own:

```bash
curl -i -H "X-Correlation-Id: demo123" http://localhost:5185/api/v1/artists
```

## 🧠 Repository Design Standards

Repositories follow a **Lifecycle-Based Ordering** strategy aligned with **ISO/IEC 25010 maintainability principles**:

1. **Commands First**
   - Methods that modify state (`Add`, `Update`)

2. **Queries Second**
   - Retrieval methods (`Get`, `Paged`)

3. **Validation & Existence**
   - Support methods like `ExistsAsync`

### 🎯 Benefits

- Predictable structure across modules
- Reduced cognitive load
- Improved maintainability

---

## 🧪 Testing Standards & Principles

- ⚡ **Fail-Fast Approach**
- 🌐 **RFC 9110 Compliance**
- 🧼 **Clean Testing with Factory Pattern (Bogus)**

---

## 📊 Code Coverage

### Run Tests with Coverage

```bash
dotnet test EventHouse.sln --collect:"XPlat Code Coverage"
```

### Generate HTML Report

```bash
reportgenerator -reports:"EventHouse.Management.Domain.Tests\TestResults\*\coverage.cobertura.xml;EventHouse.Management.Application.Tests\TestResults\*\coverage.cobertura.xml;EventHouse.Management.Api.Tests\TestResults\*\coverage.cobertura.xml;EventHouse.Management.Infrastructure.Tests\TestResults\*\coverage.cobertura.xml" -targetdir:"coverage-report" -reporttypes:Html -filefilters:"-*\\obj\\*;-*RegexGenerator.g.cs"
```

### Open Report

```bash
start coverage-report/index.html
```

---

## 🏗️ Architecture Highlights

- Clean separation of concerns (DDD layers)
- Strong testing discipline
- Observability-first mindset
- Production-ready patterns

---

## 📌 Notes

This project is intended as a **portfolio demonstration** of backend architecture and best practices in modern .NET development.
