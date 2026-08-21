using EventHouse.Management.Api.Contracts.ArtistPerformances;
using EventHouse.Management.Application.Commands.ArtistPerformances.UpdateDate;

namespace EventHouse.Management.Api.Mappers.EventVenueCalendars;

internal static class UpdatePerformanceDatesCommandMapper
{
    public static UpdatePerformanceDatesCommand FromContract(Guid eventVenueCalendarId,
    Guid artistId, UpdatePerformanceDatesRequest request)
        => new(
            eventVenueCalendarId,
            artistId,
            request.SetStart,
            request.SetEnd);
}
