using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.DTOs;
using MediatR;

namespace EventHouse.Management.Application.Queries.Venues.GetAll;

public sealed record GetAllVenuesQuery
: SortablePaginationQuery<VenueSortField>, IRequest<PagedResultDto<VenueDto>>
{
    public string? Name { get; init; }
    public string? Address { get; init; }
    public string? City { get; init; }
    public string? Region { get; init; }
    public string? CountryCode { get; init; }
    public int? Capacity { get; init; }
    public bool? IsActive { get; init; }
}
