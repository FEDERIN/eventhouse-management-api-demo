using EventHouse.Management.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EventHouse.Management.Api.Tests;

public sealed class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _dbPath;

    public CustomWebApplicationFactory()
    {
        // Environment
        Environment.SetEnvironmentVariable("Auth__DevSecret", "EVENTHOUSE_TEST_SECRET_12345678901234567890");
        Environment.SetEnvironmentVariable("Auth__Issuer", "eventhouse.local");
        Environment.SetEnvironmentVariable("Auth__Audience", "eventhouse.management");

        // Ensure deterministic absolute path for SQLite
        var dataDir = Path.Combine(AppContext.BaseDirectory, "Data");
        Directory.CreateDirectory(dataDir);

        _dbPath = Path.Combine(dataDir, $"management.tests.{Guid.NewGuid():N}.db");
        Environment.SetEnvironmentVariable(
            "ConnectionStrings__ManagementConnection",
            $"Data Source={_dbPath}"
        );
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);

        // Apply migrations for test database
        using var scope = host.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ManagementDbContext>();
        db.Database.Migrate();

        return host;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        // Cleanup test database file
        try
        {
            if (File.Exists(_dbPath))
                File.Delete(_dbPath);
        }
        catch
        {
            // ignore cleanup errors
        }
    }
}
