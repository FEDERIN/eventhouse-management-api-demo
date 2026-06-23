using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace EventHouse.Management.Infrastructure.DependencyInjection;

internal static class RepositoryExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        #region AddScoped
        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<IArtistRepository, ArtistRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IVenueRepository, VenueRepository>();
        services.AddScoped<ISeatingMapRepository, SeatingMapRepository>();
        services.AddScoped<IEventVenueRepository, EventVenueRepository>();
        services.AddScoped<IEventVenueCalendarRepository, EventVenueCalendarRepository>();
        services.AddScoped<IArtistPerformanceRepository, ArtistPerformanceRepository>();
        #endregion
        return services;
    }
}