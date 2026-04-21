using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.SeatingMaps;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Data;

[ExcludeFromCodeCoverage]
internal static class SeatingMapExampleData
{
    private static readonly Guid SeatingMapId = ExampleConstants.SeatingMapId;
    private static readonly Guid VenueId = ExampleConstants.VenueId;
    private static readonly string Name = "Main Floor Seating";
    private static readonly bool IsActive = true;
    private static readonly int Version = 1;

    internal static CreateSeatingMapRequest Create() => new()
    {
        VenueId = VenueId,
        Name = Name,
        IsActive = IsActive,
    };

    internal static SeatingMapResponse Result() => new()
    {
        Id = SeatingMapId,
        VenueId = VenueId,
        Name = Name,
        Version = Version,
        IsActive = IsActive,
        CreatedAtUtc = DateTime.UtcNow,
    };

    internal static UpdateSeatingMapRequest Update() => new()
    {
        Name = Name,
        Version = Version,
        IsActive = IsActive,
    };

    internal static GetSeatingMapsRequest Get() => new()
    {
        VenueId = VenueId,
        Name = Name,
        IsActive = IsActive,
        Page = 1,
        PageSize = 20,
        SortBy = SeatingMapSortBy.Name,
        SortDirection = SortDirection.Asc,
    };
}
