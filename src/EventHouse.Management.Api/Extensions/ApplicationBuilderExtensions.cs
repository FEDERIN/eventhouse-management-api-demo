using Core.Cache.DependencyInjection;
using Core.Idempotency.DependencyInjection;
using Core.RateLimiting.DependencyInjection;
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
        // Must precede HTTPS redirection so a trusted proxy can communicate
        // the original client scheme through X-Forwarded-Proto.
        app.UseForwardedHeaders();
        app.UseMiddleware<SecurityHeadersMiddleware>();

        // HTTPS is enforced in every runtime environment except tests. HSTS is
        // intentionally limited to non-development environments because it is
        // persisted by browsers and can make local HTTP development unusable.
        if (!env.IsEnvironment("Testing"))
        {
            app.UseHttpsRedirection();

            if (!env.IsDevelopment())
            {
                app.UseHsts();
            }
        }

        // 2. Correlation middleware and data infrastructure
        app.UseMiddleware<CorrelationIdMiddleware>();
        app.UseCoreIdempotency();
        app.UseCoreCache();

        // 3. Access security
        app.UseAuthentication();
        app.UseCoreRateLimiting();
        app.UseAuthorization();

        return app;
    }

}
