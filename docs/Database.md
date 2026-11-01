# Database

PostgreSQL 16 with EF Core, snake_case naming, microsecond precision.

## Create Migration

```bash
dotnet ef migrations add <MigrationName> --project src/EventHouse.Management.Infrastructure --startup-project src/EventHouse.Management.Api --output-dir Persistence/Migrations
```

## Update Database

```bash
dotnet ef database update --project src/EventHouse.Management.Infrastructure --startup-project src/EventHouse.Management.Api
```
