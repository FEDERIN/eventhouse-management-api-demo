using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EventHouse.Management.Api.Swagger.Filters;

public sealed class CreatedWithLocationDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        foreach (var path in swaggerDoc.Paths.Values)
        {
            foreach (var operation in path.Operations.Values)
            {
                if (operation.Responses.TryGetValue("201", out var response))
                {
                    response.Headers ??= new Dictionary<string, OpenApiHeader>();

                    response.Headers.TryAdd("Location", new OpenApiHeader
                    {
                        Description = "URL of the newly created resource",
                        Schema = new OpenApiSchema
                        {
                            Type = "string",
                            Format = "uri"
                        }
                    });
                }
            }
        }
    }
}
