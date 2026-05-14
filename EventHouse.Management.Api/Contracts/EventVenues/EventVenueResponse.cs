namespace EventHouse.Management.Api.Contracts.EventVenues;

public sealed class EventVenueResponse
{
    /// <summary>The unique identifier of the event venue.</summary>
    public Guid Id { get; set; }

    /// <summary>The unique identifier of the event to be associated with the event venue.</summary>
    public Guid EventId { get; set; }

    /// <summary>The unique identifier of the venue to be associated with the event venue.</summary>
    public Guid VenueId { get; set; }

    /// <summary>The status of the event venue.</summary>
    public EventVenueStatus Status { get; set; }

    /// <summary> Gets the name of the event.</summary>
    public string? EventName { get; init; }

    /// <summary> Gets the name of the venue.</summary>
    public string? VenueName { get; init; }
}
