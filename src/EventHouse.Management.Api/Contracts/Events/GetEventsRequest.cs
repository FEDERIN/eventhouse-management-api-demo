using EventHouse.Management.Api.Contracts.Common;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EventHouse.Management.Api.Contracts.Events;
public sealed record GetEventsRequest : SortablePaginationRequest<EventSortBy>
{
    /// <summary>Filter events by name (contains match).</summary>
    [FromQuery(Name = "name")]
    public string? Name { get; init; }

    /// <summary>Filter events by description (contains match).</summary>
    [FromQuery(Name = "description")]
    public string? Description { get; init; }

    /// <summary>Filter events by scope.</summary>
    [FromQuery(Name = "scope")]
    [EnumDataType(typeof(EventScope))]
    public EventScope? Scope { get; init; }
}

