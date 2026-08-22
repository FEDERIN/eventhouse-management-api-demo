using EventHouse.Management.Api.Common.Errors;
using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.Events;
using EventHouse.Management.Api.Mappers.Events;
using EventHouse.Management.Api.Swagger;
using EventHouse.Management.Api.Swagger.Examples.Contracts.Events;
using EventHouse.Management.Api.Swagger.Examples.Requests.Events;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace EventHouse.Management.Api.Controllers;

[ApiController]
[Route("api/v1/events")]
public sealed class EventsController(ISender sender) : BaseApiController
{
    [HttpGet]
    [SwaggerOperation(
        OperationId = "ListEvents",
        Summary = "List events with optional filtering, sorting, and pagination."
        )]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(EventPagedResultExample))]
    [SwaggerRequestExample(typeof(GetEventsRequest), typeof(GetEventsRequestExample))]
    [ProducesOk<PagedResult<EventResponse>>]
    [ProducesValidationProblem]
    [ProducesTooManyRequestsProblem]
    public async Task<ActionResult<PagedResult<EventResponse>>> GetAll(
        [FromQuery] GetEventsRequest request,
        CancellationToken ct)
    {
        var result = await sender.Send(
            GetAllEventsQueryMapper.FromContract(request),
            ct
        );

        return Ok(EventMapper.ToContract(result, Request));
    }

    [HttpGet("{eventId:guid}")]
    [SwaggerOperation(
        OperationId = "GetEventById",
        Summary = "Retrieve a specific event by its unique identifier."
        )]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(EventResponseExample))]
    [ProducesOk<EventResponse>]
    [ProducesNotFoundProblem]
    public async Task<ActionResult<EventResponse>> GetById(Guid eventId, CancellationToken ct)
    {
        var resultDto = await sender.Send(
            GetEventByIdQueryMapper.FromContract(eventId),
            ct);

        return Ok(EventMapper.ToContract(resultDto));
    }

    [HttpPost]
    [SwaggerOperation(
        OperationId = "CreateEvent",
        Summary = "Create a new event in the system."
        )]
    [SwaggerRequestExample(typeof(CreateEventRequest), typeof(CreateEventRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status201Created, typeof(EventResponseExample))]
    [ProducesCreated<EventResponse>]
    [ProducesValidationProblem]
    [ProducesConflictProblem]
    public async Task<ActionResult<EventResponse>> Create([FromBody] CreateEventRequest body, CancellationToken ct)
    {
        var createdEventDto = await sender.Send(
            CreateEventCommandMapper.FromContract(body),
            ct);

        var createdEventContract = EventMapper.ToContract(createdEventDto);

        return CreatedAtAction(nameof(GetById), new { eventId = createdEventContract.Id }, createdEventContract);
    }

    [HttpPut("{eventId:guid}")]
    [SwaggerOperation(
        OperationId = "UpdateEvent",
        Summary = "Update an existing event in the system."
        )]
    [SwaggerRequestExample(typeof(UpdateEventRequest), typeof(UpdateEventRequestExample))]
    [ProducesNoContent]
    [ProducesValidationProblem]
    [ProducesNotFoundProblem]
    [ProducesConflictProblem]
    public async Task<IActionResult> Update(Guid eventId, [FromBody] UpdateEventRequest body, CancellationToken ct)
    {
        await sender.Send(
            UpdateEventCommandMapper.FromContract(eventId, body),
            ct);

        return NoContent();
    }

    [HttpDelete("{eventId:guid}")]
    [SwaggerOperation(
        OperationId = "DeleteEvent",
        Summary = "Delete an existing event from the system."
        )]
    [ProducesNoContent]
    [ProducesNotFoundProblem]
    [ProducesConflictProblem]
    public async Task<IActionResult> Delete(Guid eventId, CancellationToken ct)
    {
        await sender.Send(
            DeleteEventCommandMapper.FromContract(eventId),
            ct);

        return NoContent();
    }   
}