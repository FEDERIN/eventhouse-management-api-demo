
using EventHouse.Management.Application.Common.Enums;

namespace EventHouse.Management.Application.DTOs;

public class EventDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public EventScopeDto Scope { get; init; }
}
