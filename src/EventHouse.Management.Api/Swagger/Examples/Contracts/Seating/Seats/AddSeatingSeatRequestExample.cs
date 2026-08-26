using EventHouse.Management.Api.Contracts.Seating.Seats;
using EventHouse.Management.Api.Swagger.Examples.Data;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Seating.Seats;

[ExcludeFromCodeCoverage]
internal sealed class AddSeatingSeatRequestExample : IExamplesProvider<AddSeatingSeatRequest>
{
    public AddSeatingSeatRequest GetExamples()
        => SeatingSeatExampleData.Add();
}