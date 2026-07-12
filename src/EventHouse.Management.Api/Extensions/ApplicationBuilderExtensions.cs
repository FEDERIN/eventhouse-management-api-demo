using Core.Cache.DependencyInjection;
using Core.Idempotency;
using EventHouse.Management.Api.Middlewares;
using EventHouse.Management.Api.Swagger;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Extensions;
using Swashbuckle.AspNetCore.Swagger;

namespace EventHouse.Management.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static WebApplication UseCustomSwagger(this WebApplication app)
    {
        // 1. Setup original JSON generation
        app.UseSwagger(c =>
        {
            c.RouteTemplate = "swagger-original/{documentName}/swagger.json";
        });

        // 2. Map the patched Swagger JSON
        app.MapGet("/swagger/v1/swagger.json", async (ISwaggerProvider swaggerProvider, HttpContext http) =>
        {
            var doc = swaggerProvider.GetSwagger("v1");
            var json = doc.SerializeAsJson(OpenApiSpecVersion.OpenApi3_0);
            var patched = SwaggerJsonRefPatcher.Patch(json);

            http.Response.ContentType = "application/json";
            await http.Response.WriteAsync(patched);
        })
        .DisableRateLimiting()
        .ExcludeFromDescription();

        // 3. Setup Swagger UI
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "EventHouse.Management.Api v1");
            c.RoutePrefix = "swagger";
        });

        return app;
    }

    public static IApplicationBuilder UseInfrastructurePipeline(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        // 1. Global error handling
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        // 2. Basic security and redirections
        if (env.IsDevelopment())
        {
            app.UseHttpsRedirection();
        }

        // 3. Correlation middleware and data infrastructure
        app.UseMiddleware<CorrelationIdMiddleware>();
        app.UseIdempotency();
        app.UseCoreCache();

        // 4. Access security
        app.UseAuthentication();
        app.UseRateLimiter();
        app.UseAuthorization();

        return app;
    }

}