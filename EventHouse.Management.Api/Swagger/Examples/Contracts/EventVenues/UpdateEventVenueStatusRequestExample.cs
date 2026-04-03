using EventHouse.Management.Api.Contracts.EventVenues;
using EventHouse.Management.Api.Swagger.Examples.Data;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.EventVenues;

[ExcludeFromCodeCoverage]
internal class UpdateEventVenueStatusRequestExample : IExamplesProvider<UpdateEventVenueStatusRequest>
{
    public UpdateEventVenueStatusRequest GetExamples() => EventVenueExampleData.Update();
}
