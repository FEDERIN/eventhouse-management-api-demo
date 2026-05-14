using EventHouse.Management.Application.Common.Pagination;

namespace EventHouse.Management.Application.Queries.ArtistPerformances.GetAll;

public sealed record ArtistPerformanceQueryCriteria : SortablePaginationCriteria<ArtistPerformanceSortField>
{
    public Guid EventVenueCalendarId { get; init; }
    public Guid? ArtistId { get; init; }
    public bool? IsHeadliner { get; init; }
}
