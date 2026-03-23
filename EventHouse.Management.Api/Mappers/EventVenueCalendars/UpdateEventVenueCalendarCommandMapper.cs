using EventHouse.Management.Api.Contracts.EventVenueCalendars;
using EventHouse.Management.Application.Commands.EventVenueCalendars.Update;

namespace EventHouse.Management.Api.Mappers.EventVenueCalendars;

internal static class UpdateEventVenueCalendarCommandMapper
{
    public static UpdateEventVenueCalendarCommand FromContract(
        Guid eventVenueCalendarId,
        UpdateEventVenueCalendarRequest request)
    {
        return new UpdateEventVenueCalendarCommand(
            eventVenueCalendarId,
            request.StartDate,
            request.EndDate,
            EventVenueCalendarStatusMapper.ToApplicationRequired(request.Status)
        );
    }
}