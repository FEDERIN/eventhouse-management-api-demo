# Getting Started

## 🐳 Run with Docker

Copy `src/EventHouse.Management.Api/.env.example` to `src/EventHouse.Management.Api/.env` and fill in the values first, then:

```bash
cd src/EventHouse.Management.Api
docker compose up -d --build
```

## 💻 Run Locally

```powershell
$Env:Auth__DevSecret="EVENTHOUSE_LOCAL_DEV_SECRET_32_CHARS_MINIMUM!!"
$Env:MEDIATR_LICENSE_KEY="<your-mediatr-license-key>"
dotnet run --project src/EventHouse.Management.Api
```

## 🗄️ Database

### Create Migration

```bash
dotnet ef migrations add <MigrationName> --project src/EventHouse.Management.Infrastructure --startup-project src/EventHouse.Management.Api --output-dir Persistence/Migrations
```

### Update Database

```bash
dotnet ef database update --project src/EventHouse.Management.Infrastructure --startup-project src/EventHouse.Management.Api
```
