using EventHouse.Management.Api.Contracts.EventVenueCalendars;
using EventHouse.Management.Application.Commands.EventVenueCalendars.Create;

namespace EventHouse.Management.Api.Mappers.EventVenueCalendars;

internal static class CreateEventVenueCalendarCommandMapper
{
    public static CreateEventVenueCalendarCommand FromContract(CreateEventVenueCalendarRequest request)
    {
        return new CreateEventVenueCalendarCommand(
            request.EventVenueId,
            request.SeatingMapId,
            request.StartDate,
            request.EndDate,
            request.TimeZoneId,
            EventVenueCalendarStatusMapper.ToApplicationRequired(request.Status)
        );
    }
}