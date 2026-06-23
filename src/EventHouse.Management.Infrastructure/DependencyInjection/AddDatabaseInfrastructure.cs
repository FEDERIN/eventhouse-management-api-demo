using EventHouse.Management.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace EventHouse.Management.Infrastructure.DependencyInjection;

internal static class DatabaseExtensions
{
    public static IServiceCollection AddDatabaseInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("ManagementConnection");
        services.AddDbContext<ManagementDbContext>(options =>
            options.UseNpgsql(connectionString, npgsqlOptions =>
                npgsqlOptions.MigrationsAssembly(typeof(ManagementDbContext).Assembly.FullName))
            .UseSnakeCaseNamingConvention());
        return services;
    }
}
