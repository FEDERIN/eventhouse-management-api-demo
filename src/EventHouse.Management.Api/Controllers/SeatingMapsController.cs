using EventHouse.Management.Api.Common.Errors;
using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.Seating.Maps;
using EventHouse.Management.Api.Contracts.Seating.Structure;
using EventHouse.Management.Api.Mappers.Seating.Maps;
using EventHouse.Management.Api.Swagger;
using EventHouse.Management.Api.Swagger.Examples.Contracts.Seating.Maps;
using EventHouse.Management.Api.Swagger.Examples.Requests.SeatingMap;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace EventHouse.Management.Api.Controllers;

[ApiController]
[Route("api/v1/seatingMaps")]
public sealed class SeatingMapsController(ISender sender)
    : BaseApiController
{
    #region READ

    [HttpGet("{seatingMapId:guid}")]
    [SwaggerOperation(
        OperationId = "GetSeatingMapById",
        Summary = "Retrieve a specific seating map by its unique identifier.")]
    [SwaggerResponseExample(
        StatusCodes.Status200OK,
        typeof(SeatingMapResponseExample))]
    [ProducesOk<SeatingMapResponse>]
    [ProducesNotFoundProblem]
    public async Task<ActionResult<SeatingMapResponse>> GetById(
        Guid seatingMapId,
        CancellationToken ct)
    {
        var resultDto = await sender.Send(
            GetSeatingMapByIdQueryMapper.FromContract(
                seatingMapId),
            ct);

        return Ok(
            SeatingMapMapper.ToContract(resultDto));
    }

    [HttpGet]
    [SwaggerOperation(
        OperationId = "ListSeatingMaps",
        Summary = "List seating maps with optional filtering, sorting, and pagination.")]
    [SwaggerResponseExample(
        StatusCodes.Status200OK,
        typeof(SeatingMapPagedResultExample))]
    [SwaggerRequestExample(
        typeof(GetSeatingMapsRequest),
        typeof(GetSeatingMapsRequestExample))]
    [ProducesOk<PagedResult<SeatingMapResponse>>]
    [ProducesValidationProblem]
    [ProducesTooManyRequestsProblem]
    public async Task<ActionResult<PagedResult<SeatingMapResponse>>> GetAll(
        [FromQuery] GetSeatingMapsRequest query,
        CancellationToken ct)
    {
        var resultDto = await sender.Send(
            GetAllSeatingMapsQueryMapper.FromContract(query),
            ct);

        return Ok(
            SeatingMapMapper.ToContract(
                resultDto,
                Request));
    }

    [HttpGet("{seatingMapId:guid}/structure")]
    [SwaggerOperation(
        OperationId = "GetSeatingMapStructure",
        Summary = "Retrieve the complete seating structure.")]
    [SwaggerResponseExample(
        StatusCodes.Status200OK,
        typeof(SeatingMapStructureResponseExample))]
    [ProducesOk<SeatingMapStructureResponse>]
    [ProducesNotFoundProblem]
    public async Task<ActionResult<SeatingMapStructureResponse>> GetStructure(
        Guid seatingMapId,
        CancellationToken ct)
    {
        var resultDto = await sender.Send(
            GetSeatingMapStructureQueryMapper.FromContract(
                seatingMapId),
            ct);

        return Ok(
            SeatingMapStructureMapper.ToResponse(resultDto));
    }

    #endregion

    #region WRITE

    [HttpPost]
    [SwaggerOperation(
        OperationId = "CreateSeatingMap",
        Summary = "Create a new seating map in the system.")]
    [SwaggerRequestExample(
        typeof(CreateSeatingMapRequest),
        typeof(CreateSeatingMapRequestExample))]
    [SwaggerResponseExample(
        StatusCodes.Status201Created,
        typeof(SeatingMapResponseExample))]
    [ProducesCreated<SeatingMapResponse>]
    [ProducesValidationProblem]
    [ProducesConflictProblem]
    public async Task<ActionResult<SeatingMapResponse>> Create(
        [FromBody] CreateSeatingMapRequest body,
        CancellationToken ct)
    {
        var createdDto = await sender.Send(
            CreateSeatingMapCommandMapper.FromContract(body),
            ct);

        var created = SeatingMapMapper.ToContract(createdDto);

        return CreatedAtAction(
            nameof(GetById),
            new { seatingMapId = created.Id },
            created);
    }

    [HttpPut("{seatingMapId:guid}")]
    [SwaggerOperation(
        OperationId = "UpdateSeatingMap",
        Summary = "Update an existing seating map's details.")]
    [SwaggerRequestExample(
        typeof(UpdateSeatingMapRequest),
        typeof(UpdateSeatingMapRequestExample))]
    [ProducesNoContent]
    [ProducesValidationProblem]
    [ProducesNotFoundProblem]
    [ProducesConflictProblem]
    public async Task<IActionResult> Update(
        Guid seatingMapId,
        [FromBody] UpdateSeatingMapRequest body,
        CancellationToken ct)
    {
        await sender.Send(
            UpdateSeatingMapCommandMapper.FromContract(
                seatingMapId,
                body),
            ct);

        return NoContent();
    }

    [HttpPatch("{seatingMapId:guid}/status")]
    [SwaggerOperation(
        OperationId = "UpdateSeatingMapStatus",
        Summary = "Activate or deactivate a seating map.")]
    [SwaggerRequestExample(
        typeof(UpdateSeatingMapStatusRequest),
        typeof(UpdateSeatingMapStatusRequestExample))]
    [ProducesNoContent]
    [ProducesValidationProblem]
    [ProducesNotFoundProblem]
    [ProducesConflictProblem]
    public async Task<IActionResult> UpdateStatus(
        Guid seatingMapId,
        [FromBody] UpdateSeatingMapStatusRequest body,
        CancellationToken ct)
    {
        await sender.Send(
            UpdateSeatingMapStatusCommandMapper.FromContract(
                seatingMapId,
                body),
            ct);

        return NoContent();
    }

    #endregion
}