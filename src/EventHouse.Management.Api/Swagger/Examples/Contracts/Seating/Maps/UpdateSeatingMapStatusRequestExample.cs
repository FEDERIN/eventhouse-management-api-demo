using EventHouse.Management.Api.Contracts.Seating.Maps;
using EventHouse.Management.Api.Swagger.Examples.Data;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Seating.Maps;

[ExcludeFromCodeCoverage]
internal sealed class UpdateSeatingMapStatusRequestExample
    : IExamplesProvider<UpdateSeatingMapStatusRequest>
{
    public UpdateSeatingMapStatusRequest GetExamples()
        => SeatingMapExampleData.UpdateStatus();
}