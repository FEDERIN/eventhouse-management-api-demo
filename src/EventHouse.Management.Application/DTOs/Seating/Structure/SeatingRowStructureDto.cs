namespace EventHouse.Management.Application.DTOs.Seating.Structure;

public sealed class SeatingRowStructureDto
{
    public Guid Id { get; set; }
    public Guid SeatingSectionId { get; set; }
    public int Number { get; set; }
    public string Label { get; set; } = null!;
    public bool IsActive { get; set; }
    public IReadOnlyCollection<SeatDto> Seats { get; set; }
        = [];
}