using EventHouse.Management.Api.Contracts.Venues;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Venues;

[ExcludeFromCodeCoverage]
internal class CreateVenueRequestExample : IExamplesProvider<CreateVenueRequest>
{
    public CreateVenueRequest GetExamples() => VenueExampleData.Create();
}
