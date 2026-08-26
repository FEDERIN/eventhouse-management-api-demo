using Bogus;
using EventHouse.Management.Api.Contracts.Seating.Maps;

namespace EventHouse.Management.Api.Tests.Factories;

public static class SeatingMapFactory
{
    private static readonly Faker Faker = new();

    public static CreateSeatingMapRequest CreateRequest(
        Guid? venueId = null,
        string? name = null,
        int version = 1)
    {
        return new CreateSeatingMapRequest
        {
            VenueId = venueId ?? Guid.NewGuid(),
            Name = name ?? $"{Faker.Commerce.ProductName()} {Faker.Random.Int(1, 999)}",
            Version = version
        };
    }

    public static UpdateSeatingMapRequest UpdateRequest(
        string? name = null,
        int version = 1)
    {
        return new UpdateSeatingMapRequest
        {
            Name = name ?? Faker.Commerce.ProductName(),
            Version = version
        };
    }
}