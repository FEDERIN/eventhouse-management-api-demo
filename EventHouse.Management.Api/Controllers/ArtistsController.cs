using EventHouse.Management.Api.Common;
using EventHouse.Management.Api.Common.Errors;
using EventHouse.Management.Api.Contracts.Artists;
using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Mappers;
using EventHouse.Management.Api.Mappers.Artists;
using EventHouse.Management.Api.Mappers.Enums;
using EventHouse.Management.Api.Swagger;
using EventHouse.Management.Api.Swagger.Examples.Contracts.Artists;
using EventHouse.Management.Api.Swagger.Examples.Requests.Artists;
using EventHouse.Management.Application.Commands.Artists.Create;
using EventHouse.Management.Application.Commands.Artists.Delete;
using EventHouse.Management.Application.Commands.Artists.Update;
using EventHouse.Management.Application.Common;
using EventHouse.Management.Application.Queries.Artists.GetAll;
using EventHouse.Management.Application.Queries.Artists.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace EventHouse.Management.Api.Controllers;

[ApiController]
[Route("api/v1/artists")]
[Produces("application/json")]
public sealed class ArtistsController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    private static readonly string[] BlockingAssociations = ["artistPerformances"];

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
        var result = await _mediator.Send(new GetAllArtistsQuery
        {
            Name = request.Name,
            Category = ArtistCategoryMapper.ToApplicationOptional(request.Category),
            Page = request.Page,
            PageSize = request.PageSize,
            SortBy = ArtistSortMapper.ToApplication(request.SortBy),
            SortDirection = SortDirectionMapper.ToApplication(request.SortDirection)
        }, cancellationToken);

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

        if (resultDto is null)
            return ArtistNotFound(artistId);

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
        var result = await _mediator.Send(
            new UpdateArtistCommand(
                artistId,
                body.Name,
                ArtistCategoryMapper.ToApplicationRequired(body.Category)
            ),
            cancellationToken);

        return result switch
        {
            UpdateResult.NotFound => ArtistNotFound(artistId),
            UpdateResult.InvalidState => ConflictProblem(
                code: "ARTIST_INVALID_STATE",
                title: "Invalid artist state",
                detail: "The artist cannot be updated in its current state."
            ),
            _ => NoContent()
        };
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
        var result = await _mediator.Send(new DeleteArtistCommand(artistId), cancellationToken);

        if (result.NotFound)
            return ArtistNotFound(artistId);

        if (result.HasAssociations)
        {
            return ConflictProblem(
                code: "ARTIST_HAS_ASSOCIATIONS",
                title: "Artist cannot be deleted",
                detail: "This artist cannot be deleted because it has associated entities.",
                ext: new Dictionary<string, object?>
                {
                    ["blockingAssociations"] = BlockingAssociations
                }
            );
        }

        return NoContent();
    }

    private ObjectResult ArtistNotFound(Guid artistId) =>
        NotFoundProblem(
            code: "ARTIST_NOT_FOUND",
            title: "Artist not found",
            detail: $"No artist exists with id '{artistId}'."
        );
}
