namespace EventHouse.Management.Domain.Exceptions.Venues;

public sealed class InvalidLongitudeException(decimal longitude)
    : DomainException(
        $"Longitude '{longitude}' is invalid. It must be between -180 and 180.")
{
    public decimal Longitude { get; } = longitude;
}