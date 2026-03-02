namespace EventHouse.Management.Application.Common.Pagination;

public abstract record PaginationQuery
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}
