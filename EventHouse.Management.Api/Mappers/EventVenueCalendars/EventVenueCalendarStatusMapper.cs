using EventHouse.Management.Api.Contracts.EventVenueCalendars;
using EventHouse.Management.Application.Common.Enums;

namespace EventHouse.Management.Api.Mappers.EventVenueCalendars;

public static class EventVenueCalendarStatusMapper
{
    public static EventVenueCalendarStatusDto ToApplicationRequired(EventVenueCalendarStatus contract) =>
    ApiEnumMapper<EventVenueCalendarStatus, EventVenueCalendarStatusDto>.ToApplicationRequired(contract);

    public static EventVenueCalendarStatusDto? ToApplicationOptional(EventVenueCalendarStatus? contract) =>
        ApiEnumMapper<EventVenueCalendarStatus, EventVenueCalendarStatusDto>.ToApplicationOptional(contract);

    public static EventVenueCalendarStatus ToContractRequired(EventVenueCalendarStatusDto dto) =>
        ApiEnumMapper<EventVenueCalendarStatus, EventVenueCalendarStatusDto>.ToContract(dto);
}
