using EventHouse.Management.Api.Contracts.Events;
using EventHouse.Management.Application.Queries.Events.GetAll;

namespace EventHouse.Management.Api.Mappers.Events;

internal static class EventSortMapper
{
    public static EventSortField? ToApplication(EventSortBy? sortBy) =>
        ApiEnumMapper<EventSortBy, EventSortField>.ToApplicationOptional(sortBy);
}