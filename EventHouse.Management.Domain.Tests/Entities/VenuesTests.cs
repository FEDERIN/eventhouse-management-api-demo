using EventHouse.Management.Domain.Entities;

namespace EventHouse.Management.Domain.Tests.Entities;

public sealed class VenuesTests
{
 
    private static readonly Guid Id = Guid.NewGuid();
    private static readonly string Name = "Palace Theatre";
    private static readonly string Address = "123 Main St";
    private static readonly string City = "Valletta";
    private static readonly string Region = "Malta";
    private static readonly string CountryCode = "MT";
    private static readonly decimal? Latitude = 35.8989m;
    private static readonly decimal? Longitude = 14.5146m;
    private static readonly string? TimeZoneId = TimeZoneInfo.GetSystemTimeZones().First().Id;
    private static readonly int? Capacity = 500;
    private static readonly bool IsActive = true;
    private static readonly string TextTooLong = new('A', 201);

    [Fact]
    public void Should_create_venue_with_valid_data()
    {
        // Act
        var venue = new Venue(
            Id,
            Name,
            Address,
            City,
            Region,
            CountryCode,
            Latitude,
            Longitude,
            TimeZoneId,
            Capacity,
            IsActive);

        // Assert
        Assert.Equal(Id, venue.Id);
        Assert.Equal(Name, venue.Name);
        Assert.Equal(Address, venue.Address);
        Assert.Equal(City, venue.City);
        Assert.Equal(Region, venue.Region);
        Assert.Equal(CountryCode, venue.CountryCode);
        Assert.Equal(Latitude, venue.Latitude);
        Assert.Equal(Longitude, venue.Longitude);
        Assert.Equal(TimeZoneId, venue.TimeZoneId);
        Assert.Equal(Capacity, venue.Capacity);
        Assert.Equal(IsActive, venue.IsActive);
    }

    [Fact]
    public void Should_create_venue_with_valid_data_with_field_null()
    {
        // Act
        var venue = new Venue(
            Id,
            Name,
            Address,
            City,
            Region,
            CountryCode,
            null,
            null,
            null,
            Capacity,
            IsActive);

        // Assert
        Assert.Equal(Id, venue.Id);
        Assert.Equal(Name, venue.Name);
        Assert.Equal(Address, venue.Address);
        Assert.Equal(City, venue.City);
        Assert.Equal(Region, venue.Region);
        Assert.Equal(CountryCode, venue.CountryCode);
        Assert.Null(venue.Latitude);
        Assert.Null(venue.Longitude);
        Assert.Null(venue.TimeZoneId);
        Assert.Equal(Capacity, venue.Capacity);
        Assert.Equal(IsActive, venue.IsActive);
    }


    [Fact]
    public void Should_throw_when_id_is_empty()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            new Venue(
                Guid.Empty,
                Name,
                Address,
                City,
                Region,
                CountryCode,
                Latitude,
                Longitude,
                TimeZoneId,
                Capacity,
                IsActive));

        Assert.Equal("id", ex.ParamName);
    }

    [Fact]
    public void Should_throw_when_name_is_empty()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        new Venue(
                Id,
                string.Empty,
                Address,
                City,
                Region,
                CountryCode,
                Latitude,
                Longitude,
                TimeZoneId,
                Capacity,
                IsActive));

        Assert.Equal("name", ex.ParamName);
    }

    [Fact]
    public void Should_throw_when_name_is_too_long()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        new Venue(
                Id,
                TextTooLong,
                Address,
                City,
                Region,
                CountryCode,
                Latitude,
                Longitude,
                TimeZoneId,
                Capacity,
                IsActive));

        Assert.Equal("name", ex.ParamName);
    }

    [Fact]
    public void Should_throw_when_address_is_empty()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        new Venue(
                Id,
                Name,
                string.Empty,
                City,
                Region,
                CountryCode,
                Latitude,
                Longitude,
                TimeZoneId,
                Capacity,
                IsActive));

        Assert.Equal("address", ex.ParamName);
    }

    [Fact]
    public void Should_throw_when_address_is_too_long()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        new Venue(
                Id,
                Name,
                TextTooLong,
                City,
                Region,
                CountryCode,
                Latitude,
                Longitude,
                TimeZoneId,
                Capacity,
                IsActive));

        Assert.Equal("address", ex.ParamName);
    }

    [Fact]
    public void Should_throw_when_city_is_empty()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        new Venue(
                Id,
                Name,
                Address,
                string.Empty,
                Region,
                CountryCode,
                Latitude,
                Longitude,
                TimeZoneId,
                Capacity,
                IsActive));

        Assert.Equal("city", ex.ParamName);
    }

    [Fact]
    public void Should_throw_when_city_is_too_long()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        new Venue(
                Id,
                Name,
                Address,
                TextTooLong,
                Region,
                CountryCode,
                Latitude,
                Longitude,
                TimeZoneId,
                Capacity,
                IsActive));

        Assert.Equal("city", ex.ParamName);
    }

    [Fact]
    public void Should_throw_when_region_is_empty()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        new Venue(
                Id,
                Name,
                Address,
                City,
                string.Empty,
                CountryCode,
                Latitude,
                Longitude,
                TimeZoneId,
                Capacity,
                IsActive));

        Assert.Equal("region", ex.ParamName);
    }

    [Fact]
    public void Should_throw_when_region_is_too_long()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        new Venue(
                Id,
                Name,
                Address,
                City,
                TextTooLong,
                CountryCode,
                Latitude,
                Longitude,
                TimeZoneId,
                Capacity,
                IsActive));

        Assert.Equal("region", ex.ParamName);
    }

    [Fact]
    public void Should_throw_when_countryCode_is_empty()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        new Venue(
                Id,
                Name,
                Address,
                City,
                Region,
                string.Empty,
                Latitude,
                Longitude,
                TimeZoneId,
                Capacity,
                IsActive));

        Assert.Equal("countryCode", ex.ParamName);
    }


    [Fact]
    public void Should_throw_when_country_code_is_invalid()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            new Venue(
                    Id,
                    Name,
                    Address,
                    City,
                    Region,
                    "MAL",
                    Latitude,
                    Longitude,
                    TimeZoneId,
                    Capacity,
                    IsActive));


        Assert.Equal("countryCode", ex.ParamName);
    }

    [Fact]
    public void Should_throw_when_latitude_is_out_of_range()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        new Venue(
                Id,
                Name,
                Address,
                City,
                Region,
                CountryCode,
                100,
                Longitude,
                TimeZoneId,
                Capacity,
                IsActive));
    }

    [Fact]
    public void Should_throw_when_longitude_is_out_of_range()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        new Venue(
                Id,
                Name,
                Address,
                City,
                Region,
                CountryCode,
                Latitude,
                200,
                TimeZoneId,
                Capacity,
                IsActive));
    }

    [Fact]
    public void Should_throw_when_timeZoneId_is_Invalid()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            new Venue(
                    Id,
                    Name,
                    Address,
                    City,
                    Region,
                    CountryCode,
                    Latitude,
                    Longitude,
                    "America/Miami",
                    Capacity,
                    IsActive));

        Assert.Equal("timeZoneId", ex.ParamName);
    }

    [Fact]
    public void Should_throw_when_capacity_is_negative()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            new Venue(
                    Id,
                    Name,
                    Address,
                    City,
                    Region,
                    CountryCode,
                    Latitude,
                    Longitude,
                    TimeZoneId,
                    -1,
                    IsActive));

        Assert.Equal("capacity", ex.ParamName);
    }
}