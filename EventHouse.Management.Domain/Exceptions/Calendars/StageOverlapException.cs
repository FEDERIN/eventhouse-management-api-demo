namespace EventHouse.Management.Domain.Exceptions.Calendars;
public sealed class StageOverlapException(Guid calendarId, DateTime start, DateTime end)
    : DomainException($"The time slot {start} - {end} is already occupied in calendar '{calendarId}'.")
{
    public Guid CalendarId { get; } = calendarId;
    public DateTime Start { get; } = start;
    public DateTime End { get; } = end;
}
