using EventHouse.Management.Domain.Enums;
using EventHouse.ShareKernel.Entities;

namespace EventHouse.Management.Domain.Entities;

public class Artist : Entity
{
    public string Name { get; private set; } = null!;
    public ArtistCategory Category { get; private set; }

    private Artist() { }

    public Artist(Guid id, string name, ArtistCategory category)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty.", nameof(id));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));

        Id = id;
        Name = name.Trim();
        Category = category;
    }

    public void Update(string name, ArtistCategory category)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));

        Name = name.Trim();
        Category = category;
    }
}
