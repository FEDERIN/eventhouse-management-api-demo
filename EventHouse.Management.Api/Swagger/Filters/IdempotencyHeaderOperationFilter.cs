using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Any;
using Core.Idempotency.Options;

namespace EventHouse.Management.Api.Swagger.Filters;

public class IdempotencyHeaderOperationFilter(IConfiguration configuration) : IOperationFilter
{
    private readonly IConfiguration _configuration = configuration;

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var section = _configuration.GetSection("Idempotency");
        var options = section.Get<IdempotencyOptions>();

        if (options == null || !options.Enabled) return;

        var currentMethod = context.ApiDescription.HttpMethod?.ToUpper();

        if (options.AllowedMethods == null || !options.AllowedMethods.Contains(currentMethod))
            return;

        operation.Parameters ??= [];

        var existingHeader = operation.Parameters.FirstOrDefault(p =>
        p.Name.Equals(options.HeaderName ?? "X-Idempotency-Key", StringComparison.OrdinalIgnoreCase));

        if (existingHeader != null) return;

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = options.HeaderName ?? "X-Idempotency-Key",
            In = ParameterLocation.Header,
            Required = false,
            Schema = new OpenApiSchema
            {
                Type = "string",
                Format = "uuid",
                Default = new OpenApiString(Guid.NewGuid().ToString())
            },
            Description = $"Idempotency header configured. Allowed methods: {string.Join(", ", options.AllowedMethods)}"
        });
    }
}