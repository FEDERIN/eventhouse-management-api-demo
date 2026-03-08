using EventHouse.Management.Api.Contracts.Venues;
using EventHouse.Management.Api.Swagger.Examples.Data;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Requests.Venues;

[ExcludeFromCodeCoverage]
public class GetVenuesRequestExample : IExamplesProvider<GetVenuesRequest>
{
    public GetVenuesRequest GetExamples() => VenueExampleData.Get();
}
