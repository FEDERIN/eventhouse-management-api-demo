# Onboarding

## Prerequisites

- .NET SDK 9.0.320 (see `global.json`)
- Docker (for integration tests and compose)
- Python 3.12 (for MkDocs docs)

## Local run

1. Copy `src/EventHouse.Management.Api/.env.example` to `src/EventHouse.Management.Api/.env` and fill values.
2. Set secrets:
   - `Auth__DevSecret` (>=32 chars)
   - `MEDIATR_LICENSE_KEY`
3. Run:

```powershell
$Env:Auth__DevSecret="EVENTHOUSE_LOCAL_DEV_SECRET_32_CHARS_MINIMUM!!"
$Env:MEDIATR_LICENSE_KEY="<your-mediatr-license-key>"
dotnet run --project src/EventHouse.Management.Api
```

## Docker

```bash
cd src/EventHouse.Management.Api
docker compose up -d --build
```
