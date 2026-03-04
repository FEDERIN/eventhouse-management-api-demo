using EventHouse.Management.Api.Contracts.SeatingMaps;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.SeatingMap;

[ExcludeFromCodeCoverage]
internal static class SeatingMapExampleData
{
    internal static CreateSeatingMapRequest Create() => new()
    {
        VenueId = new Guid("11111111-1111-1111-1111-111111111111"),
        Name = "Main Floor Seating",
        IsActive = true,
    };

    internal static SeatingMapResponse Result() => new()
    {
        Id = new Guid("22222222-2222-2222-2222-222222222222"),
        VenueId = new Guid("11111111-1111-1111-1111-111111111111"),
        Name = "Main Floor Seating",
        Version = 1,
        IsActive = true,
        CreatedAtUtc = DateTime.UtcNow,
    };

    internal static UpdateSeatingMapRequest Update() => new()
    {
        VenueId = new Guid("11111111-1111-1111-1111-111111111111"),
        Name = "Main Floor Seating",
        Version = 1,
        IsActive = true,
    };

}
