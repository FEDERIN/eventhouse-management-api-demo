namespace EventHouse.Management.Api.Contracts.Seating.Seats;

public sealed class SeatResponse
{
    public Guid Id { get; set; }
    public int Number { get; set; }
    public string Label { get; set; } = null!;
    public bool IsActive { get; set; }
}