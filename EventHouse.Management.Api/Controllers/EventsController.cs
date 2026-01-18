using EventHouse.Management.Api.Common.Errors;
using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.Events;
using EventHouse.Management.Api.Mappers.Enums;
using EventHouse.Management.Api.Mappers.Events;
using EventHouse.Management.Api.Swagger;
using EventHouse.Management.Api.Swagger.Examples.Contracts.Events;
using EventHouse.Management.Api.Swagger.Examples.Requests.Events;
using EventHouse.Management.Application.Commands.Events.Create;
using EventHouse.Management.Application.Commands.Events.Delete;
using EventHouse.Management.Application.Commands.Events.Update;
using EventHouse.Management.Application.Common;
using EventHouse.Management.Application.Queries.Events.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace EventHouse.Management.Api.Controllers;

[ApiController]
[Route("api/v1/events")]
public sealed class EventsController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [SwaggerOperation(
        OperationId = "ListEvents",
        Summary = "List events with optional filtering, sorting, and pagination."
        )]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(EventPagedResultExample))]
    [SwaggerRequestExample(typeof(GetEventsRequest), typeof(GetEventsRequestExample))]
    [ProducesOkAttribute<PagedResult<Event>>]
    [ProducesValidationProblemAttribute]
    [ProducesTooManyRequestsProblemAttribute]
    public async Task<ActionResult<PagedResult<Event>>> GetAll(
        [FromQuery] GetEventsRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            GetAllEventsQueryMapper.FromContract(request),
            cancellationToken
        );

        return Ok(EventMapper.ToContract(result, Request));
    }

    [HttpGet("{eventId:guid}")]
    [SwaggerOperation(
        OperationId = "GetEventById",
        Summary = "Retrieve a specific event by its unique identifier."
        )]
    [ProducesOkAttribute<Event>]
    [ProducesNotFoundProblem]
    public async Task<ActionResult<Event>> GetById(Guid eventId, CancellationToken cancellationToken)
    {
        var resultDto = await _mediator.Send(new GetEventByIdQuery(eventId), cancellationToken);

        if (resultDto is null)
            return EventNotFound(eventId);

        return Ok(EventMapper.ToContract(resultDto));
    }

    [HttpPost]
    [SwaggerOperation(
        OperationId = "CreateEvent",
        Summary = "Create a new event in the system."
        )]
    [SwaggerRequestExample(typeof(CreateEventRequest), typeof(CreateEventRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status201Created, typeof(EventResponseExample))]
    [ProducesCreated<Event>]
    [ProducesValidationProblemAttribute]
    [ProducesConflictProblem]
    public async Task<ActionResult<Event>> Create([FromBody] CreateEventRequest body, CancellationToken cancellationToken)
    {
        var createdEventDto = await _mediator.Send(
            new CreateEventCommand(body.Name, body.Description, EventScopeMapper.ToApplicationRequired(body.Scope)),
            cancellationToken);

        var createdEventContract = EventMapper.ToContract(createdEventDto);

        return CreatedAtAction(nameof(GetById), new { eventId = createdEventContract.Id }, createdEventContract);
    }

    [HttpPut("{eventId:guid}")]
    [SwaggerOperation(
        OperationId = "UpdateEvent",
        Summary = "Update an existing event in the system."
        )]
    [SwaggerRequestExample(typeof(UpdateEventRequest), typeof(UpdateEventRequestExample))]
    [ProducesNoContentAttribute]
    [ProducesValidationProblemAttribute]
    [ProducesNotFoundProblem]
    [ProducesConflictProblem]
    public async Task<IActionResult> Update(Guid eventId, [FromBody] UpdateEventRequest body, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new UpdateEventCommand(eventId, body.Name, body.Description, EventScopeMapper.ToApplicationRequired(body.Scope)),
            cancellationToken);

        return result switch
        {
            UpdateResult.NotFound => EventNotFound(eventId),
            UpdateResult.InvalidState => ConflictProblem(
                code: "EVENT_INVALID_STATE",
                title: "Invalid event state",
                detail: "The event cannot be updated in its current state."
            ),
            _ => NoContent()
        };
    }

    /// <summary>Deletes an event by ID.</summary>
    [HttpDelete("{eventId:guid}")]
    [SwaggerOperation(
        OperationId = "DeleteEvent",
        Summary = "Delete an existing event from the system."
        )]
    [ProducesNoContentAttribute]
    [ProducesNotFoundProblem]
    [ProducesConflictProblem]
    public async Task<IActionResult> Delete(Guid eventId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteEventCommand(eventId), cancellationToken);

        if (result.NotFound)
            return EventNotFound(eventId);

        return NoContent();
    }

    private ObjectResult EventNotFound(Guid eventId) =>
        NotFoundProblem(
            code: "EVENT_NOT_FOUND",
            title: "Event not found",
            detail: $"No event exists with id '{eventId}'."
        );
}
