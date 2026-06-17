namespace EventHouse.Management.Application.Common.Pagination;

public abstract record PaginationCriteria
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}
