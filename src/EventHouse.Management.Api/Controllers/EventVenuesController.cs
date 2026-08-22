using EventHouse.Management.Api.Common.Errors;
using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.EventVenues;
using EventHouse.Management.Api.Mappers.EventVenues;
using EventHouse.Management.Api.Swagger;
using EventHouse.Management.Api.Swagger.Examples.Contracts.EventVenues;
using EventHouse.Management.Api.Swagger.Examples.Requests.EventVenues;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace EventHouse.Management.Api.Controllers;

[ApiController]
[Route("api/v1/event-venues")]
public sealed class EventVenuesController(ISender sender) : BaseApiController
{
    #region READ
    [HttpGet("{eventVenueId:guid}")]
    [SwaggerOperation(
        OperationId = "GetEventVenueById",
        Summary = "Retrieve a specific event venue by their unique identifier."
        )]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(EventVenueResponseExample))]
    [ProducesOk<EventVenueResponse>]
    [ProducesNotFoundProblem]
    public async Task<ActionResult<EventVenueResponse>> GetById(
        Guid eventVenueId,
        CancellationToken ct)
    {
        var resultDto = await sender.Send(
            GetEventVenueByIdQueryMapper.FromContract(eventVenueId),
            ct);

        return Ok(EventVenueMapper.ToContract(resultDto));
    }

    [HttpGet]
    [SwaggerOperation(
        OperationId = "ListEventVenues",
        Summary = "List event venues with optional filtering, sorting, and pagination.")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(EventVenuePagedResultExample))]
    [SwaggerRequestExample(typeof(GetEventVenuesRequest), typeof(GetEventVenuesRequestExample))]
    [ProducesOk<PagedResult<EventVenueResponse>>]
    [ProducesValidationProblem]
    [ProducesTooManyRequestsProblem]
    public async Task<ActionResult<PagedResult<EventVenueResponse>>> GetAll(
    [FromQuery] GetEventVenuesRequest query,
    CancellationToken ct)
    {
        var resultDto = await sender.Send(
            GetAllEventVenuesQueryMapper.FromContract(query),
            ct);

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
    [ProducesValidationProblem]
    [ProducesConflictProblem]
    public async Task<ActionResult<EventVenueResponse>> Create(
        [FromBody] CreateEventVenueRequest body,
        CancellationToken ct)
    {
        var createdDto = await sender.Send(
            CreateEventVenueCommandMapper.FromContract(body),
            ct);

        var createdContract = EventVenueMapper.ToContract(createdDto);

        return CreatedAtAction(nameof(GetById), new { eventVenueId = createdContract.Id }, createdContract);
    }

    [HttpPut("{eventVenueId:guid}/status")]
    [SwaggerOperation(
        OperationId = "UpdateEventVenueStatus",
        Summary = "Update an existing event venues details.")]
    [SwaggerRequestExample(typeof(UpdateEventVenueStatusRequest), typeof(UpdateEventVenueStatusRequestExample))]
    [ProducesNoContent]
    [ProducesNotFoundProblem]
    [ProducesValidationProblem]
    public async Task<IActionResult> UpdateStatus(
        Guid eventVenueId,
        [FromBody] UpdateEventVenueStatusRequest body,
        CancellationToken ct)
    {
        await sender.Send(
            UpdateEventVenueStatusCommandMapper.FromContract(eventVenueId, body),
            ct);

        return NoContent();
    }
    #endregion

}
