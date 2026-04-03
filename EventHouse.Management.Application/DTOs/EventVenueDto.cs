using EventHouse.Management.Application.Common.Enums;

namespace EventHouse.Management.Application.DTOs;

/// <summary>
/// Data Transfer Object for EventVenue, designed for high-performance 
/// read operations and frontend consumption.
/// </summary>
public record EventVenueDto
{
    public Guid Id { get; init; }
    public Guid EventId { get; init; }
    public Guid VenueId { get; init; }
    public string? EventName { get; init; }
    public string? VenueName { get; init; }
    public EventVenueStatusDto Status { get; init; }
}