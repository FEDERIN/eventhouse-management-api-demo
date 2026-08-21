namespace EventHouse.Management.Domain.Exceptions.Venues;

public sealed class InvalidLatitudeException(decimal latitude)
    : DomainException(
        $"Latitude '{latitude}' is invalid. It must be between -90 and 90.")
{
    public decimal Latitude { get; } = latitude;
}