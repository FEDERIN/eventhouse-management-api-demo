namespace EventHouse.Management.Domain.Exceptions.Calendars;

public sealed class InvalidTimeRangeException(DateTime start, DateTime end)
    : DomainException(
        $"Start date '{start:O}' must be earlier than end date '{end:O}'.")
{
    public DateTime Start { get; } = start;
    public DateTime End { get; } = end;
}