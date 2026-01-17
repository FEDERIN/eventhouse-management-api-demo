using EventHouse.Management.Api.Common;
using EventHouse.Management.Api.Common.Errors;
using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.Events;
using EventHouse.Management.Api.Mappers;
using EventHouse.Management.Api.Mappers.Enums;
using EventHouse.Management.Api.Mappers.Events;
using EventHouse.Management.Api.Swagger;
using EventHouse.Management.Application.Commands.Events.Create;
using EventHouse.Management.Application.Commands.Events.Delete;
using EventHouse.Management.Application.Commands.Events.Update;
using EventHouse.Management.Application.Common;
using EventHouse.Management.Application.Queries.Events.GetAll;
using EventHouse.Management.Application.Queries.Events.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHouse.Management.Api.Controllers;

[ApiController]
[Route("api/v1/events")]
[Produces("application/json")]
public sealed class EventsController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    private static readonly string[] BlockingAssociations = ["eventVenueCalendars"];

    /// <summary>List events (paged).</summary>
    [HttpGet]
    [SwaggerOperation(OperationId = "ListEvents")]
    [ProducesOkAttribute<PagedResult<Event>>]
    [ProducesValidationProblemAttribute]
    [ProducesTooManyRequestsProblemAttribute]
    public async Task<ActionResult<PagedResult<Event>>> GetAll(
        [FromQuery] GetEventsRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllEventsQuery
        {
            Name = request.Name,
            Description = request.Description,
            Scope = EventScopeMapper.ToApplicationOptional(request.Scope),
            Page = request.Page,
            PageSize = request.PageSize,
            SortBy = EventSortMapper.ToApplication(request.SortBy),
            SortDirection = SortDirectionMapper.ToApplication(request.SortDirection)
        }, cancellationToken);

        return Ok(EventMapper.ToContract(result, Request));
    }

    /// <summary>Get an event by ID.</summary>
    [HttpGet("{eventId:guid}")]
    [SwaggerOperation(OperationId = "GetEventById")]
    [ProducesOkAttribute<Event>]
    [ProducesNotFoundProblem]
    public async Task<ActionResult<Event>> GetById(Guid eventId, CancellationToken cancellationToken)
    {
        var resultDto = await _mediator.Send(new GetEventByIdQuery(eventId), cancellationToken);

        if (resultDto is null)
            return EventNotFound(eventId);

        return Ok(EventMapper.ToContract(resultDto));
    }

    /// <summary>Creates a new event.</summary>
    [HttpPost]
    [SwaggerOperation(OperationId = "CreateEvent")]
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

    /// <summary>Updates an existing event.</summary>
    [HttpPut("{eventId:guid}")]
    [SwaggerOperation(OperationId = "UpdateEvent")]
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
    [SwaggerOperation(OperationId = "DeleteEvent")]
    [ProducesNoContentAttribute]
    [ProducesNotFoundProblem]
    [ProducesConflictProblem]
    public async Task<IActionResult> Delete(Guid eventId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteEventCommand(eventId), cancellationToken);

        if (result.NotFound)
            return EventNotFound(eventId);

        if (result.HasAssociations)
        {
            return ConflictProblem(
                code: "EVENT_CANNOT_BE_DELETED",
                title: "Event cannot be deleted",
                detail: "This event cannot be deleted because it has associated entities.",
                ext: new Dictionary<string, object?>
                {
                    ["blockingAssociations"] = BlockingAssociations
                }
            );
        }

        return NoContent();
    }

    private ObjectResult EventNotFound(Guid eventId) =>
        NotFoundProblem(
            code: "EVENT_NOT_FOUND",
            title: "Event not found",
            detail: $"No event exists with id '{eventId}'."
        );
}
