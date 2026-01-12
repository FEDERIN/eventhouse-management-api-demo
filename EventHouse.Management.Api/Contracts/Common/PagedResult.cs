using EventHouse.Management.Application.Common.Pagination;

namespace EventHouse.Management.Api.Contracts.Common;

public sealed class PagedResult<T>
{
    public List<T> Items { get; init; } = [];
    public int TotalCount { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
    public PaginationLinks? Links { get; init; }
}
