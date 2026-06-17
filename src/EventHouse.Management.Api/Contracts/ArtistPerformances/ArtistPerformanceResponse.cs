namespace EventHouse.Management.Api.Contracts.ArtistPerformances;

public sealed record ArtistPerformanceResponse
{
    public Guid Id { get; init; }
    public Guid EventVenueCalendarId { get; init; }
    public Guid ArtistId { get; init; }
    public bool IsHeadliner { get; init; }
    public DateTimeOffset? SetStart { get; init; }
    public DateTimeOffset? SetEnd { get; init; }
}