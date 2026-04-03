using EventHouse.Management.Api.Contracts.EventVenues;
using EventHouse.Management.Application.Queries.EventVenues.GetAll;

namespace EventHouse.Management.Api.Mappers.EventVenues;

public static class EventVenueSortMapper
{
    public static EventVenueSortField? ToApplication(EventVenueSortBy? sortBy) =>
    ApiEnumMapper<EventVenueSortBy, EventVenueSortField>.ToApplicationOptional(sortBy);
}