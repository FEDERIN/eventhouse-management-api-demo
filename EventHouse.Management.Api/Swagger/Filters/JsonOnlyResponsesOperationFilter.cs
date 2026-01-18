using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EventHouse.Management.Api.Swagger.Filters;

public sealed class JsonOnlyResponsesOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        foreach (var response in operation.Responses.Values)
        {
            if (response.Content is null || response.Content.Count == 0)
                continue;

            if (response.Content.ContainsKey("application/json"))
            {
                response.Content.Remove("text/plain");
                response.Content.Remove("text/json");
            }
        }
    }
}
