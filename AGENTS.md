# AGENTS.md — EventHouse Management API

.NET 8 Web API using Clean Architecture + DDD + CQRS (MediatR), PostgreSQL 16 (EF Core), Redis, OpenTelemetry, Docker. Solution: `EventHouse.sln`.

## Layout

- `src/EventHouse.Management.Api` — entrypoint; also holds `docker-compose.yml`, `Dockerfile`, `otel-collector-config.yml`, `prometheus.yml`, `.env.example`
- `src/EventHouse.Management.Application` / `Domain` / `Infrastructure` / `OpenApi`, `src/EventHouse.ShareKernel`
- `tests/` — per-project test projects + `EventHouse.Management.TestUtils` (shared fixtures) + `EventHouse.Management.Architecture.Tests` (layering rules — keep passing when adding references)

## Commands (verified against CI: `.github/workflows/ci.yml`)

```bash
dotnet restore EventHouse.sln --locked-mode
dotnet build EventHouse.sln --configuration Release --no-restore
dotnet test EventHouse.sln --configuration Release --no-build --settings coverlet.runsettings --collect:"XPlat Code Coverage"
```

## Gotchas (will bite you)

- **SDK pinned**: `global.json` → .NET SDK 8.0.402. Use .NET 8, not 9.
- **NuGet lock files are enforced**: `Directory.Build.props` sets `RestoreLockedMode=true`. Never put `Version=` on a `PackageReference` (CI greps for it and fails) — versions live only in `Directory.Packages.props`. After changing packages, regenerate locks with `generate-lock-files.bat` (or `dotnet restore EventHouse.sln --use-lock-file --force-evaluate`) and commit the updated `packages.lock.json` files.
- **Integration tests need Docker running**: `Api.Tests` and `Infrastructure.Tests` use Testcontainers (`postgres:16-alpine`) + Respawn. `dotnet test` fails without a Docker daemon; first run pulls images.
- **Required secrets to run locally**: `Auth__DevSecret` (≥32 chars) and `MEDIATR_LICENSE_KEY` (MediatR license). `DotNetEnv` loads `src/EventHouse.Management.Api/.env` — copy from `.env.example` there (the `.env` is git-ignored). README's `dotnet run --project EventHouse.Management.Api` also needs `Auth__DevSecret` in the environment.
- **docker-compose is NOT at repo root**: it lives in `src/EventHouse.Management.Api/`. Run `docker compose up` from that directory.
- **EF migrations** (from repo root):
  ```bash
  dotnet ef migrations add <Name> --project src/EventHouse.Management.Infrastructure --startup-project src/EventHouse.Management.Api --output-dir Persistence/Migrations
  dotnet ef database update --project src/EventHouse.Management.Infrastructure --startup-project src/EventHouse.Management.Api
  ```

## Conventions

- Code comments, commit history, and some user-facing strings are in Spanish — match surrounding style when editing existing files.
- EF Core uses snake_case tables/columns (`EFCore.NamingConventions`) with microsecond precision; coverage excludes migrations via `coverlet.runsettings`.

## Memory

- At the start, read `MEMORY.md` to learn the current state of the project and past decisions.
- When a task is finished, update it: current state, important decisions (with their rationale), and mistakes to avoid.
- Keep it short (max ~50 lines): summarize or drop what no longer adds value.
- If something becomes a permanent rule, propose moving it to `AGENTS.md` instead of keeping it in memory.
- Never store sensitive data (keys, tokens, personal data).
