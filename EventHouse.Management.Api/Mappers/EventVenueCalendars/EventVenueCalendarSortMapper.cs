using EventHouse.Management.Api.Contracts.EventVenueCalendars;
using EventHouse.Management.Application.Queries.EventVenueCalendars.GetAll;

namespace EventHouse.Management.Api.Mappers.EventVenueCalendars;

public static class EventVenueCalendarSortMapper
{
    public static EventVenueCalendarSortField? ToApplication(EventVenueCalendarSortBy? sortBy) =>
    ApiEnumMapper<EventVenueCalendarSortBy, EventVenueCalendarSortField>.ToApplicationOptional(sortBy);
}