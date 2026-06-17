using EventHouse.Management.Api.Contracts.Events;
using EventHouse.Management.Api.Swagger.Examples.Data;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Requests.Events;


[ExcludeFromCodeCoverage]
internal sealed class GetEventsRequestExample
    : IExamplesProvider<GetEventsRequest>
{
    public GetEventsRequest GetExamples() => EventExampleData.Get();
}
