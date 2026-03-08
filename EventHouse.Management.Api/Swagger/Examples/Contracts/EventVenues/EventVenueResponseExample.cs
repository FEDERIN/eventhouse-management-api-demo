using EventHouse.Management.Api.Contracts.EventVenues;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.EventVenues;

[ExcludeFromCodeCoverage]
internal sealed class EventVenueResponseExample : IExamplesProvider<EventVenueResponse>
{ 
    public EventVenueResponse GetExamples() => EventVenueExampleData.Result();
}
