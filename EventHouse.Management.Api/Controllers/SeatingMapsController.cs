using EventHouse.Management.Api.Common.Errors;
using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.SeatingMaps;
using EventHouse.Management.Api.Mappers.SeatingMaps;
using EventHouse.Management.Api.Swagger;
using EventHouse.Management.Api.Swagger.Examples.Contracts.SeatingMap;
using EventHouse.Management.Api.Swagger.Examples.Requests.SeatingMap;
using EventHouse.Management.Application.Commands.SeatingMaps.Create;
using EventHouse.Management.Application.Commands.SeatingMaps.Delete;
using EventHouse.Management.Application.Commands.SeatingMaps.Update;
using EventHouse.Management.Application.Queries.SeatingMaps.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace EventHouse.Management.Api.Controllers;

[ApiController]
[Route("api/v1/seatingMaps")]
public sealed class SeatingMapsController(IMediator mediator) : BaseApiController
{
    #region READ
    [HttpGet("{seatingMapId:guid}")]
    [SwaggerOperation(
        OperationId = "GetSeatingMapById",
        Summary = "Retrieve a specific seating map by their unique identifier."
        )]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SeatingMapResponseExample))]
    [ProducesOkAttribute<SeatingMapResponse>]
    [ProducesNotFoundProblem]
    public async Task<ActionResult<SeatingMapResponse>> GetById(Guid seatingMapId, CancellationToken cancellationToken)
    {
        var resultDto = await mediator.Send(new GetSeatingMapByIdQuery(seatingMapId), cancellationToken);
        return Ok(SeatingMapMapper.ToContract(resultDto));
    }

    [HttpGet]
    [SwaggerOperation(
    OperationId = "ListSeatingMaps",
    Summary = "List seating maps with optional filtering, sorting, and pagination."
    )]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SeatingMapPagedResultExample))]
    [SwaggerRequestExample(typeof(GetSeatingMapsRequest), typeof(GetSeatingMapsRequestExample))]
    [ProducesOkAttribute<PagedResult<SeatingMapResponse>>]
    [ProducesValidationProblemAttribute]
    [ProducesTooManyRequestsProblemAttribute]
    public async Task<ActionResult<PagedResult<SeatingMapResponse>>> GetAll(
    [FromQuery] GetSeatingMapsRequest query,
    CancellationToken cancellationToken)
    {
        var resultDto = await mediator.Send(

            GetAllSeatingMapsQueryMapper.FromContract(query),
            cancellationToken);

        return Ok(SeatingMapMapper.ToContract(resultDto, Request));
    }
    #endregion

    #region WRITE
    [HttpPost]
    [SwaggerOperation(
        OperationId = "CreateSeatingMap",
        Summary = "Create a new seating map in the system."
    )]
    [SwaggerRequestExample(typeof(CreateSeatingMapRequest), typeof(CreateSeatingMapRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status201Created, typeof(SeatingMapResponseExample))]
    [ProducesCreated<SeatingMapResponse>]
    [ProducesValidationProblemAttribute]
    [ProducesConflictProblem]
    public async Task<ActionResult<SeatingMapResponse>> Create([FromBody] CreateSeatingMapRequest body, CancellationToken cancellationToken)
    {
        var command = new CreateSeatingMapCommand(
                body.VenueId,
                body.Name,
                Version: 1,
                body.IsActive
            );

        var createdDto = await mediator.Send(command, cancellationToken);
        var created = SeatingMapMapper.ToContract(createdDto);

        return CreatedAtAction(nameof(GetById), new { seatingMapId = created.Id }, created);
    }

    [HttpPut("{seatingMapId:guid}")]
    [SwaggerOperation(OperationId = "UpdateSeatingMap",
    Summary = "Update an existing seating maps details."
    )]
    [SwaggerRequestExample(typeof(UpdateSeatingMapRequest), typeof(UpdateSeatingMapRequestExample))]
    [ProducesNoContentAttribute]
    [ProducesValidationProblemAttribute]
    [ProducesNotFoundProblem]
    [ProducesConflictProblem]
    public async Task<IActionResult> Update(Guid seatingMapId, [FromBody] UpdateSeatingMapRequest body, CancellationToken cancellationToken)
    {
        await mediator.Send(new UpdateSeatingMapCommand(
            seatingMapId,
            body.Name,
            body.Version,
            body.IsActive
            ), cancellationToken);

        return NoContent();
    }
    #endregion

    #region DELETE
    [HttpDelete("{seatingMapId:guid}")]
    [SwaggerOperation(OperationId = "DeleteSeatingMap",
    Summary = "Delete a seating map from the system."
    )]
    [ProducesNoContentAttribute]
    [ProducesNotFoundProblem]
    [ProducesConflictProblem]
    public async Task<IActionResult> Delete(Guid seatingMapId, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteSeatingMapCommand(seatingMapId), cancellationToken);

        return NoContent();
    }
    #endregion
}