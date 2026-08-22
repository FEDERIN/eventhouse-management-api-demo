namespace EventHouse.Management.Application.DTOs;

public sealed class ArtistPerformanceDto
{
    public Guid Id { get; set; }
    public Guid EventVenueCalendarId { get; set; }
    public Guid ArtistId { get; set; }
    public bool IsHeadliner { get; set; }
    public DateTimeOffset? SetStart { get; set; }
    public DateTimeOffset? SetEnd { get; set; }
}
