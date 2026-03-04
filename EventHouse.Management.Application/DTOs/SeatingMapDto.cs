
namespace EventHouse.Management.Application.DTOs;

public sealed class SeatingMapDto
{
    public Guid Id { get; set; }
    public Guid VenueId { get; set; }
    public string Name { get; set; } = null!;
    public int Version { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
