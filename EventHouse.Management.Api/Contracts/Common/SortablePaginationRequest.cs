using Microsoft.AspNetCore.Mvc;

namespace EventHouse.Management.Api.Contracts.Common;

public abstract record SortablePaginationRequest<TSort> : PaginationRequest
    where TSort : struct, Enum
{
    [FromQuery(Name = "sortBy")]
    public TSort? SortBy { get; init; }

    [FromQuery(Name = "sortDirection")]
    public SortDirection SortDirection { get; init; } = SortDirection.Asc;
}
public enum SortDirection
{
    Asc,
    Desc
}
