using EventHouse.Management.Api.Contracts.Seating.Rows;
using EventHouse.Management.Api.Mappers.Seating.Rows;
using EventHouse.Management.Api.Swagger.Examples.Contracts.Seating.Rows;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace EventHouse.Management.Api.Controllers;

[ApiController]
[Route("api/v1/seatingMaps/{seatingMapId:guid}/sections/{sectionId:guid}/rows")]
public sealed class SeatingRowsController(ISender sender) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Add a seating row",
        Description = "Adds a row to a seating section.")]
    [SwaggerRequestExample(
        typeof(AddSeatingRowRequest),
        typeof(AddSeatingRowRequestExample))]
    public async Task<IActionResult> Add(
        Guid seatingMapId,
        Guid sectionId,
        AddSeatingRowRequest request,
        CancellationToken ct)
    {
        var command =
            AddSeatingRowCommandMapper.FromContract(
                seatingMapId,
                sectionId,
                request);

        await sender.Send(command, ct);

        return NoContent();
    }

    [HttpPatch("{rowId:guid}/status")]
    [SwaggerOperation(
        Summary = "Update seating row status",
        Description = "Activates or deactivates a seating row.")]
    [SwaggerRequestExample(
        typeof(UpdateSeatingRowStatusRequest),
        typeof(UpdateSeatingRowStatusRequestExample))]
    public async Task<IActionResult> UpdateStatus(
        Guid seatingMapId,
        Guid sectionId,
        Guid rowId,
        UpdateSeatingRowStatusRequest request,
        CancellationToken ct)
    {
        var command =
            UpdateSeatingRowStatusCommandMapper.FromContract(
                seatingMapId,
                sectionId,
                rowId,
                request);

        await sender.Send(command, ct);

        return NoContent();
    }
}