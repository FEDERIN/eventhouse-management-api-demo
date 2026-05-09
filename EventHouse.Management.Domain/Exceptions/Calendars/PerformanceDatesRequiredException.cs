namespace EventHouse.Management.Domain.Exceptions.Artists;

public sealed class PerformanceDatesRequiredException(Guid calendarId, Guid artistId) 
    : Exception($"Performance dates are mandatory for artist {artistId} because calendar {calendarId} is published.")
{
    public Guid CalendarId { get; } = calendarId;
    public Guid ArtistId { get; } = artistId;
}