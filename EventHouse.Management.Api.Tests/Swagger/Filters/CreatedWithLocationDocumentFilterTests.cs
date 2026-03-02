using EventHouse.Management.Api.Swagger.Filters;
using Microsoft.OpenApi.Models;

namespace EventHouse.Management.Api.Tests.Swagger.Filters;

public sealed class CreatedWithLocationDocumentFilterTests
{
    [Fact]
    public void Apply_AddsLocationHeader_ToOperationsWith201Response()
    {
        // Arrange
        var doc = new OpenApiDocument
        {
            Paths = new OpenApiPaths
            {
                ["/artists"] = new OpenApiPathItem
                {
                    Operations =
                    {
                        [OperationType.Post] = new OpenApiOperation
                        {
                            Responses = new OpenApiResponses
                            {
                                ["201"] = new OpenApiResponse
                                {
                                    Description = "Created",
                                    Headers = null // importante: tu filtro debe inicializarlo
                                }
                            }
                        }
                    }
                }
            }
        };

        var filter = new CreatedWithLocationDocumentFilter();

        // Act
        filter.Apply(doc, null!);

        // Assert
        var response201 = doc.Paths["/artists"].Operations[OperationType.Post].Responses["201"];
        Assert.NotNull(response201.Headers);
        Assert.True(response201.Headers!.ContainsKey("Location"));

        var location = response201.Headers["Location"];
        Assert.Equal("URL of the newly created resource", location.Description);
        Assert.NotNull(location.Schema);
        Assert.Equal("string", location.Schema.Type);
        Assert.Equal("uri", location.Schema.Format);
    }

    [Fact]
    public void Apply_DoesNotAddLocationHeader_WhenNo201Response()
    {
        // Arrange
        var doc = new OpenApiDocument
        {
            Paths = new OpenApiPaths
            {
                ["/artists"] = new OpenApiPathItem
                {
                    Operations =
                    {
                        [OperationType.Get] = new OpenApiOperation
                        {
                            Responses = new OpenApiResponses
                            {
                                ["200"] = new OpenApiResponse { Description = "OK" }
                            }
                        }
                    }
                }
            }
        };

        var filter = new CreatedWithLocationDocumentFilter();

        // Act
        filter.Apply(doc, null!);

        // Assert
        var response200 = doc.Paths["/artists"].Operations[OperationType.Get].Responses["200"];
        Assert.True(response200.Headers is null || !response200.Headers.ContainsKey("Location"));
    }

    [Fact]
    public void Apply_IsIdempotent_DoesNotDuplicateOrThrow_WhenAppliedTwice()
    {
        // Arrange
        var doc = new OpenApiDocument
        {
            Paths = new OpenApiPaths
            {
                ["/events"] = new OpenApiPathItem
                {
                    Operations =
                    {
                        [OperationType.Post] = new OpenApiOperation
                        {
                            Responses = new OpenApiResponses
                            {
                                ["201"] = new OpenApiResponse
                                {
                                    Description = "Created",
                                    Headers = new Dictionary<string, OpenApiHeader>
                                    {
                                        ["Location"] = new OpenApiHeader
                                        {
                                            Description = "existing",
                                            Schema = new OpenApiSchema { Type = "string" }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        var filter = new CreatedWithLocationDocumentFilter();

        // Act
        filter.Apply(doc, null!);
        filter.Apply(doc, null!);

        // Assert
        var response201 = doc.Paths["/events"].Operations[OperationType.Post].Responses["201"];
        Assert.NotNull(response201.Headers);
        Assert.True(response201.Headers!.ContainsKey("Location"));
        // Sigue habiendo solo una key "Location"
        Assert.Single(response201.Headers);
        // Como usas TryAdd, no pisa el existente
        Assert.Equal("existing", response201.Headers["Location"].Description);
    }
}
