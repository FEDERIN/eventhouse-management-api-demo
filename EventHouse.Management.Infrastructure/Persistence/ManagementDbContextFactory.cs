using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EventHouse.Management.Infrastructure.Persistence;

public sealed class ManagementDbContextFactory : IDesignTimeDbContextFactory<ManagementDbContext>
{
    public ManagementDbContext CreateDbContext(string[] args)
    {
        var cwd = Directory.GetCurrentDirectory();

        // Probamos appsettings en el directorio actual y en la carpeta del API (si estás parado en la raíz).
        var candidates = new[]
        {
            Path.Combine(cwd, "appsettings.json"),
            Path.Combine(cwd, "EventHouse.Management.Api", "appsettings.json"),
            Path.Combine(cwd, "appsettings.Development.json"),
            Path.Combine(cwd, "EventHouse.Management.Api", "appsettings.Development.json"),
        };

        var cb = new ConfigurationBuilder();

        foreach (var file in candidates)
        {
            if (File.Exists(file))
                cb.AddJsonFile(file, optional: false);
        }

        cb.AddEnvironmentVariables();
        var config = cb.Build();

        // Fallback seguro (demo)
        var cs = config.GetConnectionString("ManagementConnection")
                 ?? "Data Source=./Data/management.db";

        var options = new DbContextOptionsBuilder<ManagementDbContext>()
            .UseSqlite(cs)
            .Options;

        return new ManagementDbContext(options);
    }
}
