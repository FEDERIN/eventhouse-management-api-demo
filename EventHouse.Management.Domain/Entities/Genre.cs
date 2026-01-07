using EventHouse.ShareKernel.Entities;

namespace EventHouse.Management.Domain.Entities;

public class Genre : Entity
{
    public string Name { get; private set; } = string.Empty;

    private Genre() { }

    public Genre(Guid id, string name)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty.", nameof(id));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Genre name is required", nameof(name));

        Id = id;
        Name = name.Trim();
    }

    public void Update(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Genre name is required", nameof(name));

        Name = name.Trim();
    }
}
