using EventHouse.Management.Api.Contracts.ArtistPerformances;
using EventHouse.Management.Application.Commands.ArtistPerformances.Add;

namespace EventHouse.Management.Api.Mappers.ArtistPerformances;

/// <summary>
/// Maps the URL parameter and the request body into a single Application Command.
/// </summary>
internal static class CreateArtistPerformanceCommandMapper
{
    public static AddArtistPerformanceCommand FromContract(Guid eventVenueCalendarId, CreateArtistPerformanceRequest request)
    {
        return new AddArtistPerformanceCommand(
            eventVenueCalendarId,
            request.ArtistId,
            request.IsHeadliner,
            request.SetStart,
            request.SetEnd
        );
    }
}
