using EventHouse.Management.Api.Contracts.ArtistPerformances;
using EventHouse.Management.Application.Commands.ArtistPerformances.Add;

namespace EventHouse.Management.Api.Mappers.ArtistPerformances;

internal static class AddArtistPerformanceCommandMapper
{
    public static AddArtistPerformanceCommand FromContract(Guid eventVenueCalendarId, CreateArtistPerformanceRequest request)
        => new(
            eventVenueCalendarId,
            request.ArtistId,
            request.IsHeadliner,
            request.SetStart,
            request.SetEnd
            );
}
