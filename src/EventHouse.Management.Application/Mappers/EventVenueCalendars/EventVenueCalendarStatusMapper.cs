
using EventHouse.Management.Application.Common.Enums;
using EventHouse.Management.Domain.Enums;

namespace EventHouse.Management.Application.Mappers.EventVenueCalendars;

public static class EventVenueCalendarStatusMapper
{

    public static EventVenueCalendarStatus ToDomainRequired(EventVenueCalendarStatusDto dto) =>
    EnumMapper<EventVenueCalendarStatus, EventVenueCalendarStatusDto>.ToDomainRequired(dto);

    public static EventVenueCalendarStatus? ToDomainOptional(EventVenueCalendarStatusDto? dto) =>
        EnumMapper<EventVenueCalendarStatus, EventVenueCalendarStatusDto>.ToDomainOptional(dto);

    public static EventVenueCalendarStatusDto ToApplicationRequired(EventVenueCalendarStatus domain) =>
        EnumMapper<EventVenueCalendarStatus, EventVenueCalendarStatusDto>.ToApplicationRequired(domain);
}