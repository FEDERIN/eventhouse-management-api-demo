using EventHouse.ShareKernel.Entities;

namespace EventHouse.Management.Domain.Entities;

public class Venue : Entity
{
    public string Name { get; private set; } = default!;
    public string Address { get; private set; } = string.Empty;
    public string City { get; private set; } = string.Empty;
    public string Region { get; private set; } = string.Empty;
    public string CountryCode { get; private set; } = string.Empty;
    public decimal? Latitude { get; private set; }
    public decimal? Longitude { get; private set; }
    public string? TimeZoneId { get; private set; }
    public int? Capacity { get; private set; }
    public bool IsActive { get; private set; } = true;

    private Venue() { }

    public Venue(Guid id, string name, string address, string city, string region, string countryCode,
        decimal? latitude, decimal? longitude, string? timeZoneId, int? capacity, bool isActive)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty.", nameof(id));

        Id = id;

        Update(name, address, city, region, countryCode,
            latitude, longitude, timeZoneId, capacity, isActive);   
    }

    public void Update(string name, string address, string city, string region, string countryCode,
        decimal? latitude, decimal? longitude, string? timeZoneId, int? capacity, bool isActive)
    {
        SetName(name);
        SetAddress(address, city, region, countryCode);
        SetCoordinates(latitude, longitude);
        SetTimeZone(timeZoneId);
        SetCapacity(capacity);
        IsActive = isActive;
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;

    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Venue name is required.", nameof(name));

        if (name.Length > 200)
            throw new ArgumentException("Venue name is too long.", nameof(name));

        Name = name.Trim();
    }

    private void SetAddress(
        string? address,
        string? city,
        string? region,
        string? countryCode)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Venue address is required.", nameof(address));

        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("Venue city is required.", nameof(city));

        if (string.IsNullOrWhiteSpace(region))
            throw new ArgumentException("Venue region is required.", nameof(region));

        if (string.IsNullOrWhiteSpace(countryCode))
            throw new ArgumentException("Venue countryCode is required.", nameof(countryCode));

        Address = address.Trim();
        City = city.Trim();
        Region = region.Trim();

        var cc = countryCode.Trim().ToUpperInvariant();

        if (cc.Length != 2) throw new ArgumentException("CountryCode must be ISO-3166 alpha-2.", nameof(countryCode));
        CountryCode = cc;
    }

    private void SetCoordinates(decimal? latitude, decimal? longitude)
    {
        if (latitude is < -90 or > 90)
            throw new ArgumentOutOfRangeException(nameof(latitude));

        if (longitude is < -180 or > 180)
            throw new ArgumentOutOfRangeException(nameof(longitude));

        Latitude = latitude;
        Longitude = longitude;
    }

    private void SetTimeZone(string? timeZoneId)
    {
        TimeZoneId = Normalize(timeZoneId);
    }

    private void SetCapacity(int? capacity)
    {
        if (capacity is < 0)
            throw new ArgumentException("Capacity cannot be negative.", nameof(capacity));

        Capacity = capacity;
    }

    private static string? Normalize(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
