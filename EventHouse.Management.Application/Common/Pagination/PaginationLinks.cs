
namespace EventHouse.Management.Application.Common.Pagination;

public sealed class PaginationLinks
{
    public string? Self { get; init; }
    public string? First { get; init; }
    public string? Previous { get; init; }
    public string? Next { get; init; }
    public string? Last { get; init; }
}

