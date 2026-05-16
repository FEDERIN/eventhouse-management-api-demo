using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Infrastructure.Errors;
using EventHouse.Management.Infrastructure.Repositories;
using EventHouse.Management.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventHouse.Management.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        // 1. Connection String
        var connectionString = config.GetConnectionString("DefaultConnection");

        // 2. DbContext
        services.AddDbContext<ManagementDbContext>(options =>
            options.UseNpgsql(connectionString, npgsqlOptions =>
                npgsqlOptions.MigrationsAssembly(typeof(ManagementDbContext).Assembly.FullName)));

        // 3. Repositories
        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<IArtistRepository, ArtistRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IVenueRepository, VenueRepository>();
        services.AddScoped<ISeatingMapRepository, SeatingMapRepository>();
        services.AddScoped<IEventVenueRepository, EventVenueRepository>();
        services.AddScoped<IEventVenueCalendarRepository, EventVenueCalendarRepository>();
        services.AddScoped<IArtistPerformanceRepository, ArtistPerformanceRepository>();

        // 4. Mappers
        services.AddSingleton<IExceptionMapper, ExceptionMapper>();

        return services;
    }
}