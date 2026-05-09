using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.DTOs;
using MediatR;


namespace EventHouse.Management.Application.Queries.ArtistPerformances.GetAll;

public sealed record GetAllArtistPerformancesQuery(Guid EventVenueCalendarId)
    : SortablePaginationQuery<ArtistPerformanceSortField>, IRequest<PagedResultDto<ArtistPerformanceDto>>
{
    public Guid EventVenueCalendarId { get; init; } = EventVenueCalendarId;
    public Guid? ArtistId { get; init; }
    public bool? IsHeadliner { get; init; }
}