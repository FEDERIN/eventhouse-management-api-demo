using EventHouse.Management.Api.Common.Errors;
using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.EventVenues;
using EventHouse.Management.Api.Mappers.EventVenues;
using EventHouse.Management.Api.Swagger;
using EventHouse.Management.Api.Swagger.Examples.Contracts.EventVenues;
using EventHouse.Management.Api.Swagger.Examples.Requests.EventVenues;
using EventHouse.Management.Application.Commands.EventVenues.Create;
using EventHouse.Management.Application.Commands.EventVenues.UpdateStatus;
using EventHouse.Management.Application.Queries.EventVenues.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace EventHouse.Management.Api.Controllers;

[ApiController]
[Route("api/v1/event-venues")]
public sealed class EventVenuesController(IMediator mediator) : BaseApiController
{
    #region READ

    [HttpGet("{eventVenueId:guid}")]
    [SwaggerOperation(
        OperationId = "GetEventVenueById",
        Summary = "Retrieve a specific event venue by their unique identifier."
        )]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(EventVenueResponseExample))]
    [ProducesOkAttribute<EventVenueResponse>]
    [ProducesNotFoundProblem]
    public async Task<ActionResult<EventVenueResponse>> GetById(
        Guid eventVenueId,
        CancellationToken cancellationToken)
    {
        var resultDto = await mediator.Send(new GetEventVenueByIdQuery(eventVenueId), cancellationToken);

        return Ok(EventVenueMapper.ToContract(resultDto));
    }

    [HttpGet]
    [SwaggerOperation(
        OperationId = "ListEventVenues",
        Summary = "List event venues with optional filtering, sorting, and pagination.")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(EventVenuePagedResultExample))]
    [SwaggerRequestExample(typeof(GetEventVenuesRequest), typeof(GetEventVenuesRequestExample))]
    [ProducesOkAttribute<PagedResult<EventVenueResponse>>]
    [ProducesValidationProblemAttribute]
    [ProducesTooManyRequestsProblemAttribute]
    public async Task<ActionResult<PagedResult<EventVenueResponse>>> GetAll(
    [FromQuery] GetEventVenuesRequest query,
    CancellationToken cancellationToken)
    {
        var resultDto = await mediator.Send(GetAllEventVenuesQueryMapper.FromContract(query), cancellationToken);

        return Ok(EventVenueMapper.ToContract(resultDto, Request));
    }
    #endregion

    #region WRITE
    [HttpPost]
    [SwaggerOperation(
        OperationId = "CreateEventVenue",
        Summary = "Create a new event venue in the system.")]
    [SwaggerRequestExample(typeof(CreateEventVenueRequest), typeof(CreateEventVenueRequestExample))]
    [ProducesCreated<EventVenueResponse>]
    [ProducesValidationProblemAttribute]
    [ProducesConflictProblem]
    public async Task<ActionResult<EventVenueResponse>> Create(
        [FromBody] CreateEventVenueRequest body,
        CancellationToken cancellationToken)
    {
        var command = new CreateEventVenueCommand(
            body.EventId,
            body.VenueId,
            EventVenueStatusMapper.ToApplicationRequired(body.Status));

        var createdDto = await mediator.Send(command, cancellationToken);

        var createdContract = EventVenueMapper.ToContract(createdDto);

        return CreatedAtAction(nameof(GetById), new { eventVenueId = createdContract.Id }, createdContract);
    }

    [HttpPut("{eventVenueId:guid}/status")]
    [SwaggerOperation(
        OperationId = "UpdateEventVenueStatus",
        Summary = "Update an existing event venues details.")]
    [SwaggerRequestExample(typeof(UpdateEventVenueStatusRequest), typeof(UpdateEventVenueStatusRequestExample))]
    [ProducesNoContentAttribute]
    [ProducesNotFoundProblem]
    [ProducesValidationProblemAttribute]
    public async Task<IActionResult> UpdateStatus(
        Guid eventVenueId,
        [FromBody] UpdateEventVenueStatusRequest body,
        CancellationToken cancellationToken)
    {
        await mediator.Send(
            new UpdateEventVenueStatusCommand(
                eventVenueId,
                EventVenueStatusMapper.ToApplicationRequired(body.Status)),
            cancellationToken);

        return NoContent();
    }
    #endregion

}
