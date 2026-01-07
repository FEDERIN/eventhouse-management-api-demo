namespace EventHouse.Management.Application.Common.Pagination;

using EventHouse.Management.Application.Common.Sorting;

public abstract record SortablePaginationCriteria<TSort> : PaginationCriteria
    where TSort : struct, Enum
{
    public TSort? SortBy { get; init; }
    public SortDirection SortDirection { get; init; } = SortDirection.Asc;
}
