using EventHouse.Management.Api.Contracts.EventVenueCalendars;
using EventHouse.Management.Application.Queries.EventVenueCalendars.GetAll;

namespace EventHouse.Management.Api.Mappers.EventVenueCalendars;

public static class EventVenueCalendarSortMapper
{
    public static EventVenueCalendarSortField? ToApplication(EventVenueCalendarSortBy? sortBy)
        => sortBy switch
        {
            EventVenueCalendarSortBy.StartDate => EventVenueCalendarSortField.StartDate,
            EventVenueCalendarSortBy.EndDate   => EventVenueCalendarSortField.EndDate,
            EventVenueCalendarSortBy.TimeZoneId => EventVenueCalendarSortField.TimeZoneId,
            EventVenueCalendarSortBy.Status => EventVenueCalendarSortField.Status,
            _ => null
        };
}