using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Infrastructure.Errors;
using EventHouse.Management.Infrastructure.Repositories;
using EventHouse.Management.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Core.Idempotency;

namespace EventHouse.Management.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        // 1. Connection String
        var connectionString = config.GetConnectionString("ManagementConnection");

        // 2. DbContext
        services.AddDbContext<ManagementDbContext>(options =>
            options.UseNpgsql(connectionString, npgsqlOptions =>
                npgsqlOptions.MigrationsAssembly(typeof(ManagementDbContext).Assembly.FullName))
            .UseSnakeCaseNamingConvention());

        var provider = config["Idempotency:Provider"];
        var enabled = config["Idempotency:Enabled"];
        Console.WriteLine($"🔍 DEBUG IDEMPOTENCY: Provider={provider}, Enabled={enabled}");

        // 3. Idempotency - Registering your generic provider
        services.AddIdempotencyProvider(config);

        // 4. Repositories
        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<IArtistRepository, ArtistRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IVenueRepository, VenueRepository>();
        services.AddScoped<ISeatingMapRepository, SeatingMapRepository>();
        services.AddScoped<IEventVenueRepository, EventVenueRepository>();
        services.AddScoped<IEventVenueCalendarRepository, EventVenueCalendarRepository>();
        services.AddScoped<IArtistPerformanceRepository, ArtistPerformanceRepository>();

        // 5. Mappers
        services.AddSingleton<IExceptionMapper, ExceptionMapper>();

        return services;
    }
}