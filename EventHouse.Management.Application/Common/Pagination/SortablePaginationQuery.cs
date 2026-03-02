namespace EventHouse.Management.Application.Common.Pagination;

using EventHouse.Management.Application.Common.Sorting;

public abstract record SortablePaginationQuery<TSort> : PaginationQuery
    where TSort : struct, Enum
{
    public TSort? SortBy { get; init; }
    public SortDirection SortDirection { get; init; } = SortDirection.Asc;
}
