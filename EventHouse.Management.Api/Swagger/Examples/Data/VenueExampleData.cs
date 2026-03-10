using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.Venues;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Data;

[ExcludeFromCodeCoverage]
internal static class VenueExampleData
{
    private static readonly Guid Id = ExampleConstants.VenueId;
    private static readonly string Name = ExampleConstants.VenueName;
    private static readonly string Address = "4 Pennsylvania Plaza, New York, NY 10001";
    private static readonly string City = "New York";
    private static readonly string Region = "NY";
    private static readonly string CountryCode = "US";
    private static readonly decimal Latitude = 40.7505m;
    private static readonly decimal Longitude = -73.9934m;
    private static readonly string TimeZoneId = ExampleConstants.TimeZoneId;
    private static readonly int Capacity = 20000;
    private static readonly bool IsActive = true;

    internal static VenueResponse Result() => new()
    {
        Id = Id,
        Name = Name,
        Address = Address,
        City = City,
        Region = Region,
        CountryCode = CountryCode,
        Latitude = Latitude,
        Longitude = Longitude,
        TimeZoneId = TimeZoneId,
        Capacity = Capacity,
        IsActive = IsActive
    };

    internal static CreateVenueRequest Create() => new()
    {
        Name = Name,
        Address = Address,
        City = City,
        Region = Region,
        CountryCode = CountryCode,
        Latitude = Latitude,
        Longitude = Longitude,
        TimeZoneId = TimeZoneId,
        Capacity = Capacity,
        IsActive = IsActive
    };

    internal static UpdateVenueRequest Update() => new()
    {
        Name = Name,
        Address = Address,
        City = City,
        Region = Region,
        CountryCode = CountryCode,
        Latitude = Latitude,
        Longitude = Longitude,
        TimeZoneId = TimeZoneId,
        Capacity = Capacity,
        IsActive = IsActive
    };

    internal static GetVenuesRequest Get() => new()
    {
        Name = Name,
        Address = Address,
        City = City,
        Region = Region,
        CountryCode = CountryCode,
        Capacity = Capacity,
        IsActive = IsActive,
        Page = 1,
        PageSize = 15,
        SortBy = VenueSortBy.Name,
        SortDirection = SortDirection.Asc
    };
}