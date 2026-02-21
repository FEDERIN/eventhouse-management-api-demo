namespace EventHouse.Management.Api.Contracts.Venues;

public sealed class Venue
{
    // <summary>Unique identifier of the venue.</summary>
    public Guid Id { get; set; }

    // <summary>Public display name of the venue.</summary>
    public string Name { get; set; } = null!;

    // <summary>Physical address of the venue.</summary>
    public string? Address { get; set; }

    // <summary>City where the venue is located.</summary>
    public string? City { get; set; }

    // <summary>Region or state where the venue is located.</summary>
    public string? Region { get; set; }

    // <summary>ISO country code representing the country where the venue is located.</summary>
    public string? CountryCode { get; set; }

    // <summary>Latitude coordinate of the venue's location.</summary>
    public decimal? Latitude { get; set; }

    // <summary>Longitude coordinate of the venue's location.</summary>
    public decimal? Longitude { get; set; }

    // <summary>Time zone identifier for the venue's location (e.g., "America/New_York").</summary>
    public string? TimeZoneId { get; set; }

    // <summary>Maximum capacity of the venue. Null if not specified.</summary>
    public int? Capacity { get; set; }

    // <summary>Indicates whether the venue is currently active and available for hosting events.</summary>
    public bool IsActive { get; set; } = true;
}
