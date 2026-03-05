using EventHouse.Management.Application.Common.Pagination;

namespace EventHouse.Management.Application.Queries.SeatingMaps.GetAll;

public sealed record SeatingMapQueryCriteria : SortablePaginationCriteria<SeatingMapSortField>
{
    public Guid? VenueId { get; init; }
    public string? Name { get; init; }
    public bool? IsActive { get; init; }
}