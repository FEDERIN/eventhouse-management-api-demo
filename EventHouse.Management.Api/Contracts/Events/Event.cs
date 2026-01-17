using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace EventHouse.Management.Api.Contracts.Events;

public class Event
{
    /// <summary>Event name.</summary>
    /// <example>Summer Fest 2026</example>
    [SwaggerSchema(
        Description = "Event name. Must be between 2 and 200 characters."
    )]
    public string Name { get; init; } = default!;

    /// <summary>Optional event description.</summary>
    /// <example>Annual open-air music festival.</example>
    [SwaggerSchema(
        Description = "Optional event description."
    )]
    public string? Description { get; init; }

    /// <summary>Geographical scope of the event.</summary>
    /// <example>International</example>
    [SwaggerSchema(
        Description = "Defines who can see the event."
    )]
    [EnumDataType(typeof(EventScope))]
    public EventScope Scope { get; init; }
}
