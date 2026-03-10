using EventHouse.Management.Api.Contracts.EventVenueCalendars;
using EventHouse.Management.Application.Common.Enums;

namespace EventHouse.Management.Api.Mappers.Enums;

public static class EventVenueCalendarStatusMapper
{
    public static EventVenueCalendarStatusDto ToApplicationRequired(EventVenueCalendarStatus statusContract)
        => ToApplication(statusContract);

    public static EventVenueCalendarStatusDto? ToApplicationOptional(EventVenueCalendarStatus? statusContract)
        => statusContract.HasValue ? ToApplication(statusContract.Value) : null;

    public static EventVenueCalendarStatus ToContractRequired(this EventVenueCalendarStatusDto status) =>
    status switch
    {
        EventVenueCalendarStatusDto.Draft => EventVenueCalendarStatus.Draft,
        EventVenueCalendarStatusDto.Published => EventVenueCalendarStatus.Published,
        EventVenueCalendarStatusDto.Cancelled => EventVenueCalendarStatus.Cancelled,
        _ => throw new ArgumentOutOfRangeException(nameof(status), status, "Invalid EventVenueCalendarStatus value.")
    };

    private static EventVenueCalendarStatusDto ToApplication(EventVenueCalendarStatus statusContract)
    {
        return statusContract switch
        {
            EventVenueCalendarStatus.Draft => EventVenueCalendarStatusDto.Draft,
            EventVenueCalendarStatus.Published => EventVenueCalendarStatusDto.Published,
            EventVenueCalendarStatus.Cancelled => EventVenueCalendarStatusDto.Cancelled,
            _ => throw new ArgumentOutOfRangeException(
                nameof(statusContract),
                statusContract,
                "Invalid EventVenueCalendarStatusContract value."
            )
        };
    }
}
