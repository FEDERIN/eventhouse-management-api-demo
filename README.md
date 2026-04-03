# eventhouse-management-api-demo

Demo REST API applying DDD and Clean Architecture with .NET, JWT authentication, Swagger, and Azure-ready patterns.

---

## Run locally

### Set development secret

```powershell
$Env:Auth__DevSecret="EVENTHOUSE_LOCAL_DEV_SECRET_32_CHARS_MINIMUM!!"


Run API
dotnet run --project EventHouse.Management.Api

### Update BD


dotnet ef database update ` --project EventHouse.Management.Infrastructure --startup-project EventHouse.Management.Api

## Observability

### Correlation ID
All responses include the `X-Correlation-Id` header to enable end-to-end request tracing.
You can also provide your own correlation id in the request header:

```bash
curl -i -H "X-Correlation-Id: demo123" http://localhost:5185/api/v1/artists
curl http://localhost:5185/health
curl http://localhost:5185/ready


# Tests Standards & Principles
    - Fail-Fast Approach: Tests are ordered by "depth" in the tech stack so failures occur as early as possible in the pipeline.
    - RFC 9110 Compliance: Strict adherence to HTTP semantics: 201 Created (with Location header), 204 No Content (updates/deletes), 409 Conflict (state collisions), and 404 Not Found.
    - Clean Testing: Use of the Factory Pattern (via Bogus) to ensure test data is decoupled from test logic, adhering to the F.I.R.S.T. principles of testing.

## Tests and code coverage (local)
#dotnet test EventHouse.Management.Infrastructure.Tests\EventHouse.Management.Infrastructure.Tests.csproj --collect:"XPlat Code Coverage"
#dotnet test EventHouse.Management.Domain.Tests\EventHouse.Management.Domain.Tests.csproj --collect:"XPlat Code Coverage"
#dotnet test EventHouse.Management.Api.Tests\EventHouse.Management.Api.Tests.csproj --collect:"XPlat Code Coverage"
#dotnet test EventHouse.Management.Application.Tests\EventHouse.Management.Application.Tests.csproj --collect:"XPlat Code Coverage"
dotnet test EventHouse.sln --collect:"XPlat Code Coverage"


## Generate HTML coverage report
reportgenerator -reports:"EventHouse.Management.Domain.Tests\TestResults\*\coverage.cobertura.xml;EventHouse.Management.Application.Tests\TestResults\*\coverage.cobertura.xml;EventHouse.Management.Api.Tests\TestResults\*\coverage.cobertura.xml;EventHouse.Management.Infrastructure.Tests\TestResults\*\coverage.cobertura.xml" -targetdir:"coverage-report" -reporttypes:Html -filefilters:"-*\\obj\\*;-*RegexGenerator.g.cs"



## Open the report:
start coverage-report/index.html


## Observability

### Correlation ID
All responses include the `X-Correlation-Id` header to enable end-to-end request tracing.

You can also provide your own correlation id in the request header:

```bash
curl -i -H "X-Correlation-Id: demo123" http://localhost:5185/api/v1/artists
curl http://localhost:5185/health
curl http://localhost:5185/ready

