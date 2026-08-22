namespace EventHouse.Management.Domain.ValueObjects;

using EventHouse.Management.Domain.Exceptions.Calendars;

public sealed record TimeRange
{
    public DateTime Start { get; }
    public DateTime End { get; }

    public TimeSpan Duration => End - Start;

    private TimeRange(DateTime start, DateTime end)
    {
        Start = start;
        End = end;
    }

    public static TimeRange Create(DateTime start, DateTime end)
    {
        if (end <= start)
            throw new InvalidTimeRangeException(start, end);

        return new(start, end);
    }

    /// <summary>
    /// Returns true when the specified range is completely contained within this range.
    /// </summary>
    public bool Contains(TimeRange other)
    {
        ArgumentNullException.ThrowIfNull(other);

        return other.Start >= Start &&
               other.End <= End;
    }

    /// <summary>
    /// Returns true when the specified instant belongs to this range.
    /// </summary>
    public bool Contains(DateTime instant)
    {
        return instant >= Start &&
               instant <= End;
    }

    /// <summary>
    /// Returns true when both ranges overlap.
    /// </summary>
    public bool Overlaps(TimeRange other)
    {
        ArgumentNullException.ThrowIfNull(other);

        return Start < other.End &&
               End > other.Start;
    }
}