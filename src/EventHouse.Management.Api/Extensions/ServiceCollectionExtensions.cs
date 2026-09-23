using EventHouse.Management.Api.Swagger.Filters;
using Core.RateLimiting.DependencyInjection;
using Core.RateLimiting.Options;
using EventHouse.Management.Application.DependencyInjection;
using EventHouse.Management.Infrastructure.DependencyInjection;
using EventHouse.Management.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;

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
        services.AddTransportSecurity(configuration);
        services.AddCustomAuthentication(configuration);
        services.AddAuthorization();
        var rateLimitingSection = configuration.GetSection("Core:RateLimiting");
        var rateLimitingOptions = new RateLimitingOptions();

        rateLimitingSection.Bind(rateLimitingOptions);

        services.AddCoreRateLimiting(options => options.CopyFrom(rateLimitingOptions));

        // 3. Infrastructure and Persistence (DB, Cache)
        services.AddCustomHealthChecks();
        services.AddInfrastructure(configuration);

        // 4. Application Layer
        services.AddApplication();

        // 5. Documentation and Tools (Swagger)
        services.AddCustomSwagger();

        return services;
    }

    private static void AddTransportSecurity(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHsts(options =>
        {
            options.MaxAge = TimeSpan.FromDays(180);
            options.IncludeSubDomains = false;
            options.Preload = false;
        });

        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor |
                ForwardedHeaders.XForwardedProto;

            foreach (var value in configuration
                .GetSection("ReverseProxy:KnownProxies")
                .Get<string[]>() ?? [])
            {
                if (!IPAddress.TryParse(value, out var address))
                {
                    throw new InvalidOperationException(
                        $"ReverseProxy:KnownProxies contains an invalid IP address: '{value}'.");
                }

                options.KnownProxies.Add(address);
            }
        });
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
