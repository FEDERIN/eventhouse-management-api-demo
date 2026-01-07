using EventHouse.Management.Domain.Enums;
using EventHouse.ShareKernel.Entities;

namespace EventHouse.Management.Domain.Entities;

public class Event : Entity
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public EventScope Scope { get; private set; } = EventScope.National;

    public Event(Guid id, string name, string? description, EventScope scope = EventScope.National)
    {
        Id = id;
        Name = !string.IsNullOrWhiteSpace(name)
            ? name
            : throw new ArgumentException("Event name is required", nameof(name));

        Description = description;
        Scope = scope;
    }

    public void Update(string name, string? description, EventScope scope)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Event name is required", nameof(name));

        Name = name;
        Description = description;
        Scope = scope;
    }
}
