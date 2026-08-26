namespace EventHouse.Management.Api.Contracts.Seating.Structure;

public sealed class SeatingMapStructureResponse
{
    public Guid Id { get; set; }
    public Guid VenueId { get; set; }
    public string Name { get; set; } = null!;
    public int Version { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    public IReadOnlyCollection<SeatingSectionStructureResponse> Sections { get; set; }
        = [];
}