using EventHouse.Management.Api.Contracts.Events;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Events;

[ExcludeFromCodeCoverage]
internal sealed class UpdateEventRequestExample : IExamplesProvider<UpdateEventRequest>
{
    public UpdateEventRequest GetExamples() => new()
    {
        Name = "Summer Music Festival 2024",
        Description = "An exciting outdoor music festival featuring top artists from around the world. Updated for 2024!",
        Scope = EventScope.International
    };
}
