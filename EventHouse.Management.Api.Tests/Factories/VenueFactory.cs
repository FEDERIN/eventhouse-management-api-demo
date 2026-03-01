using Bogus;
using EventHouse.Management.Api.Contracts.Venues;

namespace EventHouse.Management.Api.Tests.Factories;

public static class VenueFactory
{
    private static readonly Faker Faker = new();

    public static CreateVenueRequest CreateRequest(
        string? name = null,
        int? capacity = null,
        string? city = null)
    {
        var cityName = city ?? Faker.Address.City();

        return new CreateVenueRequest
        {
            Name = name ?? $"{Faker.Address.StreetName()} Arena",
            Address = Faker.Address.StreetAddress(),
            City = cityName,
            Region = Faker.Address.StateAbbr(),
            CountryCode = "US",
            Latitude = decimal.Parse(Faker.Address.Latitude().ToString("F4")),
            Longitude = decimal.Parse(Faker.Address.Longitude().ToString("F4")),
            TimeZoneId = "America/New_York",
            Capacity = capacity ?? Faker.Random.Number(500, 50000),
            IsActive = true
        };
    }

    public static UpdateVenueRequest UpdateRequest(string? name = null) => new()
    {
        Name = name ?? $"{Faker.Address.StreetName()} Center",
        Address = Faker.Address.StreetAddress(),
        City = Faker.Address.City(),
        Region = Faker.Address.StateAbbr(),
        CountryCode = "US",
        Latitude = decimal.Parse(Faker.Address.Latitude().ToString("F4")),
        Longitude = decimal.Parse(Faker.Address.Longitude().ToString("F4")),
        TimeZoneId = "America/Chicago",
        Capacity = Faker.Random.Number(1000, 20000),
        IsActive = true
    };
}