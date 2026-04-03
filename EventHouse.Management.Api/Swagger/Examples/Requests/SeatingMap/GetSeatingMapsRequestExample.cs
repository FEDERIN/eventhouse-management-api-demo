using EventHouse.Management.Api.Contracts.SeatingMaps;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Requests.SeatingMap;

[ExcludeFromCodeCoverage]
internal sealed class GetSeatingMapsRequestExample : IExamplesProvider<GetSeatingMapsRequest>
{
    public GetSeatingMapsRequest GetExamples() => new()
    {
        VenueId = new Guid("11111111-1111-1111-1111-111111111111"),
        Name = "Main Floor",
        IsActive = true,
    };
}
