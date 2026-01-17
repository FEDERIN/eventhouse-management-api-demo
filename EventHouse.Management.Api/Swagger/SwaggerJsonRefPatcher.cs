using System.Text.Json;
using System.Text.Json.Nodes;

namespace EventHouse.Management.Api.Swagger;

public static class SwaggerJsonRefPatcher
{
    public static string Patch(string swaggerJson)
    {
        JsonNode? parsed;
        try
        {
            parsed = JsonNode.Parse(swaggerJson);
        }
        catch (JsonException)
        {
            return swaggerJson; // no rompe si viene basura
        }

        if (parsed is not JsonObject root) return swaggerJson;

        EnsureCommonResponses(root);
        ReplaceResponsesWithRefs(root);

        return root.ToJsonString(new JsonSerializerOptions { WriteIndented = true });
    }

    private static void EnsureCommonResponses(JsonObject root)
    {
        var components = root["components"] as JsonObject ?? [];
        root["components"] = components;

        var responses = components["responses"] as JsonObject ?? [];
        components["responses"] = responses;

        responses.TryAdd("Unauthorized", ProblemResponse(
            "Unauthorized", 
            "EventHouseProblemDetails",
            ProblemExample(
                "urn:eventhouse:error:UNAUTHORIZED",
                "Unauthorized",
                401,
                "A valid JWT access token is required.",
                "UNAUTHORIZED")));


        responses.TryAdd("Forbidden", ProblemResponse(
            "Forbidden", 
            "EventHouseProblemDetails",
            ProblemExample(
                "urn:eventhouse:error:FORBIDDEN",
                "Forbidden",
                403,
                "You do not have permission to access this resource.",
                "FORBIDDEN")));

        responses.TryAdd("InternalError", ProblemResponse(
            "Internal Server Error",
            "EventHouseProblemDetails",
            ProblemExample(
                "urn:eventhouse:error:INTERNAL_ERROR",
                "Internal Server Error",
                500,
                "An unexpected error occurred.",
                "INTERNAL_ERROR")));

        responses.TryAdd("NotFound", ProblemResponse(
            "Not Found",
            "EventHouseProblemDetails",
            ProblemExample(
                "urn:eventhouse:error:NOT_FOUND",
                "Not Found",
                404,
                "The requested resource was not found.",
                "NOT_FOUND")));

        responses.TryAdd("Conflict", ProblemResponse(
            "Conflict",
            "EventHouseProblemDetails",
            ProblemExample(
                "urn:eventhouse:error:CONFLICT",
                "Conflict",
                409,
                "The request conflicts with the current state of the resource.",
                "CONFLICT")));


        responses.TryAdd("ValidationError", new JsonObject
        {
            ["description"] = "Validation error",
            ["content"] = new JsonObject
            {
                ["application/problem+json"] = new JsonObject
                {
                    ["schema"] = new JsonObject
                    {
                        ["$ref"] = "#/components/schemas/ValidationProblemDetails"
                    },
                    ["example"] = new JsonObject
                    {
                        ["type"] = "urn:eventhouse:error:VALIDATION_ERROR",
                        ["title"] = "Validation error",
                        ["status"] = 400,
                        ["detail"] = "One or more validation errors occurred.",
                        ["errors"] = new JsonObject
                        {
                            ["name"] = new JsonArray { "The Name field is required." }
                        },
                        ["traceId"] = "00-aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa-bbbbbbbbbbbbbbbb-01"
                    }
                }
            }
        });

        responses.TryAdd(
            "TooManyRequests",
            TooManyRequestsResponse(
                "EventHouseProblemDetails",
                ProblemExample(
                    "urn:eventhouse:error:TOO_MANY_REQUESTS",
                    "Too Many Requests",
                    429,
                    "Rate limit exceeded. Please retry later.",
                    "TOO_MANY_REQUESTS"
                )
            )
        );

    }

    private static JsonObject ProblemResponse(string description, string schemaId, JsonObject? example = null)
        => new()
        {
            ["description"] = description,
            ["content"] = new JsonObject
            {
                ["application/problem+json"] = new JsonObject
                {
                    ["schema"] = new JsonObject
                    {
                        ["$ref"] = $"#/components/schemas/{schemaId}"
                    },
                    ["example"] = example
                }
            }
        };

    private static void ReplaceResponsesWithRefs(JsonObject root)
    {
        if (root["paths"] is not JsonObject paths) return;

        foreach (var path in paths)
        {
            if (path.Value is not JsonObject pathItem) continue;

            foreach (var op in pathItem)
            {
                if (op.Value is not JsonObject operation) continue;
                if (operation["responses"] is not JsonObject responses) continue;

                ReplaceIfExists(responses, "401", "Unauthorized");
                ReplaceIfExists(responses, "403", "Forbidden");
                ReplaceIfExists(responses, "500", "InternalError");

                // opcional:
                ReplaceIfExists(responses, "404", "NotFound");
                ReplaceIfExists(responses, "409", "Conflict");
                Replace400IfProblemDetails(responses, "ValidationError");
                ReplaceIfExists(responses, "429", "TooManyRequests");

            }
        }
    }

    private static void ReplaceIfExists(JsonObject responses, string statusCode, string componentId)
    {
        if (!responses.ContainsKey(statusCode)) return;

        responses[statusCode] = new JsonObject
        {
            ["$ref"] = $"#/components/responses/{componentId}"
        };
    }

    private static JsonObject TooManyRequestsResponse(string schemaId, JsonObject? example = null)
    => new()
    {
        ["description"] = "Too Many Requests",
        ["headers"] = new JsonObject
        {
            ["Retry-After"] = new JsonObject
            {
                ["description"] = "Seconds to wait before retrying the request.",
                ["schema"] = new JsonObject { ["type"] = "integer", ["format"] = "int32" }
            }
        },
        ["content"] = new JsonObject
        {
            ["application/problem+json"] = new JsonObject
            {
                ["schema"] = new JsonObject
                {
                    ["$ref"] = $"#/components/schemas/{schemaId}"
                },
                ["example"] = example
            }
        }
    };

    private static void Replace400IfProblemDetails(JsonObject responses, string componentId)
    {
        if (!responses.TryGetPropertyValue("400", out var node) || node is not JsonObject resp400)
            return;

        // Si ya es $ref, no tocar
        if (resp400.ContainsKey("$ref"))
            return;

        // Si no tiene content -> lo reemplazamos
        if (!resp400.TryGetPropertyValue("content", out var contentNode) || contentNode is not JsonObject content)
        {
            responses["400"] = new JsonObject { ["$ref"] = $"#/components/responses/{componentId}" };
            return;
        }

        // Caso 1: ya viene como problem+json
        if (content.ContainsKey("application/problem+json"))
        {
            responses["400"] = new JsonObject { ["$ref"] = $"#/components/responses/{componentId}" };
            return;
        }

        // Caso 2: Swashbuckle lo pone como application/json pero con ValidationProblemDetails
        if (content.TryGetPropertyValue("application/json", out var appJsonNode) &&
            appJsonNode is JsonObject appJson &&
            appJson.TryGetPropertyValue("schema", out var schemaNode) &&
            schemaNode is JsonObject schema &&
            schema.TryGetPropertyValue("$ref", out var refNode) &&
            refNode is JsonValue refValue &&
            refValue.ToString().EndsWith("/ValidationProblemDetails", StringComparison.Ordinal))
        {
            responses["400"] = new JsonObject { ["$ref"] = $"#/components/responses/{componentId}" };
        }
    }

    private static JsonObject ProblemExample(string type, string title, int status, string detail, string errorCode)
        => new()
        {
            ["type"] = type,
            ["title"] = title,
            ["status"] = status,
            ["detail"] = detail,
            ["errorCode"] = errorCode,
            ["traceId"] = "00-aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa-bbbbbbbbbbbbbbbb-01"
        };
}
