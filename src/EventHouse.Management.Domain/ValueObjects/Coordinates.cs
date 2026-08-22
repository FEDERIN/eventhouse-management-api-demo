using EventHouse.Management.Domain.Exceptions.Venues;

namespace EventHouse.Management.Domain.ValueObjects;

public sealed record Coordinates
{
    public decimal Latitude { get; }
    public decimal Longitude { get; }

    private Coordinates(decimal latitude, decimal longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    public static Coordinates Create(decimal latitude, decimal longitude)
    {
        if (latitude < -90 || latitude > 90)
            throw new InvalidLatitudeException(latitude);

        if (longitude < -180 || longitude > 180)
            throw new InvalidLongitudeException(longitude);

        return new Coordinates(latitude, longitude);
    }
}