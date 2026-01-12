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
dotnet test `
  /p:CollectCoverage=true `
  /p:CoverletOutput=./coverage/coverage `
  /p:CoverletOutputFormat=cobertura `
  /p:MergeWith=./coverage/coverage.cobertura.xml

## Generate HTML coverage report
reportgenerator -reports:"EventHouse.Management.Api.Tests\coverage\coverage.cobertura.xml" -targetdir:"coverage-report" -reporttypes:Html


## Open the report:

start coverage-report/index.html

