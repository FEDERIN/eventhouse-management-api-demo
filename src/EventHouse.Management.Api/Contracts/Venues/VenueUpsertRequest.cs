using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace EventHouse.Management.Api.Contracts.Venues;

public abstract class VenueUpsertRequest
{
    /// <summary>Venue name.</summary>
    [Required, MinLength(2), MaxLength(200)]
    [SwaggerSchema(Description = "Venue name. Must be between 2 and 200 characters.")]
    public string Name { get; set; } = null!;

    /// <summary>Physical address of the venue.</summary>
    [Required, MinLength(2), MaxLength(200)]
    [SwaggerSchema(Description = "Physical address of the venue. Must be between 2 and 200 characters.")]
    public string Address { get; set; } = null!;

    /// <summary>City where the venue is located.</summary>
    [Required, MinLength(2), MaxLength(100)]
    [SwaggerSchema(Description = "City where the venue is located. Must be between 2 and 100 characters.")]
    public string City { get; set; } = null!;

    /// <summary>Region or state where the venue is located.</summary>
    [Required, MinLength(2), MaxLength(100)]
    [SwaggerSchema(Description = "Region or state where the venue is located. Must be between 2 and 100 characters.")]
    public string Region { get; set; } = null!;

    /// <summary>ISO-3166-1 alpha-2 country code.</summary>
    [Required, MinLength(2), MaxLength(2)]
    [RegularExpression("^[A-Z]{2}$")]
    [SwaggerSchema(Description = "ISO-3166-1 alpha-2 country code (uppercase). Example: 'ES'.")]
    public string CountryCode { get; set; } = null!;

    /// <summary>Latitude coordinate of the venue's location.</summary>
    [Range(-90, 90)]
    [SwaggerSchema(Description = "Latitude coordinate of the venue's location. Must be between -90 and 90.")]
    public decimal Latitude { get; set; }

    /// <summary>Longitude coordinate of the venue's location.</summary>
    [Range(-180, 180)]
    [SwaggerSchema(Description = "Longitude coordinate of the venue's location. Must be between -180 and 180.")]
    public decimal Longitude { get; set; }

    /// <summary>Time zone identifier (IANA), e.g. "America/New_York".</summary>
    [MaxLength(64)]
    [SwaggerSchema(Description = "Time zone identifier (IANA), e.g. 'America/New_York'.")]
    public required string TimeZoneId { get; set; }

    /// <summary>Maximum capacity of the venue. Null if not specified.</summary>
    [Range(0, int.MaxValue)]
    [SwaggerSchema(Description = "Maximum capacity of the venue. Must be a non-negative integer.")]
    public int? Capacity { get; set; }

    /// <summary>Indicates whether the venue is currently active.</summary>
    [SwaggerSchema(Description = "Indicates whether the venue is currently active. Defaults to true.")]
    public bool IsActive { get; set; } = true;
}
