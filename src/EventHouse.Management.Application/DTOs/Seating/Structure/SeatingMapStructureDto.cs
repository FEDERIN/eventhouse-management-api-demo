namespace EventHouse.Management.Application.DTOs.Seating.Structure;

public sealed class SeatingMapStructureDto
{
    public Guid Id { get; set; }
    public Guid VenueId { get; set; }
    public string Name { get; set; } = null!;
    public int Version { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    public IReadOnlyCollection<SeatingSectionStructureDto> Sections { get; set; }
        = [];
}