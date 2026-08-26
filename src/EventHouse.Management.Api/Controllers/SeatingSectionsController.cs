using EventHouse.Management.Api.Common.Errors;
using EventHouse.Management.Api.Contracts.Seating.Sections;
using EventHouse.Management.Api.Mappers.Seating.Sections;
using EventHouse.Management.Api.Swagger;
using EventHouse.Management.Api.Swagger.Examples.Contracts.Seating.Sections;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace EventHouse.Management.Api.Controllers;

[ApiController]
[Route("api/v1/seatingMaps/{seatingMapId:guid}/sections")]
public sealed class SeatingSectionsController(ISender sender)
    : BaseApiController
{
    [HttpPost]
    [SwaggerOperation(
        OperationId = "AddSeatingSection",
        Summary = "Add a seating section to a seating map.")]
    [SwaggerRequestExample(
        typeof(AddSeatingSectionRequest),
        typeof(AddSeatingSectionRequestExample))]
    [ProducesNoContent]
    [ProducesValidationProblem]
    [ProducesNotFoundProblem]
    [ProducesConflictProblem]
    public async Task<IActionResult> Add(
        Guid seatingMapId,
        [FromBody] AddSeatingSectionRequest body,
        CancellationToken ct)
    {
        await sender.Send(
            AddSeatingSectionCommandMapper.FromContract(
                seatingMapId,
                body),
            ct);

        return NoContent();
    }

    [HttpPut("{sectionId:guid}")]
    [SwaggerOperation(
        OperationId = "UpdateSeatingSection",
        Summary = "Update a seating section.")]
    [SwaggerRequestExample(
        typeof(UpdateSeatingSectionRequest),
        typeof(UpdateSeatingSectionRequestExample))]
    [ProducesNoContent]
    [ProducesValidationProblem]
    [ProducesNotFoundProblem]
    [ProducesConflictProblem]
    public async Task<IActionResult> Update(
        Guid seatingMapId,
        Guid sectionId,
        [FromBody] UpdateSeatingSectionRequest body,
        CancellationToken ct)
    {
        await sender.Send(
            UpdateSeatingSectionCommandMapper.FromContract(
                seatingMapId,
                sectionId,
                body),
            ct);

        return NoContent();
    }

    [HttpPatch("{sectionId:guid}/status")]
    [SwaggerOperation(
        OperationId = "UpdateSeatingSectionStatus",
        Summary = "Update the status of a seating section.")]
    [SwaggerRequestExample(
        typeof(UpdateSeatingSectionStatusRequest),
        typeof(UpdateSeatingSectionStatusRequestExample))]
    [ProducesNoContent]
    [ProducesValidationProblem]
    [ProducesNotFoundProblem]
    [ProducesConflictProblem]
    public async Task<IActionResult> UpdateStatus(
        Guid seatingMapId,
        Guid sectionId,
        [FromBody] UpdateSeatingSectionStatusRequest body,
        CancellationToken ct)
    {
        await sender.Send(
            UpdateSeatingSectionStatusCommandMapper.FromContract(
                seatingMapId,
                sectionId,
                body),
            ct);

        return NoContent();
    }
}