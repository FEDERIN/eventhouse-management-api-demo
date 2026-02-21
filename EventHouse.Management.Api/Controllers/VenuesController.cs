using EventHouse.Management.Api.Common.Errors;
using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.Venues;
using EventHouse.Management.Api.Mappers.Venues;
using EventHouse.Management.Api.Swagger;
using EventHouse.Management.Api.Swagger.Examples.Contracts.Venues;
using EventHouse.Management.Api.Swagger.Examples.Requests.Venues;
using EventHouse.Management.Application.Commands.Venues.Create;
using EventHouse.Management.Application.Commands.Venues.Delete;
using EventHouse.Management.Application.Commands.Venues.Update;
using EventHouse.Management.Application.Queries.Venues.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace EventHouse.Management.Api.Controllers;

[ApiController]
[Route("api/v1/venues")]
public sealed class VenuesController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]    
    [SwaggerOperation(
        OperationId = "ListVenues",
        Summary = "List venues with optional filtering, sorting, and pagination."
        )]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(VenuePagedResultExample))]
    [SwaggerRequestExample(typeof(GetVenuesRequest), typeof(GetVenuesRequestExample))]
    [ProducesOkAttribute<PagedResult<Venue>>]
    [ProducesValidationProblemAttribute]
    [ProducesTooManyRequestsProblemAttribute]
    public async Task<ActionResult<PagedResult<Venue>>> GetAll(
        [FromQuery] GetVenuesRequest query,
        CancellationToken cancellationToken)
    {
        var resultDto = await _mediator.Send(
            GetAllVenuesQueryMapper.FromContract(query),
            cancellationToken);

        return Ok(VenueMapper.ToContract(resultDto, Request));
    }

    [HttpGet("{venueId:guid}")]
    [SwaggerOperation(
        OperationId = "GetVenueById",
        Summary = "Retrieve a specific venue by their unique identifier."
        )]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(VenueResponseExample))]
    [ProducesOkAttribute<Venue>]
    [ProducesNotFoundProblem]
    public async Task<ActionResult<Venue>> GetById(Guid venueId, CancellationToken cancellationToken)
    {
        var resultDto = await _mediator.Send(new GetVenueByIdQuery(venueId), cancellationToken);

        return Ok(VenueMapper.ToContract(resultDto));
    }

    [HttpPost]
    [SwaggerOperation(
        OperationId = "CreateVenue",
        Summary = "Create a new venue in the system."
        )]
    [SwaggerRequestExample(typeof(CreateVenueRequest), typeof(CreateVenueRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status201Created, typeof(VenueResponseExample))]
    [ProducesCreated<Venue>]
    [ProducesValidationProblemAttribute]
    [ProducesConflictProblem]

    public async Task<ActionResult<Venue>> Create([FromBody] CreateVenueRequest body, CancellationToken cancellationToken)
    {

        var command = new CreateVenueCommand(
            body.Name,
            body.Address,
            body.City,
            body.Region,
            body.CountryCode,
            body.Latitude,
            body.Longitude,
            body.TimeZoneId,
            body.Capacity,
            body.IsActive);

        var createdDto = await _mediator.Send(command, cancellationToken);

        var created = VenueMapper.ToContract(createdDto);

        return CreatedAtAction(nameof(GetById), new { venueId = created.Id }, created);
    }

    [HttpPut("{venueId:guid}")]
    [SwaggerOperation(OperationId = "UpdateVenue",
        Summary = "Update an existing venue's details."
        )]
    [SwaggerRequestExample(typeof(UpdateVenueRequest), typeof(UpdateVenueRequestExample))]
    [ProducesNoContentAttribute]
    [ProducesValidationProblemAttribute]
    [ProducesNotFoundProblem]
    [ProducesConflictProblem]
    public async Task<IActionResult> Update(Guid venueId, [FromBody] UpdateVenueRequest body, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateVenueCommand(
            venueId,
            body.Name,
            body.Address,
            body.City,
            body.Region,
            body.CountryCode,
            body.Latitude,
            body.Longitude,
            body.TimeZoneId,
            body.Capacity,
            body.IsActive), cancellationToken);

        return NoContent();
    }

    [HttpDelete("{venueId:guid}")]
    [SwaggerOperation(OperationId = "DeleteVenue",
        Summary = "Delete a venue from the system."
        )]
    [ProducesNoContentAttribute]
    [ProducesNotFoundProblem]
    [ProducesConflictProblem]
    public async Task<IActionResult> Delete(Guid venueId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteVenueCommand(venueId), cancellationToken);

        return NoContent();
    }
}