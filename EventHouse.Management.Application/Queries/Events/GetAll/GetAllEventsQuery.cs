using EventHouse.Management.Application.Common.Enums;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.DTOs;
using MediatR;

namespace EventHouse.Management.Application.Queries.Events.GetAll;

public sealed record GetAllEventsQuery
    : SortablePaginationQuery<EventSortField>, IRequest<PagedResultDto<EventDto>>
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public EventScopeDto? Scope { get; init; }
}
