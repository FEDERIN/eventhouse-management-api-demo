using EventHouse.Management.Api.Common.Errors;
using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.Venues;
using EventHouse.Management.Api.Mappers.Venues;
using EventHouse.Management.Api.Swagger;
using EventHouse.Management.Api.Swagger.Examples.Contracts.Venues;
using EventHouse.Management.Api.Swagger.Examples.Requests.Venues;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace EventHouse.Management.Api.Controllers;

[ApiController]
[Route("api/v1/venues")]
public sealed class VenuesController(ISender sender) : BaseApiController
{
    #region READ
    [HttpGet("{venueId:guid}")]
    [SwaggerOperation(
    OperationId = "GetVenueById",
    Summary = "Retrieve a specific venue by their unique identifier."
    )]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(VenueResponseExample))]
    [ProducesOk<VenueResponse>]
    [ProducesNotFoundProblem]
    public async Task<ActionResult<VenueResponse>> GetById(Guid venueId, CancellationToken ct)
    {
        var resultDto = await sender.Send(
            GetVenueByIdQueryMapper.FromContract(venueId),
            ct);

        return Ok(VenueMapper.ToContract(resultDto));
    }

    [HttpGet]
    [SwaggerOperation(
    OperationId = "ListVenues",
    Summary = "List venues with optional filtering, sorting, and pagination."
    )]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(VenuePagedResultExample))]
    [SwaggerRequestExample(typeof(GetVenuesRequest), typeof(GetVenuesRequestExample))]
    [ProducesOk<PagedResult<VenueResponse>>]
    [ProducesValidationProblem]
    [ProducesTooManyRequestsProblem]
    public async Task<ActionResult<PagedResult<VenueResponse>>> GetAll(
    [FromQuery] GetVenuesRequest query,
    CancellationToken ct)
    {
        var resultDto = await sender.Send(
            GetAllVenuesQueryMapper.FromContract(query),
            ct);

        return Ok(VenueMapper.ToContract(resultDto, Request));
    }
    #endregion

    #region WRITE
    [HttpPost]
    [SwaggerOperation(
        OperationId = "CreateVenue",
        Summary = "Create a new venue in the system."
        )]
    [SwaggerRequestExample(typeof(CreateVenueRequest), typeof(CreateVenueRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status201Created, typeof(VenueResponseExample))]
    [ProducesCreated<VenueResponse>]
    [ProducesValidationProblem]
    [ProducesConflictProblem]
    public async Task<ActionResult<VenueResponse>> Create([FromBody] CreateVenueRequest body, CancellationToken ct)
    {
        var createdDto = await sender.Send(
            CreateVenueCommandMapper.FromContract(body),
            ct);
        
        var created = VenueMapper.ToContract(createdDto);

        return CreatedAtAction(nameof(GetById), new { venueId = created.Id }, created);
    }

    [HttpPut("{venueId:guid}")]
    [SwaggerOperation(OperationId = "UpdateVenue",
        Summary = "Update an existing venue's details."
        )]
    [SwaggerRequestExample(typeof(UpdateVenueRequest), typeof(UpdateVenueRequestExample))]
    [ProducesNoContent]
    [ProducesValidationProblem]
    [ProducesNotFoundProblem]
    [ProducesConflictProblem]
    public async Task<IActionResult> Update(Guid venueId, [FromBody] UpdateVenueRequest body, CancellationToken ct)
    {
        await sender.Send(
            UpdateVenueCommandMapper.FromContract(venueId, body),
            ct);

        return NoContent();
    }
    #endregion

    #region DELETE
    [HttpDelete("{venueId:guid}")]
    [SwaggerOperation(OperationId = "DeleteVenue",
        Summary = "Delete a venue from the system."
        )]
    [ProducesNoContent]
    [ProducesNotFoundProblem]
    [ProducesConflictProblem]
    public async Task<IActionResult> Delete(Guid venueId, CancellationToken ct)
    {
        await sender.Send(
            DeleteVenueCommandMapper.FromContract(venueId),
            ct);

        return NoContent();
    }
    #endregion
}