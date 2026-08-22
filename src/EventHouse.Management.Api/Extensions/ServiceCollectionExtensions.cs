using EventHouse.Management.Api.Common.Errors;
using EventHouse.Management.Api.Swagger.Filters;
using EventHouse.Management.Application.DependencyInjection;
using EventHouse.Management.Infrastructure.DependencyInjection;
using EventHouse.Management.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;

namespace EventHouse.Management.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Core / Framework (Controllers, JSON, etc)
        services.AddControllers(options =>
        {
            var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            options.Filters.Add(new AuthorizeFilter(policy));
        })
        .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        // 2. Security and Control (Auth, RateLimiter)
        services.AddCustomAuthentication(configuration);
        services.AddAuthorization();
        services.AddCustomRateLimiting(configuration);

        // 3. Infrastructure and Persistence (DB, Cache)
        services.AddCustomDbContext(configuration);
        services.AddCustomHealthChecks();
        services.AddInfrastructure(configuration);

        // 4. Application Layer
        services.AddApplication();

        // 5. Documentation and Tools (Swagger)
        services.AddCustomSwagger();

        return services;
    }

    private static void AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSecret = configuration["Auth:DevSecret"];
        if (string.IsNullOrWhiteSpace(jwtSecret))
        {
            throw new InvalidOperationException(
                "JWT secret is not configured. Please set the Auth__DevSecret environment variable.");
        }

        var issuer = configuration["Auth:Issuer"];
        var audience = configuration["Auth:Audience"];

        if (string.IsNullOrWhiteSpace(issuer) || string.IsNullOrWhiteSpace(audience))
        {
            throw new InvalidOperationException("Auth:Issuer/Auth:Audience not configured.");
        }

        services
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
    }

    private static void AddCustomRateLimiting(this IServiceCollection services, IConfiguration configuration)
    {
        var rlSection = configuration.GetSection("RateLimiting");
        var permitLimit = rlSection.GetValue<int>("PermitLimit", 60);
        var windowSeconds = rlSection.GetValue<int>("WindowSeconds", 60);
        var queueLimit = rlSection.GetValue<int>("QueueLimit", 0);

        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.OnRejected = async (context, token) =>
            {
                context.HttpContext.Response.ContentType = "application/problem+json";

                context.HttpContext.Response.Headers.RetryAfter = windowSeconds.ToString();

                var problem = new EventHouseProblemDetails
                {
                    Type = "urn:eventhouse:error:RATE_LIMIT_EXCEEDED",
                    Title = "Too Many Requests",
                    Status = StatusCodes.Status429TooManyRequests,
                    Detail = "Rate limit exceeded. Please retry later.",
                    Instance = context.HttpContext.Request.Path,
                    ErrorCode = "RATE_LIMIT_EXCEEDED",
                    TraceId = System.Diagnostics.Activity.Current?.Id ?? context.HttpContext.TraceIdentifier
                };

                await context.HttpContext.Response.WriteAsJsonAsync(problem, cancellationToken: token);
            };

            // Global Policy by IP
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
            {
                var key =
                    httpContext.User?.Identity?.IsAuthenticated == true
                        ? $"user:{httpContext.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? "auth"}"
                        : $"ip:{httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown"}";

                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: key,
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = permitLimit,
                        Window = TimeSpan.FromSeconds(windowSeconds),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = queueLimit
                    });
            });
        });
    }

    private static void AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ManagementDbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("ManagementConnection"))
                .UseSnakeCaseNamingConvention();
        });
    }

    private static void AddCustomHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddDbContextCheck<ManagementDbContext>("db");
    }

    private static void AddCustomSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
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
            c.AddServer(new OpenApiServer
            {
                Url = "https://eventhouse-management-api-demo.onrender.com",
                Description = "Render Production"
            });

            c.AddServer(new OpenApiServer { Url = "https://localhost:7232", Description = "Local SSL" });
            c.AddServer(new OpenApiServer { Url = "http://localhost:5185", Description = "Local" });

            c.SupportNonNullableReferenceTypes();
            c.EnableAnnotations();

            // XML documentation
            var basePath = AppContext.BaseDirectory;
            var apiXml = Path.Combine(basePath, "EventHouse.Management.Api.xml");
            if (File.Exists(apiXml))
            {
                c.IncludeXmlComments(apiXml, includeControllerXmlComments: true);
            }

            c.ExampleFilters();

            // Document filter to add Location header in 201 responses
            c.DocumentFilter<CreatedWithLocationDocumentFilter>();
            c.OperationFilter<JsonOnlyResponsesOperationFilter>();
            c.OperationFilter<IdempotencyHeaderOperationFilter>();
        });
    }
}