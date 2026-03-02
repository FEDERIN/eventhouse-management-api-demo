using EventHouse.Management.Api.Contracts.Events;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Events;

[ExcludeFromCodeCoverage]
internal sealed class CreateEventRequestExample : IExamplesProvider<CreateEventRequest>
{
    public CreateEventRequest GetExamples() => new()
    {
        Name = "Summer Music Festival",
        Description = "An exciting outdoor music festival featuring top artists from around the world.",
        Scope = EventScope.National
    };
}
