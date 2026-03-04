using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.DTOs;
using MediatR;

namespace EventHouse.Management.Application.Queries.SeatingMaps.GetAll;

public sealed record GetAllSeatingMapsQuery
    : SortablePaginationQuery<SeatingMapSortField>, IRequest<PagedResultDto<SeatingMapDto>>
{
    public string? Name { get; init; }
    public Guid? VenueId { get; init; }
    public bool? IsActive { get; init; }
}
