namespace EventHouse.Management.Domain.Exceptions.Calendars;

public sealed class PerformanceOutsideCalendarException(
    DateTime calendarStart,
    DateTime calendarEnd,
    DateTime performanceStart,
    DateTime performanceEnd)
    : DomainException(
        $"Performance ({performanceStart:o} - {performanceEnd:o}) must be within the calendar boundaries ({calendarStart:o} - {calendarEnd:o}).")
{
    public DateTime CalendarStart { get; } = calendarStart;
    public DateTime CalendarEnd { get; } = calendarEnd;
    public DateTime PerformanceStart { get; } = performanceStart;
    public DateTime PerformanceEnd { get; } = performanceEnd;
}