using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.Events;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Data;

[ExcludeFromCodeCoverage]
internal static class EventExampleData
{
    internal static readonly Guid EventId = ExampleConstants.EventId;
    internal static readonly string Name = ExampleConstants.EventName;
    internal static readonly string Description = "Annual open-air music festival.";
    internal static readonly EventScope Scope = EventScope.International;

    internal static CreateEventRequest Create() => new()
    {
        Name = Name,
        Description = Description,
        Scope = Scope
    };

    internal static UpdateEventRequest Update() => new()
    {
        Name = Name,
        Description = Description,
        Scope = Scope
    };

    internal static EventResponse Result() => new()
    {
        Id = EventId,
        Name = Name,
        Description = Description,
        Scope = Scope
    };

    internal static GetEventsRequest Get() => new()
    {
        Name = Name,
        Description = Description,
        Scope = Scope,
        Page = 1,
        PageSize = 10,
        SortBy = EventSortBy.Name,
        SortDirection = SortDirection.Asc
    };
}
