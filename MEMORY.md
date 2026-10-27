# MEMORY.md — EventHouse Management API

## Current state
- .NET 8 Clean Architecture + DDD + CQRS (MediatR), PostgreSQL 16 (EF Core), Redis, OpenTelemetry, Docker. Solution `EventHouse.sln`.
- CI runs restore/build/test with locked NuGet + coverlet. Integration tests need Docker.
- Secrets required: `Auth__DevSecret` (≥32 chars) and `MEDIATR_LICENSE_KEY`; `.env` loaded from `src/EventHouse.Management.Api/.env` (git-ignored).

## Important decisions
- NuGet versions only in `Directory.Packages.props`; never `Version=` on `PackageReference` (CI grep).
- EF Core snake_case tables/columns with microsecond precision; coverage excludes migrations.
- docker-compose lives in `src/EventHouse.Management.Api/`, not repo root.
- Code comments/commits/user-facing strings partly in Spanish; match surrounding style.

## Mistakes to avoid
- Don't run `dotnet test` without Docker daemon (Testcontainers needs it).
- Don't edit `AGENTS.md` rules into memory; propose permanent rules there instead.
- Never store secrets in this file.