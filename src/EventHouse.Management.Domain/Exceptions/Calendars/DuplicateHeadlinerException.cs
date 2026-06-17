namespace EventHouse.Management.Domain.Exceptions.Calendars;

public sealed class DuplicateHeadlinerException(Guid calendarId)
    : DomainException($"Calendar '{calendarId}' already has a headliner assigned.")
{
    public Guid CalendarId { get; } = calendarId;
}