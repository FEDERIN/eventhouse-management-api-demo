using EventHouse.Management.Api.Contracts.Common;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHouse.Management.Api.Contracts.Artists;

public sealed record GetArtistsRequest : SortablePaginationRequest<ArtistSortBy>
{
    /// <summary>Filter artists by name (contains match).</summary>
    [FromQuery(Name = "name")]
    [SwaggerSchema(Description = "Optional name filter (contains match).")]
    public string? Name { get; init; }

    /// <summary>Filter artists by category.</summary>
    [FromQuery(Name = "category")]
    [SwaggerSchema(Description = "Optional category filter.")]
    public ArtistCategory? Category { get; init; }
}
