using EventHouse.Management.Application.Common.Enums;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.DTOs;
using MediatR;

namespace EventHouse.Management.Application.Queries.EventVenues.GetAll;

public sealed record GetAllEventVenuesQuery
    : SortablePaginationQuery<EventVenueSortField>, IRequest<PagedResultDto<EventVenueDto>>
{
    public Guid? EventId { get; init; }
    public Guid? VenueId { get; init; }
    public EventVenueStatusDto? Status { get; init; }
}