using EventHouse.Management.Api.Contracts.Common;
using Microsoft.AspNetCore.Mvc;

namespace EventHouse.Management.Api.Contracts.Genres;

public sealed record GetGenresRequest : SortablePaginationRequest<GenreSortBy>
{
    /// <summary>Filter genres by name (contains match).</summary>
    /// <example>Rock</example>
    [FromQuery(Name = "name")]
    public string? Name { get; init; }
}