using EventHouse.Management.Application.Commands.ArtistPerformances.Remove;

namespace EventHouse.Management.Api.Mappers.ArtistPerformances;

internal static class RemoveArtistPerformanceCommandMapper
{
    public static RemoveArtistPerformanceCommand FromContract(Guid calendarId, Guid artistId)
        => new(calendarId, artistId);
}
