using EventHouse.Management.Api.Contracts.Events;
using EventHouse.Management.Application.Queries.Events.GetAll;

namespace EventHouse.Management.Api.Mappers.Events;

public static class EventSortMapper
{
    public static EventSortField? ToApplication(EventSortBy? sortBy)
        => sortBy switch
        {
            EventSortBy.Name => EventSortField.Name,
            EventSortBy.Description => EventSortField.Description,
            EventSortBy.Scope => EventSortField.Scope,
            _ => null
        };
}
