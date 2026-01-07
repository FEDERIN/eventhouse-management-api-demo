using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Domain.Enums;

namespace EventHouse.Management.Application.Queries.Events.GetAll;

public sealed record EventQueryCriteria : SortablePaginationCriteria<EventSortField>
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public EventScope? Scope { get; init; }
}
