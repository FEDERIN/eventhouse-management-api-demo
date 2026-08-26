namespace EventHouse.Management.Api.Contracts.Seating.Structure;

public sealed class SeatingSectionStructureResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public bool IsNumbered { get; set; }
    public int Capacity { get; set; }
    public bool IsActive { get; set; }
    public IReadOnlyCollection<SeatingRowStructureResponse> Rows { get; set; }
        = [];
}