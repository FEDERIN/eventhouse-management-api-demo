namespace EventHouse.Management.Application.DTOs.Seating.Structure;

public sealed class SeatingSectionStructureDto
{
    public Guid Id { get; set; }
    public Guid SeatingMapId { get; set; }
    public string Name { get; set; } = null!;
    public bool IsNumbered { get; set; }
    public int Capacity { get; set; }
    public bool IsActive { get; set; }

    public IReadOnlyCollection<SeatingRowStructureDto> Rows { get; set; }
        = [];
}