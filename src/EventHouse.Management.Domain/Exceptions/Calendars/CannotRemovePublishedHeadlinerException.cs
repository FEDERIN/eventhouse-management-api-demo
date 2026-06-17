namespace EventHouse.Management.Domain.Exceptions.Calendars;

public sealed class CannotRemovePublishedHeadlinerException(Guid calendarId, Guid artistId)
    : DomainException($"Cannot remove artist '{artistId}' because they are the headliner of the published calendar '{calendarId}'. Assign a new headliner first.")
{
    public Guid CalendarId { get; } = calendarId;
    public Guid ArtistId { get; } = artistId;
}