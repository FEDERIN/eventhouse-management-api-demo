using EventHouse.Management.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace EventHouse.Management.Api.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
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
        builder.UseEnvironment("Development");

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
    }

    public new async ValueTask DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await base.DisposeAsync();

        // Fix for CA1816: Properly handle the garbage collector
        GC.SuppressFinalize(this);
    }
}