namespace EventHouse.Management.Application.DTOs.Seating;

public sealed class SeatDto
{
    public Guid Id { get; set; }
    public Guid SeatingRowId { get; set; }
    public int Number { get; set; }
    public string Label { get; set; } = null!;
    public bool IsActive { get; set; }
}