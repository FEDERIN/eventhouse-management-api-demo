using EventHouse.Management.Api.Contracts.EventVenueCalendars;
using EventHouse.Management.Application.Commands.EventVenueCalendars.Create;

namespace EventHouse.Management.Api.Mappers.EventVenueCalendars;

internal static class CreateEventVenueCalendarCommandMapper
{
    public static CreateEventVenueCalendarCommand FromContract(CreateEventVenueCalendarRequest request)
    {
        var endDate = request.EndDate
            ?? CalculateEndDate(request.StartDate, request.TimeZoneId);

        return new CreateEventVenueCalendarCommand(
            request.EventVenueId,
            request.SeatingMapId,
            request.StartDate,
            endDate,
            request.TimeZoneId
        );
    }

    private static DateTimeOffset CalculateEndDate(
        DateTimeOffset startDate,
        string timeZoneId)
    {
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

        var localStart = TimeZoneInfo.ConvertTime(
            startDate,
            timeZone);

        var localEndOfDay = new DateTimeOffset(
            localStart.Year,
            localStart.Month,
            localStart.Day,
            23,
            59,
            59,
            999,
            localStart.Offset);

        return localEndOfDay.ToUniversalTime();
    }
}
