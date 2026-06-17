using EventHouse.Management.Api.Swagger.Filters;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EventHouse.Management.Api.Tests.Swagger.Filters;

public class JsonOnlyResponsesOperationFilterTests
{
    [Fact]
    public void Apply_ShouldRemoveNonJsonContent_WhenJsonResponseExists()
    {
        // 1. Arrange
        var operation = new OpenApiOperation
        {
            Responses = new OpenApiResponses
            {
                ["200"] = new OpenApiResponse
                {
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["application/json"] = new OpenApiMediaType(),
                        ["text/plain"] = new OpenApiMediaType(),
                        ["text/json"] = new OpenApiMediaType()
                    }
                }
            }
        };

        var context = new OperationFilterContext(null, null, null, null);
        var filter = new JsonOnlyResponsesOperationFilter();

        // 2. Act
        filter.Apply(operation, context);

        // 3. Assert
        var content = operation.Responses["200"].Content;

        Assert.Single(content);
        Assert.True(content.ContainsKey("application/json"));
        Assert.False(content.ContainsKey("text/plain"));
        Assert.False(content.ContainsKey("text/json"));
    }

    [Fact]
    public void Apply_ShouldKeepOtherContent_WhenJsonIsMissing()
    {
        var operation = new OpenApiOperation
        {
            Responses = new OpenApiResponses
            {
                ["200"] = new OpenApiResponse
                {
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["text/plain"] = new OpenApiMediaType()
                    }
                }
            }
        };

        var filter = new JsonOnlyResponsesOperationFilter();
        filter.Apply(operation, new OperationFilterContext(null, null, null, null));

        Assert.Single(operation.Responses["200"].Content);
        Assert.True(operation.Responses["200"].Content.ContainsKey("text/plain"));
    }
}