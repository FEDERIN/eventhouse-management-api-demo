# eventhouse-management-api-demo

Demo REST API applying DDD and Clean Architecture with .NET, JWT authentication, Swagger, and Azure-ready patterns.

---

## Run locally

### Set development secret
```powershell
$Env:Auth__DevSecret="EVENTHOUSE_LOCAL_DEV_SECRET_32_CHARS_MINIMUM!!"


Run API
dotnet run --project EventHouse.Management.Api


## Tests and code coverage (local)
dotnet test /p:CollectCoverage=true /p:CoverletOutput=./coverage/coverage /p:CoverletOutputFormat=cobertura /p:ExcludeByFile="**/Swagger/Examples/**/*.cs" 

dotnet test /p:MergeWith=./coverage/coverage.cobertura.xml
## Generate HTML coverage report
reportgenerator -reports:"EventHouse.Management.Api.Tests\coverage\coverage.cobertura.xml" -targetdir:"coverage-report" -reporttypes:Html -filefilters:"-*\\obj\\*;-*RegexGenerator.g.cs"

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

