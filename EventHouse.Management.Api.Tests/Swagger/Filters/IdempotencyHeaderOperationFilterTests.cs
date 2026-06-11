using EventHouse.Management.Api.Swagger.Filters;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EventHouse.Management.Api.Tests.Swagger.Filters;

public class IdempotencyHeaderOperationFilterTests
{
    [Fact]
    public void Apply_ShouldAddHeader_WhenIdempotencyIsEnabled()
    {
        // 1. Arrange
        var inMemorySettings = new Dictionary<string, string?> {
            {"Idempotency:Enabled", "true"},
            {"Idempotency:AllowedMethods", "POST"},
            {"Idempotency:HeaderName", "X-Idempotency-Key"}
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings
                .Select(kv => new KeyValuePair<string, string?>(kv.Key, kv.Value)))
            .Build();

        var filter = new IdempotencyHeaderOperationFilter(configuration);

        // 2. Mock
        var operation = new OpenApiOperation { Parameters = [] };
        var apiDescription = new ApiDescription { HttpMethod = "POST" };
        var context = new OperationFilterContext(apiDescription, null, null, null);

        // 3. Act
        filter.Apply(operation, context);

        // 4. Assert
        Assert.Contains(operation.Parameters, p => p.Name == "X-Idempotency-Key");
    }
}