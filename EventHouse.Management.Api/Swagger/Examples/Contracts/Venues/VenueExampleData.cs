using EventHouse.Management.Api.Contracts.Venues;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Venues;

[ExcludeFromCodeCoverage]
internal static class VenueExampleData
{
    internal static Venue MadisonSquareGarden() => new()
    {
        Id = new Guid("11111111-1111-1111-1111-111111111111"),
        Name = "Madison Square Garden",
        Address = "4 Pennsylvania Plaza, New York, NY 10001",
        City = "New York",
        Region = "NY",
        CountryCode = "US",
        Latitude = 40.7505m,
        Longitude = -73.9934m,
        TimeZoneId = "America/New_York",
        Capacity = 20000,
        IsActive = true
    };

    internal static CreateVenueRequest Create() => new()
    {
        Name = "Madison Square Garden",
        Address = "4 Pennsylvania Plaza, New York, NY 10001",
        City = "New York",
        Region = "NY",
        CountryCode = "US",
        Latitude = 40.7505m,
        Longitude = -73.9934m,
        TimeZoneId = "America/New_York",
        Capacity = 20000,
        IsActive = true
    };

    internal static UpdateVenueRequest Update() => new()
    {
        Name = "Madison Square Garden",
        Address = "4 Pennsylvania Plaza, New York, NY 10001",
        City = "New York",
        Region = "NY",
        CountryCode = "US",
        Latitude = 40.7505m,
        Longitude = -73.9934m,
        TimeZoneId = "America/New_York",
        Capacity = 20000,
        IsActive = true
    };
}