using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Domain.Enums;

namespace EventHouse.Management.Application.Queries.Artists.GetAll;

public sealed record ArtistQueryCriteria : SortablePaginationCriteria<ArtistSortField>
{
    public string? Name { get; init; }
    public ArtistCategory? Category { get; init; }
}
