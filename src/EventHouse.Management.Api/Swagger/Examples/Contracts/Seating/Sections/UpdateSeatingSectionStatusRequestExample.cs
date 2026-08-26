using EventHouse.Management.Api.Contracts.Seating.Sections;
using EventHouse.Management.Api.Swagger.Examples.Data;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Seating.Sections;

[ExcludeFromCodeCoverage]
internal sealed class UpdateSeatingSectionStatusRequestExample
    : IExamplesProvider<UpdateSeatingSectionStatusRequest>
{
    public UpdateSeatingSectionStatusRequest GetExamples()
        => SeatingSectionExampleData.UpdateStatus();
}