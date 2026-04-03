namespace EventHouse.Management.Api.Contracts.SeatingMaps;

public sealed class SeatingMapResponse
{
    /// <summary>Unique identifier of the seating map.</summary>
    public Guid Id { get; init; }

    /// <summary>Unique identifier of the venue.</summary>
    public Guid VenueId { get; init; }

    /// <summary>Seating map name.</summary>
    public string Name { get; init; } = null!;

    /// <summary>Current version of the seating map (for concurrency control).</summary>
    public int Version { get; init; }

    /// <summary>Indicates whether the seatingMap is currently active.</summary>
    public bool IsActive { get; init; }

    /// <summary>The date and time the seating map was created in UTC.</summary>
    public DateTime CreatedAtUtc { get; init; }
}