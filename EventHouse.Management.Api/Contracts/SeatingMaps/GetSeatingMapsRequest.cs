
using EventHouse.Management.Api.Contracts.Common;
using Microsoft.AspNetCore.Mvc;

namespace EventHouse.Management.Api.Contracts.SeatingMaps;

public sealed record GetSeatingMapsRequest : SortablePaginationRequest<SeatingMapSortBy>
{
    /// <summary>Venue ID to filter seating maps by.</summary>
    /// <example>11111111-1111-1111-1111-111111111111</example>
    [FromQuery(Name = "venueId")]
    public Guid? VenueId { get; init; }

    /// <summary>Seating map name.</summary>
    /// <example>Main Floor Seating</example>
    [FromQuery(Name = "name")]
    public string? Name { get; init; } = null!;

    /// <summary>Indicates whether the seatingMap is currently active.</summary>
    /// <example>true</example>
    [FromQuery(Name = "isActive")]
    public bool? IsActive { get; init; }
}
