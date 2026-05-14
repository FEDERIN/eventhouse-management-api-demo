
using EventHouse.Management.Api.Contracts.Common;
using Microsoft.AspNetCore.Mvc;

namespace EventHouse.Management.Api.Contracts.SeatingMaps;

public sealed record GetSeatingMapsRequest : SortablePaginationRequest<SeatingMapSortBy>
{
    /// <summary>Venue ID to filter seating maps by.</summary>
    [FromQuery(Name = "venueId")]
    public Guid? VenueId { get; init; }

    /// <summary>Seating map name.</summary>
    [FromQuery(Name = "name")]
    public string? Name { get; init; } = null!;

    /// <summary>Indicates whether the seatingMap is currently active.</summary>
    [FromQuery(Name = "isActive")]
    public bool? IsActive { get; init; }
}
