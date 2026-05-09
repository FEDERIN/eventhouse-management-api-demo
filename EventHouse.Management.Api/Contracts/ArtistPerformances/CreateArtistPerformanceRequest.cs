using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace EventHouse.Management.Api.Contracts.ArtistPerformances;

/// <summary>
/// Represents the request to schedule an artist's performance at a specific venue and time slot.
/// </summary>
[SwaggerSchema(Description = "Contract for creating a relationship between an artist and a venue calendar slot.")]
public sealed record CreateArtistPerformanceRequest
{
    /// <summary>
    /// Unique identifier for the performing artist.
    /// </summary>
    /// <example>1b9d6bcd-bbfd-4b2d-9b5d-ab8dfbbd4bed</example>
    [SwaggerSchema(Description = "The unique ID of the artist to be assigned.")]
    [Required]
    public Guid ArtistId { get; init; }

    /// <summary>
    /// Indicates if the artist is the main act (Headliner) for this slot.
    /// Note: Business rule restricts this to one headliner per calendar slot.
    /// </summary>
    /// <example>true</example>
    [SwaggerSchema(Description = "Defines if the artist is the main act. (Constraint: Max one per slot).")]
    public bool IsHeadliner { get; init; }

    /// <summary>
    /// Scheduled start date and time.
    /// </summary>
    /// <example>2026-06-15T21:00:00Z</example>
    [SwaggerSchema(Description = "Planned start time (UTC).")]
    public DateTimeOffset? SetStart { get; init; }

    /// <summary>
    /// Scheduled end date and time.
    /// </summary>
    /// <example>2026-06-15T22:30:00Z</example>
    [SwaggerSchema(Description = "Planned end time (UTC). Must be after the start time.")]
    public DateTimeOffset? SetEnd { get; init; }
};