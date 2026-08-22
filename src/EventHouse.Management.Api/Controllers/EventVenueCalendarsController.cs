using EventHouse.Management.Api.Common.Errors;
using EventHouse.Management.Api.Contracts.ArtistPerformances;
using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.EventVenueCalendars;
using EventHouse.Management.Api.Mappers.ArtistPerformances;
using EventHouse.Management.Api.Mappers.EventVenueCalendars;
using EventHouse.Management.Api.Swagger;
using EventHouse.Management.Api.Swagger.Examples.Contracts.ArtistPerformances;
using EventHouse.Management.Api.Swagger.Examples.Contracts.EventVenueCalendars;
using EventHouse.Management.Api.Swagger.Examples.Requests.ArtistPerformances;
using EventHouse.Management.Api.Swagger.Examples.Requests.EventVenueCalendars;
using EventHouse.Management.Application.Common.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace EventHouse.Management.Api.Controllers;

[ApiController]
[Route("api/v1/event-venue-calendars")]
public sealed class EventVenueCalendarsController(ISender sender) : BaseApiController
{

    #region READ

    [HttpGet("{eventVenueCalendarId:guid}")]
    [SwaggerOperation(
        OperationId = "GetEventVenueCalendarById",
        Summary = "Retrieve a specific event venue calendar by their unique identifier.")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(EventVenueCalendarResponseExample))]
    [ProducesOk<EventVenueCalendarResponse>]
    [ProducesNotFoundProblem]
    public async Task<ActionResult<EventVenueCalendarResponse>> GetById(
    Guid eventVenueCalendarId,
    CancellationToken ct)
    {
        var resultDto = await sender.Send(
            GetEventVenueCalendarByIdQueryMapper.FromContract(eventVenueCalendarId),
            ct);

        return Ok(EventVenueCalendarMapper.ToContract(resultDto));
    }

    [HttpGet]
    [SwaggerOperation(
    OperationId = "ListEventVenueCalendars",
    Summary = "List event venue calendars with optional filtering, sorting, and pagination.")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(EventVenueCalendarPagedResultExample))]
    [SwaggerRequestExample(typeof(GetEventVenueCalendarsRequest), typeof(GetEventVenueCalendarsRequestExample))]
    [ProducesOk<PagedResult<EventVenueCalendarResponse>>]
    [ProducesValidationProblem]
    public async Task<ActionResult<PagedResult<EventVenueCalendarResponse>>> GetEventVenueCalendars(
    [FromQuery] GetEventVenueCalendarsRequest request,
    CancellationToken ct)
    {
        var result = await sender.Send(
            GetAllEventVenueCalendarsQueryMapper.FromContract(request),
            ct);

        return Ok(EventVenueCalendarMapper.ToContract(result, Request));
    }

    [HttpGet("{eventVenueCalendarId:guid}/artist-performances")]
    [SwaggerOperation(
        OperationId = "ListArtistPerformancesForCalendar",
        Summary = "Retrieve the line-up for a specific calendar slot.")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ArtistPerformancePagedResultExample))]
    [SwaggerRequestExample(typeof(GetArtistPerformancesRequest), typeof(GetArtistPerformancesRequestExample))]
    [ProducesOk<PagedResultDto<ArtistPerformanceResponse>>]
    [ProducesResponseType(typeof(EventHouseProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PagedResultDto<ArtistPerformanceResponse>>> GetArtistPerformances(
    Guid eventVenueCalendarId,
    [FromQuery] GetArtistPerformancesRequest request,
    CancellationToken ct = default)
    {
        var result = await sender.Send(
            GetAllArtistPerformancesQueryMapper.FromContract(eventVenueCalendarId, request),
            ct);

        return Ok(ArtistPerformanceMapper.ToContract(result, Request));
    }
    #endregion


    #region WRITE
    [HttpPost]
    [SwaggerOperation(
        OperationId = "CreateEventVenueCalendar",
        Summary = "Create a new event venue calendar in the system.")]
    [SwaggerRequestExample(typeof(CreateEventVenueCalendarRequest), typeof(CreateEventVenueCalendarRequestExample))]
    [ProducesCreated<EventVenueCalendarResponse>]
    [ProducesValidationProblem]
    [ProducesNotFoundProblem]
    [ProducesConflictProblem]
    public async Task<IActionResult> Create(
        [FromBody] CreateEventVenueCalendarRequest body,
        CancellationToken ct = default)
    {
        var createdDto = await sender.Send(
            CreateEventVenueCalendarCommandMapper.FromContract(body),
            ct);

        var createdContract = EventVenueCalendarMapper.ToContract(createdDto);

        return CreatedAtAction(nameof(GetById), new { eventVenueCalendarId = createdContract.Id }, createdContract);
    }

    [HttpPost("{eventVenueCalendarId:guid}/artist-performances")]
    [SwaggerOperation(
    OperationId = "CreateArtistPerformanceForCalendar",
    Summary = "Creates an artist performance for a calendar.")]
    [SwaggerResponseExample(StatusCodes.Status201Created, typeof(ArtistPerformanceResponseExample))]
    [SwaggerRequestExample(typeof(CreateArtistPerformanceRequest), typeof(CreateArtistPerformanceRequestExample))]
    [ProducesCreated<ArtistPerformanceResponse>]
    [ProducesValidationProblem]
    [ProducesNotFoundProblem]
    [ProducesConflictProblem]
    public async Task<IActionResult> CreateArtistPerformance(
    Guid eventVenueCalendarId,
    [FromBody] CreateArtistPerformanceRequest body,
    CancellationToken ct = default)
    {
        var createDto = await sender.Send(
            AddArtistPerformanceCommandMapper.FromContract(eventVenueCalendarId, body),
            ct);

        var created = ArtistPerformanceMapper.ToContract(createDto);

        return CreatedAtAction(
                    nameof(ArtistPerformancesController.GetById),
                    "ArtistPerformances",
                    new { id = created.Id },
                    created);
    }

    [HttpPut("{eventVenueCalendarId:guid}")]
    [SwaggerOperation(
        OperationId = "UpdateEventVenueCalendar",
        Summary = "Update an existing event venue calendars details.")]
    [SwaggerRequestExample(typeof(UpdateEventVenueCalendarRequest), typeof(UpdateEventVenueCalendarRequestExample))]
    [ProducesNoContent]
    [ProducesValidationProblem]
    [ProducesNotFoundProblem]
    [ProducesConflictProblem]
    public async Task<IActionResult> Update(
        Guid eventVenueCalendarId,
        [FromBody] UpdateEventVenueCalendarRequest body,
        CancellationToken ct = default)
    {
        await sender.Send(
            UpdateEventVenueCalendarCommandMapper.FromContract(eventVenueCalendarId, body),
            ct);

        return NoContent();
    }

    [HttpPatch("{eventVenueCalendarId:guid}/artist-performances/{artistId:guid}/times")]
    [SwaggerOperation(
    OperationId = "UpdateArtistPerformanceTimes",
    Summary = "Update set times for an artist performance.",
    Description = "Updates the start and end times. Validates overlaps and calendar boundaries.")]
    [ProducesNoContent]
    [ProducesNotFoundProblem]
    [ProducesConflictProblem]
    public async Task<IActionResult> UpdatePerformanceTimes(
    Guid eventVenueCalendarId,
    Guid artistId,
    [FromBody] UpdatePerformanceDatesRequest request,
    CancellationToken ct = default)
    {
        await sender.Send(
            UpdatePerformanceDatesCommandMapper.FromContract(eventVenueCalendarId, artistId, request),
            ct);

        return NoContent();
    }

    [HttpPatch("{eventVenueCalendarId:guid}/artist-performances/swap-headliner")]
    [SwaggerOperation(
        OperationId = "SwapArtistHeadliner",
        Summary = "Atomic swap of the headliner role.")]
    [SwaggerRequestExample(typeof(SwapHeadlinerRequest), typeof(SwapHeadlinerRequestExample))]
    [ProducesNoContent]
    [ProducesValidationProblem]
    [ProducesNotFoundProblem]
    [ProducesConflictProblem]
    public async Task<IActionResult> SwapHeadliner(
        Guid eventVenueCalendarId,
        [FromBody] SwapHeadlinerRequest body,
        CancellationToken ct = default)
    {
        await sender.Send(
            SwapHeadlinerCommandMapper.FromContract(eventVenueCalendarId, body),
            ct);

        return NoContent();
    }
    #endregion
}
