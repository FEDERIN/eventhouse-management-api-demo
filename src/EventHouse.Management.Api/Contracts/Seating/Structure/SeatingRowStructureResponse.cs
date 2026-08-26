using EventHouse.Management.Api.Contracts.Seating.Seats;

namespace EventHouse.Management.Api.Contracts.Seating.Structure;

public sealed class SeatingRowStructureResponse
{
    public Guid Id { get; set; }
    public int Number { get; set; }
    public string Label { get; set; } = null!;

    public bool IsActive { get; set; }
    public IReadOnlyCollection<SeatResponse> Seats { get; set; }
        = [];
}