using EventHouse.Management.Api.Contracts.Events;
using Contract = EventHouse.Management.Api.Contracts.Events.Event;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Events;

public sealed class EventResponseExample
{
    public Contract GetExamples() => new()
    {
        Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
        Name = "Summer Music Festival",
        Description = "An exciting outdoor music festival featuring various artists.",
        Scope = EventScope.International,
    };

}
