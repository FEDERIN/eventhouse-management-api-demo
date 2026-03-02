using EventHouse.Management.Api.Contracts.Venues;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Venues;

[ExcludeFromCodeCoverage]
internal class UpdateVenueRequestExample : IExamplesProvider<UpdateVenueRequest>
{
    public UpdateVenueRequest GetExamples() => VenueExampleData.Update();
}

