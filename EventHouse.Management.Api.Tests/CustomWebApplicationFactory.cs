using EventHouse.Management.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EventHouse.Management.Api.Tests;

public sealed class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private static readonly string DbPath;
    private static bool _initialized;
    private static readonly object InitLock = new();

    static CustomWebApplicationFactory()
    {
        // Auth env for tests
        Environment.SetEnvironmentVariable("Auth__DevSecret", "EVENTHOUSE_TEST_SECRET_12345678901234567890");
        Environment.SetEnvironmentVariable("Auth__Issuer", "eventhouse.local");
        Environment.SetEnvironmentVariable("Auth__Audience", "eventhouse.management");

        // One deterministic DB per test run
        var dataDir = Path.Combine(AppContext.BaseDirectory, "Data");
        Directory.CreateDirectory(dataDir);

        DbPath = Path.Combine(dataDir, "management.tests.run.db");

        // Pooling off helps avoid file-lock weirdness in some environments
        Environment.SetEnvironmentVariable(
            "ConnectionStrings__ManagementConnection",
            $"Data Source={DbPath};Pooling=False"
        );
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);

        // ✅ Clean DB once per run + migrate once (thread-safe)
        lock (InitLock)
        {
            if (!_initialized)
            {
                using var scope = host.Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ManagementDbContext>();

                // Start clean every run
                db.Database.EnsureDeleted();
                db.Database.Migrate();

                _initialized = true;
            }
        }

        return host;
    }
}
