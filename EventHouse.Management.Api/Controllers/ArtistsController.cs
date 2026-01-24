using EventHouse.Management.Api.Common.Errors;
using EventHouse.Management.Api.Contracts.Artists;
using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Mappers.Artists;
using EventHouse.Management.Api.Mappers.Enums;
using EventHouse.Management.Api.Swagger;
using EventHouse.Management.Api.Swagger.Examples.Contracts.Artists;
using EventHouse.Management.Api.Swagger.Examples.Requests.Artists;
using EventHouse.Management.Application.Commands.Artists.Create;
using EventHouse.Management.Application.Commands.Artists.Delete;
using EventHouse.Management.Application.Commands.Artists.Update;
using EventHouse.Management.Application.Queries.Artists.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace EventHouse.Management.Api.Controllers;

[ApiController]
[Route("api/v1/artists")]
public sealed class ArtistsController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [SwaggerOperation(
        OperationId = "ListArtists",
        Summary = "List artists with optional filtering, sorting, and pagination."
        )]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ArtistPagedResultExample))]
    [SwaggerRequestExample(typeof(GetArtistsRequest), typeof(GetArtistsRequestExample))]
    [ProducesOkAttribute<PagedResult<Artist>>]
    [ProducesValidationProblemAttribute]
    [ProducesTooManyRequestsProblemAttribute]
    public async Task<ActionResult<PagedResult<Artist>>> GetAll(
        [FromQuery] GetArtistsRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(GetAllArtistsQueryMapper.FromContract
            (request), cancellationToken);

        return Ok(ArtistMapper.ToContract(result, Request));
    }

    [HttpGet("{artistId:guid}")]
    [SwaggerOperation(
        OperationId = "GetArtistById",
        Summary = "Retrieve a specific artist by their unique identifier."
        )]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ArtistResponseExample))]
    [ProducesOkAttribute<Artist>]
    [ProducesNotFoundProblem]
    public async Task<ActionResult<Artist>> GetById(Guid artistId, CancellationToken cancellationToken)
    {
        var resultDto = await _mediator.Send(new GetArtistByIdQuery(artistId), cancellationToken);

        return Ok(ArtistMapper.ToContract(resultDto));
    }

    [HttpPost]
    [SwaggerOperation(
        OperationId = "CreateArtist",
        Summary = "Create a new artist in the system."
        )]
    [SwaggerRequestExample(typeof(CreateArtistRequest), typeof(CreateArtistRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status201Created, typeof(ArtistResponseExample))]
    [ProducesCreated<Artist>]
    [ProducesValidationProblemAttribute]
    [ProducesConflictProblem]
    public async Task<IActionResult> Create([FromBody] CreateArtistRequest body, CancellationToken cancellationToken)
    {
        var createdDto = await _mediator.Send(
            new CreateArtistCommand(body.Name,
            ArtistCategoryMapper.ToApplicationRequired(body.Category)),
            cancellationToken);

        var createdArtist = ArtistMapper.ToContract(createdDto);

        return CreatedAtAction(nameof(GetById), new { artistId = createdArtist.Id }, createdArtist);
    }

    [HttpPut("{artistId:guid}")]
    [SwaggerOperation(
        OperationId = "UpdateArtist",
        Summary = "Update an existing artist's details."
        )]
    [SwaggerRequestExample(typeof(UpdateArtistRequest), typeof(UpdateArtistRequestExample))]
    [ProducesNoContentAttribute]
    [ProducesValidationProblemAttribute]
    [ProducesNotFoundProblem]
    [ProducesConflictProblem]
    public async Task<IActionResult> Update(Guid artistId, [FromBody] UpdateArtistRequest body, CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new UpdateArtistCommand(
                artistId,
                body.Name,
                ArtistCategoryMapper.ToApplicationRequired(body.Category)
            ),
            cancellationToken);

        return NoContent();
    }

    [HttpDelete("{artistId:guid}")]
    [SwaggerOperation(
        OperationId = "DeleteArtist",
        Summary = "Delete an artist from the system."
        )]
    [ProducesNoContentAttribute]
    [ProducesNotFoundProblem]
    [ProducesConflictProblem]
    public async Task<IActionResult> Delete(Guid artistId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteArtistCommand(artistId), cancellationToken);

        return NoContent();
    }
}
