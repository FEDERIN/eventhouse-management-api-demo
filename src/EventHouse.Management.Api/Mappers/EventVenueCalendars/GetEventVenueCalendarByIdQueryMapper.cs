using EventHouse.Management.Application.Queries.EventVenueCalendars.GetById;

namespace EventHouse.Management.Api.Mappers.EventVenueCalendars;

internal static class GetEventVenueCalendarByIdQueryMapper
{
    public static GetEventVenueCalendarByIdQuery FromContract(Guid eventVenueCalendarId)
        => new(eventVenueCalendarId);
}
