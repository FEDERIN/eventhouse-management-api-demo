using EventHouse.Management.Api.Contracts.Common;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EventHouse.Management.Api.Contracts.Events;
public sealed record GetEventsRequest : SortablePaginationRequest<EventSortBy>
{
    /// <summary>Filter events by name (contains match).</summary>
    /// <example>Summer Fest</example>
    [FromQuery(Name = "name")]
    public string? Name { get; init; }

    /// <summary>Filter events by description (contains match).</summary>
    /// <example>music festival</example>
    [FromQuery(Name = "description")]
    public string? Description { get; init; }

    /// <summary>Filter events by scope.</summary>
    // <example>International</example>
    [FromQuery(Name = "scope")]
    [EnumDataType(typeof(EventScope))]
    public EventScope? Scope { get; init; }
}

