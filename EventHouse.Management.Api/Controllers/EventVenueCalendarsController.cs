using EventHouse.Management.Api.Common.Errors;
using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.EventVenueCalendars;
using EventHouse.Management.Api.Mappers.EventVenueCalendars;
using EventHouse.Management.Api.Swagger;
using EventHouse.Management.Api.Swagger.Examples.Contracts.EventVenueCalendars;
using EventHouse.Management.Api.Swagger.Examples.Requests.EventVenueCalendars;
using EventHouse.Management.Application.Queries.EventVenueCalendars.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace EventHouse.Management.Api.Controllers;

[ApiController]
[Route("api/v1/event-venue-calendars")]
public sealed class EventVenueCalendarsController(IMediator mediator) : BaseApiController
{

    #region READ

    [HttpGet("{eventVenueCalendarId:guid}")]
    [SwaggerOperation(
        OperationId = "GetEventVenueCalendarById",
        Summary = "Retrieve a specific event venue calendar by their unique identifier.")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(EventVenueCalendarResponseExample))]
    [ProducesOkAttribute<EventVenueCalendarResponse>]
    [ProducesNotFoundProblem]
    public async Task<ActionResult<EventVenueCalendarResponse>> GetById(
    Guid eventVenueCalendarId,
    CancellationToken cancellationToken)
    {
        var resultDto = await mediator.Send(
            new GetEventVenueCalendarByIdQuery(eventVenueCalendarId),
            cancellationToken);

        return Ok(EventVenueCalendarMapper.ToContract(resultDto));
    }

    [HttpGet]
    [SwaggerOperation(
    OperationId = "ListEventVenueCalendars",
    Summary = "List event venue calendars with optional filtering, sorting, and pagination.")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(EventVenueCalendarPagedResultExample))]
    [SwaggerRequestExample(typeof(GetEventVenueCalendarsRequest), typeof(GetEventVenueCalendarsRequestExample))]
    [ProducesOkAttribute<PagedResult<EventVenueCalendarResponse>>]
    [ProducesValidationProblemAttribute]
    public async Task<ActionResult<PagedResult<EventVenueCalendarResponse>>> GetEventVenueCalendars(
    [FromQuery] GetEventVenueCalendarsRequest request,
    CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            GetAllEventVenueCalendarsQueryMapper.FromContract(request),
            cancellationToken);

        return Ok(EventVenueCalendarMapper.ToContract(result, Request));
    }
    #endregion


    #region WRITE
    [HttpPost]
    [SwaggerOperation(
        OperationId = "CreateEventVenueCalendar",
        Summary = "Create a new event venue calendar in the system.")]
    [SwaggerRequestExample(typeof(CreateEventVenueCalendarRequest), typeof(CreateEventVenueCalendarRequestExample))]
    [ProducesCreated<EventVenueCalendarResponse>]
    [ProducesValidationProblemAttribute]
    [ProducesNotFoundProblem]
    [ProducesConflictProblem]
    public async Task<IActionResult> Create(
        [FromBody] CreateEventVenueCalendarRequest body,
        CancellationToken ct)
    {
        var command = CreateEventVenueCalendarCommandMapper.FromContract(body);
        var createdDto = await mediator.Send(command, ct);
        var createdContract = EventVenueCalendarMapper.ToContract(createdDto);

        return CreatedAtAction(nameof(GetById), new { eventVenueCalendarId = createdContract.Id }, createdContract);
    }

    [HttpPut("{eventVenueCalendarId:guid}")]
    [SwaggerOperation(
        OperationId = "UpdateEventVenueCalendar",
        Summary = "Update an existing event venue calendars details.")]
    [SwaggerRequestExample(typeof(UpdateEventVenueCalendarRequest), typeof(UpdateEventVenueCalendarRequestExample))]
    [ProducesNoContent]
    [ProducesValidationProblemAttribute]
    [ProducesNotFoundProblem]
    [ProducesConflictProblem]
    public async Task<IActionResult> Update(
        Guid eventVenueCalendarId,
        [FromBody] UpdateEventVenueCalendarRequest body,
        CancellationToken ct)
    {
        var command = UpdateEventVenueCalendarCommandMapper.FromContract(eventVenueCalendarId, body);
        await mediator.Send(command, ct);
        return NoContent();
    }
    #endregion
}
