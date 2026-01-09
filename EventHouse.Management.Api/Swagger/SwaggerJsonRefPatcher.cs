using System.Text.Json;
using System.Text.Json.Nodes;

namespace EventHouse.Management.Api.Swagger;

public static class SwaggerJsonRefPatcher
{
    public static string Patch(string swaggerJson)
    {
        if (JsonNode.Parse(swaggerJson) is not JsonObject root) return swaggerJson;

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

        // si ya existen, no los pisan
        responses.TryAdd("Unauthorized", ProblemResponse("Unauthorized", "EventHouseProblemDetails"));
        responses.TryAdd("Forbidden", ProblemResponse("Forbidden", "EventHouseProblemDetails"));
        responses.TryAdd("InternalError", ProblemResponse("Internal Server Error", "EventHouseProblemDetails"));

        // opcional (si querés extender):
        responses.TryAdd("NotFound", ProblemResponse("Not Found", "EventHouseProblemDetails"));
        responses.TryAdd("Conflict", ProblemResponse("Conflict", "EventHouseProblemDetails"));
        responses.TryAdd("ValidationError", ProblemResponse("Validation error", "ValidationProblemDetails"));
        responses.TryAdd("TooManyRequests", TooManyRequestsResponse("EventHouseProblemDetails"));
    }

    private static JsonObject ProblemResponse(string description, string schemaId)
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
                    }
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

    private static JsonObject TooManyRequestsResponse(string schemaId)
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
                }
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

        // ✅ Caso 1: ya viene como problem+json
        if (content.ContainsKey("application/problem+json"))
        {
            responses["400"] = new JsonObject { ["$ref"] = $"#/components/responses/{componentId}" };
            return;
        }

        // ✅ Caso 2: Swashbuckle lo pone como application/json pero con ValidationProblemDetails
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
}
