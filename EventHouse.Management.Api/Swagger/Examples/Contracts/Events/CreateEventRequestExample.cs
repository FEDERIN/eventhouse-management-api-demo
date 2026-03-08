using EventHouse.Management.Api.Contracts.Events;
using EventHouse.Management.Api.Swagger.Examples.Data;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Events;

[ExcludeFromCodeCoverage]
internal sealed class CreateEventRequestExample : IExamplesProvider<CreateEventRequest>
{
    public CreateEventRequest GetExamples() => EventExampleData.Create();
}
