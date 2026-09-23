using EventHouse.Management.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using Testcontainers.PostgreSql;

namespace EventHouse.Management.Api.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    protected virtual bool IsRateLimitingEnabled => false;
    protected virtual int RateLimitPermitLimit => 100;

    // Fix for CS0618: Pass the image directly to the builder
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder("postgres:16-alpine")
        .WithDatabase("eventhouse_management_tests")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    static CustomWebApplicationFactory()
    {
        Environment.SetEnvironmentVariable("Auth__DevSecret", "EVENTHOUSE_TEST_SECRET_12345678901234567890");
        Environment.SetEnvironmentVariable("Auth__Issuer", "eventhouse.local");
        Environment.SetEnvironmentVariable("Auth__Audience", "eventhouse.management");
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Core:Idempotency:Enabled"] = "true",
                ["Core:Idempotency:Provider"] = "PostgreSql",
                ["PostgreSqlConnections:MainPostgreSql:ConnectionString"] =
                    _dbContainer.GetConnectionString(),

                ["Core:RateLimiting:Enabled"] = "false"
            });
        });

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ManagementDbContext>));

            if (descriptor != null) services.Remove(descriptor);

            services.AddDbContext<ManagementDbContext>(options =>
            {
                options.UseNpgsql(_dbContainer.GetConnectionString())
                       .UseSnakeCaseNamingConvention();
            });

            services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            });

            services.PostConfigure<Microsoft.AspNetCore.RateLimiting.RateLimiterOptions>(options =>
            {
                options.GlobalLimiter = null;
            });
        });
    }

    public async ValueTask InitializeAsync()
    {
        // This will now work as soon as Docker is running
        await _dbContainer.StartAsync();

        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ManagementDbContext>();
        await db.Database.MigrateAsync();

        // The idempotency PostgreSQL provider persists outside ManagementDbContext,
        // so its table is not covered by the application's EF Core migrations.
        await db.Database.ExecuteSqlRawAsync(
            """
            CREATE TABLE IF NOT EXISTS idempotency_keys (
                key text PRIMARY KEY,
                request_fingerprint text NULL,
                hash_algorithm text NULL,
                status_code integer NOT NULL,
                content_type text NULL,
                headers bytea NULL,
                body bytea NULL,
                expires_at timestamp with time zone NOT NULL
            );
            """);
    }

    public new async ValueTask DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await base.DisposeAsync();

        GC.SuppressFinalize(this);
    }
}
