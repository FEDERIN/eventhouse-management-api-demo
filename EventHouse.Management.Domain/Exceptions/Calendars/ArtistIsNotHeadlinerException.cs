namespace EventHouse.Management.Domain.Exceptions.Calendars;

public sealed class ArtistIsNotHeadlinerException(Guid calendarId, Guid artistId)
    : DomainException($"Artist '{artistId}' is not the current headliner for calendar '{calendarId}'.")
{
    public Guid CalendarId { get; } = calendarId;
    public Guid ArtistId { get; } = artistId;
}