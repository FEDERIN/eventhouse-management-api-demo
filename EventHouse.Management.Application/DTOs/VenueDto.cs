
namespace EventHouse.Management.Application.DTOs;
public class VenueDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Region { get; set; }
    public string? CountryCode { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? TimeZoneId { get; set; }
    public int? Capacity { get; set; }
    public bool IsActive { get; set; } = true;
}
