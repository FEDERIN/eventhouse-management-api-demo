namespace EventHouse.Management.Api.Contracts.EventVenues;

public sealed class EventVenueResponse
{
    /// <summary>The unique identifier of the event venue.</summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    public Guid Id { get; set; }

    /// <summary>The unique identifier of the event to be associated with the event venue.</summary>
    /// <example>3fa85f64-5717-4562-bcd3-2c963f66afa6</example>
    public Guid EventId { get; set; }

    /// <summary>The unique identifier of the venue to be associated with the event venue.</summary>
    /// <example>3fa85f64-5268-4562-b3fc-2c963f66afa6</example>
    public Guid VenueId { get; set; }

    /// <summary>The status of the event venue.</summary>
    public EventVenueStatus Status { get; set; }

    /// <summary> Gets the name of the event.</summary>
    /// <example>Rock in Rio 2026</example>
    public string? EventName { get; init; }

    /// <summary> Gets the name of the venue.</summary>
    /// <example>Maracanã Stadium</example>
    public string? VenueName { get; init; }
}
