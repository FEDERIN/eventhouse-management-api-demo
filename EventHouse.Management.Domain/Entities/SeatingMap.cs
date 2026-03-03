
using EventHouse.ShareKernel.Entities;

namespace EventHouse.Management.Domain.Entities;
public sealed class SeatingMap : Entity
{
    public Guid VenueId { get; private set; }
    public string Name { get; private set; } = default!;
    public int Version { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    private SeatingMap() { } // EF

    public SeatingMap(Guid id, Guid venueId, string name, int version, bool isActive)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty.", nameof(id));

        Id = id;
        VenueId = venueId;
        Name = name;
        Version = version;
        IsActive = isActive;
        CreatedAtUtc = DateTime.UtcNow;
    }
}
