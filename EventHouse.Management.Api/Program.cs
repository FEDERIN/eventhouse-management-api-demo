using EventHouse.Management.Api.Middlewares;
using EventHouse.Management.Api.Swagger;
using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.DependencyInjection;
using EventHouse.Management.Infrastructure.Persistence;
using EventHouse.Management.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

// para HealthCheckOptions + HealthReport
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

//
// Controllers + JSON
//
builder.Services.AddControllers(options =>
{
    // Auth por defecto
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    options.Filters.Add(new AuthorizeFilter(policy));
})
.AddJsonOptions(options =>
{
    // Enums como string en JSON
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

//
// Repositories
//
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IArtistRepository, ArtistRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IVenueRepository, VenueRepository>();

//
// Auth JWT (env var: Auth__DevSecret)
//
var jwtSecret = builder.Configuration["Auth:DevSecret"];
if (string.IsNullOrWhiteSpace(jwtSecret))
{
    throw new InvalidOperationException(
        "JWT secret is not configured. Please set the Auth__DevSecret environment variable.");
}

var issuer = builder.Configuration["Auth:Issuer"];
var audience = builder.Configuration["Auth:Audience"];

if (string.IsNullOrWhiteSpace(issuer) || string.IsNullOrWhiteSpace(audience))
{
    throw new InvalidOperationException("Auth:Issuer/Auth:Audience not configured.");
}

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,

            ValidateAudience = true,
            ValidAudience = audience,

            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
        };
    });

builder.Services.AddAuthorization();

//
// DbContext
//
builder.Services.AddDbContext<ManagementDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("ManagementConnection"));

    if (builder.Environment.IsDevelopment())
    {
        options.EnableDetailedErrors();
        options.EnableSensitiveDataLogging();
        options.LogTo(Console.WriteLine);
    }
});

//
// Health checks (cloud readiness)
//
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ManagementDbContext>("db");

builder.Services.AddApplication();

//
// Swagger
//
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "EventHouse.Management.Api",
        Version = "v1"
    });

    c.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header. Example: Bearer {token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // Servers placeholders
    c.AddServer(new OpenApiServer { Url = "http://localhost:5185", Description = "Local" });
    c.AddServer(new OpenApiServer { Url = "https://staging.api.tu-dominio.com", Description = "Staging" });
    c.AddServer(new OpenApiServer { Url = "https://api.tu-dominio.com", Description = "Production" });

    c.EnableAnnotations();

    // XML docs - API
    var basePath = AppContext.BaseDirectory;
    var apiXml = Path.Combine(basePath, "EventHouse.Management.Api.xml");
    if (File.Exists(apiXml))
    {
        c.IncludeXmlComments(apiXml, includeControllerXmlComments: true);
    }

    // Document filter para agregar header Location en respuestas 201
    c.DocumentFilter<CreatedWithLocationDocumentFilter>();
});

builder.Services.AddTransient<CorrelationIdMiddleware>();

var app = builder.Build();

//
// Health response writer (JSON)
//
static Task WriteHealthResponse(HttpContext context, HealthReport report)
{
    context.Response.ContentType = "application/json; charset=utf-8";

    var payload = new
    {
        status = report.Status.ToString(),
        totalDurationMs = report.TotalDuration.TotalMilliseconds,
        checks = report.Entries.Select(e => new
        {
            name = e.Key,
            status = e.Value.Status.ToString(),
            durationMs = e.Value.Duration.TotalMilliseconds,
            description = e.Value.Description
        })
    };

    return context.Response.WriteAsync(JsonSerializer.Serialize(payload));
}

//
// Pipeline
//
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        c.RouteTemplate = "swagger/{documentName}/swagger.json";
    });

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "EventHouse.Management.Api v1");
        c.RoutePrefix = "swagger";
    });
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

// Health endpoints (liveness / readiness)
app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => false, // no checks, solo "Alive"
    ResponseWriter = async (ctx, _) =>
    {
        ctx.Response.ContentType = "application/json; charset=utf-8";
        await ctx.Response.WriteAsync("""{"status":"Healthy"}""");
    }
});

app.MapHealthChecks("/ready", new HealthCheckOptions
{
    Predicate = _ => true, // incluye db
    ResponseWriter = WriteHealthResponse
});


app.MapControllers();

app.Run();

// Para tests de integración (opcional, pero útil)
public partial class Program { }
