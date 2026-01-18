using System.Text.Json;
using System.Text.Json.Nodes;
using EventHouse.Management.Api.Swagger;

namespace EventHouse.Management.Api.Tests.Swagger.Filters;

public sealed class SwaggerJsonRefPatcherTests
{
    [Fact]
    public void Patch_WhenJsonIsInvalid_ReturnsOriginal()
    {
        var input = "{ not-json }";

        var output = SwaggerJsonRefPatcher.Patch(input);

        Assert.Equal(input, output);
    }

    [Fact]
    public void Patch_WhenJsonHasNoPaths_StillAddsCommonResponses()
    {
        var input = """
        {
          "openapi": "3.0.4",
          "info": { "title": "X", "version": "v1" }
        }
        """;

        var output = SwaggerJsonRefPatcher.Patch(input);
        var root = JsonNode.Parse(output)!.AsObject();

        var responses = root["components"]!["responses"]!.AsObject();

        Assert.True(responses.ContainsKey("Unauthorized"));
        Assert.True(responses.ContainsKey("Forbidden"));
        Assert.True(responses.ContainsKey("InternalError"));
        Assert.True(responses.ContainsKey("NotFound"));
        Assert.True(responses.ContainsKey("Conflict"));
        Assert.True(responses.ContainsKey("ValidationError"));
        Assert.True(responses.ContainsKey("TooManyRequests"));
    }

    [Fact]
    public void Patch_ReplacesKnownStatusCodesWithRefs_WhenTheyExist()
    {
        var input = """
        {
          "openapi": "3.0.4",
          "info": { "title": "X", "version": "v1" },
          "paths": {
            "/api/v1/artists/{id}": {
              "get": {
                "responses": {
                  "200": { "description": "OK" },
                  "401": { "description": "whatever" },
                  "403": { "description": "whatever" },
                  "404": { "description": "whatever" },
                  "409": { "description": "whatever" },
                  "429": { "description": "whatever" },
                  "500": { "description": "whatever" }
                }
              }
            }
          }
        }
        """;

        var output = SwaggerJsonRefPatcher.Patch(input);
        var root = JsonNode.Parse(output)!.AsObject();

        var responses = root["paths"]!["/api/v1/artists/{id}"]!["get"]!["responses"]!.AsObject();

        Assert.Equal("#/components/responses/Unauthorized", responses["401"]!["$ref"]!.ToString());
        Assert.Equal("#/components/responses/Forbidden", responses["403"]!["$ref"]!.ToString());
        Assert.Equal("#/components/responses/NotFound", responses["404"]!["$ref"]!.ToString());
        Assert.Equal("#/components/responses/Conflict", responses["409"]!["$ref"]!.ToString());
        Assert.Equal("#/components/responses/TooManyRequests", responses["429"]!["$ref"]!.ToString());
        Assert.Equal("#/components/responses/InternalError", responses["500"]!["$ref"]!.ToString());

        // 200 no se toca
        Assert.False(responses["200"]!.AsObject().ContainsKey("$ref"));
    }

    [Fact]
    public void Patch_DoesNotAddRefForStatusCodeThatDoesNotExist()
    {
        var input = """
        {
          "openapi": "3.0.4",
          "info": { "title": "X", "version": "v1" },
          "paths": {
            "/x": {
              "get": {
                "responses": {
                  "200": { "description": "OK" }
                }
              }
            }
          }
        }
        """;

        var output = SwaggerJsonRefPatcher.Patch(input);
        var root = JsonNode.Parse(output)!.AsObject();

        var responses = root["paths"]!["/x"]!["get"]!["responses"]!.AsObject();

        Assert.True(responses.ContainsKey("200"));
        Assert.False(responses.ContainsKey("401")); // no se inventa en paths
    }

    [Fact]
    public void Patch_Replaces400WithValidationError_WhenContentIsProblemJson()
    {
        var input = """
        {
          "openapi": "3.0.4",
          "info": { "title": "X", "version": "v1" },
          "paths": {
            "/x": {
              "post": {
                "responses": {
                  "400": {
                    "description": "Bad Request",
                    "content": {
                      "application/problem+json": {
                        "schema": { "$ref": "#/components/schemas/ValidationProblemDetails" }
                      }
                    }
                  }
                }
              }
            }
          }
        }
        """;

        var output = SwaggerJsonRefPatcher.Patch(input);
        var root = JsonNode.Parse(output)!.AsObject();

        var resp400 = root["paths"]!["/x"]!["post"]!["responses"]!["400"]!.AsObject();
        Assert.Equal("#/components/responses/ValidationError", resp400["$ref"]!.ToString());
    }

    [Fact]
    public void Patch_Replaces400WithValidationError_WhenContentIsApplicationJsonAndSchemaIsValidationProblemDetails()
    {
        var input = """
        {
          "openapi": "3.0.4",
          "info": { "title": "X", "version": "v1" },
          "paths": {
            "/x": {
              "post": {
                "responses": {
                  "400": {
                    "description": "Bad Request",
                    "content": {
                      "application/json": {
                        "schema": { "$ref": "#/components/schemas/ValidationProblemDetails" }
                      }
                    }
                  }
                }
              }
            }
          }
        }
        """;

        var output = SwaggerJsonRefPatcher.Patch(input);
        var root = JsonNode.Parse(output)!.AsObject();

        var resp400 = root["paths"]!["/x"]!["post"]!["responses"]!["400"]!.AsObject();
        Assert.Equal("#/components/responses/ValidationError", resp400["$ref"]!.ToString());
    }

    [Fact]
    public void Patch_DoesNotTouch400_WhenItIsAlreadyARef()
    {
        var input = """
        {
          "openapi": "3.0.4",
          "info": { "title": "X", "version": "v1" },
          "paths": {
            "/x": {
              "post": {
                "responses": {
                  "400": { "$ref": "#/components/responses/ValidationError" }
                }
              }
            }
          }
        }
        """;

        var output = SwaggerJsonRefPatcher.Patch(input);
        var root = JsonNode.Parse(output)!.AsObject();

        var resp400 = root["paths"]!["/x"]!["post"]!["responses"]!["400"]!.AsObject();
        Assert.Equal("#/components/responses/ValidationError", resp400["$ref"]!.ToString());
    }

    [Fact]
    public void Patch_IsIdempotent_RunningTwiceProducesSameJsonStructure()
    {
        var input = """
        {
          "openapi": "3.0.4",
          "info": { "title": "X", "version": "v1" },
          "paths": {
            "/x": {
              "get": { "responses": { "401": { "description": "x" }, "500": { "description": "x" } } }
            }
          }
        }
        """;

        var once = SwaggerJsonRefPatcher.Patch(input);
        var twice = SwaggerJsonRefPatcher.Patch(once);

        // Comparación estructural (no string) por si cambia el orden de propiedades
        var n1 = JsonNode.Parse(once)!.ToJsonString(new JsonSerializerOptions { WriteIndented = false });
        var n2 = JsonNode.Parse(twice)!.ToJsonString(new JsonSerializerOptions { WriteIndented = false });

        Assert.Equal(n1, n2);
    }

    [Fact]
    public void Patch_TooManyRequestsResponse_HasRetryAfterHeader()
    {
        var input = """
        {
          "openapi": "3.0.4",
          "info": { "title": "X", "version": "v1" }
        }
        """;

        var output = SwaggerJsonRefPatcher.Patch(input);
        var root = JsonNode.Parse(output)!.AsObject();

        var tmr = root["components"]!["responses"]!["TooManyRequests"]!.AsObject();
        var headers = tmr["headers"]!.AsObject();

        Assert.True(headers.ContainsKey("Retry-After"));
        Assert.Equal("integer", headers["Retry-After"]!["schema"]!["type"]!.ToString());
    }

    [Fact]
    public void Patch_Replaces400_WhenContentIsMissing()
    {
        var input = """
    {
      "openapi": "3.0.4",
      "info": { "title": "X", "version": "v1" },
      "paths": {
        "/x": {
          "post": {
            "responses": {
              "400": {
                "description": "Bad Request"
              }
            }
          }
        }
      }
    }
    """;

        var output = SwaggerJsonRefPatcher.Patch(input);
        var root = JsonNode.Parse(output)!.AsObject();

        var resp400 = root["paths"]!["/x"]!["post"]!["responses"]!["400"]!.AsObject();
        Assert.Equal("#/components/responses/ValidationError", resp400["$ref"]!.ToString());
    }

    [Fact]
    public void Patch_WhenJsonIsValidButNotObject_ReturnsOriginal()
    {
        var input = "[]";

        var output = SwaggerJsonRefPatcher.Patch(input);

        Assert.Equal(input, output);
    }
}
