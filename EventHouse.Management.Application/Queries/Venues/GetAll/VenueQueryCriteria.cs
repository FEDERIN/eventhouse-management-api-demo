using EventHouse.Management.Application.Common.Pagination;

namespace EventHouse.Management.Application.Queries.Venues.GetAll;

public sealed record VenueQueryCriteria : SortablePaginationCriteria<VenueSortField>
{
    public string? Name { get; init; }
    public string? Address { get; init; }
    public string? City { get; init; }
    public string? Region { get; init; }
    public string? CountryCode { get; init; }
    public int? Capacity { get; init; }
    public bool? IsActive { get; init; }
}