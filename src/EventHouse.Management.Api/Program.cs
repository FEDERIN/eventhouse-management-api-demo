using Core.Http.ProblemDetails.DependencyInjection;
using Core.Idempotency.Exceptions;
using Core.Observability;
using DotNetEnv;
using EventHouse.Management.Api.ErrorHandling;
using EventHouse.Management.Api.Extensions;
using EventHouse.Management.Api.Middlewares;
using EventHouse.Management.Infrastructure.DependencyInjection;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

if (!builder.Environment.IsEnvironment("Testing"))
{
    var environmentFile = Path.Combine(builder.Environment.ContentRootPath, ".env");
    if (File.Exists(environmentFile))
    {
        Env.Load(environmentFile);
    }

    builder.Configuration.AddEnvironmentVariables();
}

// You can get these from builder.Configuration or set them manually
string environment = builder.Environment.EnvironmentName;
string serviceName = "EventHouse.Management.Api";
string serviceNamespace = "EventHouse.Management";

builder.AddInfrastructureObservability(environment, serviceName, serviceNamespace);

builder.Services.AddApiServices(builder.Configuration);
builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();
builder.Services.AddTransient<CorrelationIdMiddleware>();
builder.Services.AddTransient<SecurityHeadersMiddleware>();

builder.Services.AddCoreProblemDetails(options =>
{
    options.CustomizeProblemDetails = static (problem, context, exception) =>
    {
        if (exception is IdempotencyFingerprintMismatchException)
        {
            problem.Extensions["idempotencyKey"] =
                context.Request.Headers["Idempotency-Key"].ToString();
        }
    };
});
builder.Services.AddCoreExceptionHandler<EventHouseExceptionProblemMapper>();

var app = builder.Build();

app.UseExceptionHandler();
app.UseObservabilityEndpoints();
app.UseCustomSwagger();
app.UseInfrastructurePipeline(app.Environment);
app.MapControllers();
app.Run();

public partial class Program { }
