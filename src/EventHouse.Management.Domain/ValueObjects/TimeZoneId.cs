namespace EventHouse.Management.Domain.ValueObjects;

public sealed record TimeZoneId
{
    public const string Utc = "UTC";

    public string Value { get; }

    private TimeZoneId(string value)
    {
        Value = value;
    }

    public static TimeZoneId Create(string? value)
    {
        value = string.IsNullOrWhiteSpace(value)
            ? Utc
            : value.Trim();

        return new(value);
    }

    public override string ToString() => Value;

    public static implicit operator string(TimeZoneId id)
        => id.Value;
}