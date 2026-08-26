using EventHouse.Management.Api.Contracts.Seating.Rows;
using EventHouse.Management.Api.Swagger.Examples.Data;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Seating.Rows;

[ExcludeFromCodeCoverage]
internal sealed class UpdateSeatingRowStatusRequestExample
    : IExamplesProvider<UpdateSeatingRowStatusRequest>
{
    public UpdateSeatingRowStatusRequest GetExamples()
        => SeatingRowExampleData.UpdateStatus();
}