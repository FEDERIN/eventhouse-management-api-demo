using EventHouse.Management.Api.Contracts.EventVenues;
using EventHouse.Management.Api.Swagger.Examples.Data;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Requests.EventVenues;

[ExcludeFromCodeCoverage]
internal sealed class GetEventVenuesRequestExample : IExamplesProvider<GetEventVenuesRequest>
{
    public GetEventVenuesRequest GetExamples() => EventVenueExampleData.Get();
}
