using EventHouse.Management.Api.Contracts.Seating.Seats;
using EventHouse.Management.Api.Mappers.Seating.Seats;
using EventHouse.Management.Api.Swagger.Examples.Contracts.Seating.Seats;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace EventHouse.Management.Api.Controllers;

[ApiController]
[Route("api/v1/seatingMaps/{seatingMapId:guid}/sections/{sectionId:guid}/rows/{rowId:guid}/seats")]
public sealed class SeatingSeatsController(ISender sender) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Add a seat",
        Description = "Adds a seat to a seating row.")]
    [SwaggerRequestExample(
        typeof(AddSeatingSeatRequest),
        typeof(AddSeatingSeatRequestExample))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Add(
        Guid seatingMapId,
        Guid sectionId,
        Guid rowId,
        [FromBody] AddSeatingSeatRequest request,
        CancellationToken ct)
    {
        var command =
            AddSeatingSeatCommandMapper.FromContract(
                seatingMapId,
                sectionId,
                rowId,
                request);

        await sender.Send(command, ct);

        return NoContent();
    }

    [HttpPatch("{seatId:guid}/status")]
    [SwaggerOperation(
        Summary = "Update seat status",
        Description = "Activates or deactivates a seat.")]
    [SwaggerRequestExample(
        typeof(UpdateSeatingSeatStatusRequest),
        typeof(UpdateSeatingSeatStatusRequestExample))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateStatus(
        Guid seatingMapId,
        Guid sectionId,
        Guid rowId,
        Guid seatId,
        [FromBody] UpdateSeatingSeatStatusRequest request,
        CancellationToken ct)
    {
        var command =
            UpdateSeatingSeatStatusCommandMapper.FromContract(
                seatingMapId,
                sectionId,
                rowId,
                seatId,
                request);

        await sender.Send(command, ct);

        return NoContent();
    }
}