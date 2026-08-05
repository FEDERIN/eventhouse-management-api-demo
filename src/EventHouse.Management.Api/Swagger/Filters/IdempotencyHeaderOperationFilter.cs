using Core.Idempotency.Options;
using Dapper;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EventHouse.Management.Api.Swagger.Filters;

public class IdempotencyHeaderOperationFilter(IConfiguration configuration) : IOperationFilter
{
    private readonly IConfiguration _configuration = configuration;

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var section = _configuration.GetSection("Core:Idempotency");
        var options = new IdempotencyOptions();
        section.Bind(options);

        options.AllowedMethods.Clear();
        var values = section.GetSection("AllowedMethods").Get<string[]>();

        if (values is null)
            return;

        options.AddAllowedMethods(values);

        if (options == null || !options.Enabled) return;

        var currentMethod = context.ApiDescription.HttpMethod?.ToUpper();

        if (options.AllowedMethods == null || string.IsNullOrEmpty(currentMethod) 
            || !options.AllowedMethods.Contains(currentMethod))
            return;

        operation.Parameters ??= [];

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "Idempotency-Key",
            In = ParameterLocation.Header,
            Required = false,
            Schema = new OpenApiSchema
            {
                Type = "string",
                Format = "uuid",
                Default = new OpenApiString(Guid.NewGuid().ToString())
            },
            Description = $"Idempotency header configured. Allowed methods: {string.Join(", ", options.AllowedMethods.Distinct().AsList())}"
        });
    }
}