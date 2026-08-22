using Core.Observability;
using DotNetEnv;
using EventHouse.Management.Api.Extensions;
using EventHouse.Management.Api.Middlewares;
using EventHouse.Management.Infrastructure.DependencyInjection;
using Swashbuckle.AspNetCore.Filters;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// You can get these from builder.Configuration or set them manually
string environment = builder.Environment.EnvironmentName;
string serviceName = "EventHouse.Management.Api";
string serviceNamespace = "EventHouse.Management";

builder.AddInfrastructureObservability(environment, serviceName, serviceNamespace);

builder.Services.AddApiServices(builder.Configuration);
builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();
builder.Services.AddTransient<CorrelationIdMiddleware>();

builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();
app.UseObservabilityEndpoints();
app.UseCustomSwagger();
app.UseInfrastructurePipeline(app.Environment);
app.MapControllers();
app.Run();

public partial class Program { }
