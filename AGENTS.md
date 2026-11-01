# AGENTS.md

## 1. Engineering Standards

Before performing any task, read `AGENTS-STANDARDS.md`.

These standards are mandatory and apply to all development
activities in this repository.

The repository-specific instructions in this file complement
the common standards.

If a project-specific rule differs from a common standard,
follow the explicit project-specific exception.

## 2. Repository Overview

EventHouse Management API is a .NET 8/9 Web API (multi-targeted)
using Clean Architecture + DDD + CQRS (MediatR), PostgreSQL 16
(EF Core), Redis, OpenTelemetry, Docker.

- Solution: `EventHouse.sln`
- Source: `src/`
- Documentation: none (MkDocs not used)
- Local run: docker-compose in `src/EventHouse.Management.Api/`

Follow `AGENTS-STANDARDS.md` for common engineering rules.

## 3. Repository Architecture

- Clean Architecture layers: `Domain`, `Application`, `Infrastructure`,
  `Api`, plus `OpenApi` and shared `ShareKernel`.
- `Application` uses MediatR (CQRS); handlers live under
  `Application/...`.
- `Infrastructure` holds EF Core, Redis, external integrations.
- Architecture tests enforce layering rules in
  `tests/EventHouse.Management.Architecture.Tests`.
- Do not introduce references that violate architecture tests.

## 4. Dependencies

- Package versions are centralized in `Directory.Packages.props`.
- `Directory.Build.props` enables `RestoreLockedMode`.
- After adding, removing or changing a package, run
  `dotnet restore EventHouse.sln --use-lock-file --force-evaluate`
  and commit the updated `packages.lock.json` files.
- Per-TFM packages are pinned with conditional
  `<ItemGroup Condition="'$(TargetFramework)' == 'netX.0'">` blocks
  in `Directory.Packages.props`.
- Integration tests use Testcontainers; Docker must be running.
- CoreSystem.* packages (cache, correlation, idempotency, observability,
  rate limiting, resilience, etc.) are external dependencies; bump their
  central versions deliberately and verify compatibility per TFM.

## 5. Framework Configuration

- SDK: 9.0.320 (`global.json`, `rollForward: latestFeature`)
- Target frameworks: `net8.0;net9.0`
- Multi-targeting from `Directory.Build.props`.
- Nullable Reference Types and Implicit Usings enabled.

## 6. Testing

- Unit tests: `tests/EventHouse.Management.*.Tests`
- Integration tests: `Api.Tests` and `Infrastructure.Tests`
  (Testcontainers postgres:16-alpine + Respawn).
- Shared fixtures in `tests/EventHouse.Management.TestUtils`.
- Run all tests: `dotnet test EventHouse.sln`.

## 7. Build and Commands

```bash
dotnet restore EventHouse.sln --locked-mode
dotnet build EventHouse.sln --configuration Release --no-restore
dotnet test EventHouse.sln --configuration Release --no-build --settings coverlet.runsettings --collect:"XPlat Code Coverage"
```

EF migrations (from repo root):

```bash
dotnet ef migrations add <Name> --project src/EventHouse.Management.Infrastructure --startup-project src/EventHouse.Management.Api --output-dir Persistence/Migrations
dotnet ef database update --project src/EventHouse.Management.Infrastructure --startup-project src/EventHouse.Management.Api
```

docker-compose lives in `src/EventHouse.Management.Api/`.

## 8. CI/CD

- GitHub Actions (`.github/workflows/ci.yml`).
- Restore, build, tests with coverage.
- `main` is protected: use feature branches and PRs.

## 9. Documentation

- MkDocs documentation (see `requirements.txt`).
- Code comments, commits, PRs and user-facing strings in English.
- EF Core uses snake_case tables/columns with microsecond precision.

## 10. Memory

Read `MEMORY.md` at the beginning of a task.
Update it when significant decisions or project state change.
Keep it concise and avoid duplicating permanent rules
(max ~50 lines: summarize or drop what no longer adds value).
If something becomes a permanent rule, propose moving it to
`AGENTS.md` instead of keeping it in memory.
Never store sensitive data (keys, tokens, personal data).
