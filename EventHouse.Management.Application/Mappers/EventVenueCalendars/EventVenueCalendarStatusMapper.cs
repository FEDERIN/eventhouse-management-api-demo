
using EventHouse.Management.Application.Common.Enums;
using EventHouse.Management.Domain.Enums;

namespace EventHouse.Management.Application.Mappers.EventVenueCalendars;

public static class EventVenueCalendarStatusMapper
{
    public static EventVenueCalendarStatus ToDomainRequired(EventVenueCalendarStatusDto status) 
        => ToDomain(status);

    public static EventVenueCalendarStatus? ToDomainOptional(EventVenueCalendarStatusDto? status)
        => status.HasValue ? ToDomain(status.Value) : null;

    public static EventVenueCalendarStatusDto ToApplicationRequired(this EventVenueCalendarStatus status) =>
        status switch
        {
            EventVenueCalendarStatus.Draft => EventVenueCalendarStatusDto.Draft,
            EventVenueCalendarStatus.Published => EventVenueCalendarStatusDto.Published,
            EventVenueCalendarStatus.Cancelled => EventVenueCalendarStatusDto.Cancelled,
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, "Invalid DomainStatus value.")
        };

    private static EventVenueCalendarStatus ToDomain(EventVenueCalendarStatusDto status) =>
        status switch
        {
            EventVenueCalendarStatusDto.Draft => EventVenueCalendarStatus.Draft,
            EventVenueCalendarStatusDto.Published => EventVenueCalendarStatus.Published,
            EventVenueCalendarStatusDto.Cancelled => EventVenueCalendarStatus.Cancelled,
            _ => throw new ArgumentOutOfRangeException(
                nameof(status),
                status,
                "Invalid EventVenueCalendarStatus value."
            )
        };
}