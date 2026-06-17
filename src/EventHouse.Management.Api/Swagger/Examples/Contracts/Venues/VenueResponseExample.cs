using EventHouse.Management.Api.Contracts.Venues;
using EventHouse.Management.Api.Swagger.Examples.Data;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Venues;

[ExcludeFromCodeCoverage]
internal class VenueResponseExample : IExamplesProvider<VenueResponse>
{
    public VenueResponse GetExamples() => VenueExampleData.Result();
}