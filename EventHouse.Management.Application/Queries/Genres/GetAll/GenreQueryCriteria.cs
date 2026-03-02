using EventHouse.Management.Application.Common.Pagination;

namespace EventHouse.Management.Application.Queries.Genres.GetAll;

public sealed record GenreQueryCriteria : SortablePaginationCriteria<GenreSortField>
{
    public string? Name { get; init; }
}
