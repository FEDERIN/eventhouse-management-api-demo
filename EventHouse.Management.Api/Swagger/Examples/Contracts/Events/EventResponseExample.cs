using EventHouse.Management.Api.Contracts.Events;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;
using Contract = EventHouse.Management.Api.Contracts.Events.Event;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Events;

[ExcludeFromCodeCoverage]
internal sealed class EventResponseExample
    : IExamplesProvider<Contract>
{
    public Contract GetExamples() => new()
    {
        Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
        Name = "Summer Music Festival",
        Description = "An exciting outdoor music festival featuring various artists.",
        Scope = EventScope.International,
    };

}
