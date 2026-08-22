using EventHouse.Management.Api.Common.Errors;
using EventHouse.Management.Api.Contracts.Artists;
using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Mappers.Artists;
using EventHouse.Management.Api.Swagger;
using EventHouse.Management.Api.Swagger.Examples.Contracts.Artists;
using EventHouse.Management.Api.Swagger.Examples.Requests.Artists;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace EventHouse.Management.Api.Controllers;

[ApiController]
[Route("api/v1/artists")]
public sealed class ArtistsController(ISender sender) : BaseApiController
{
    #region READ
    [HttpGet("{artistId:guid}")]
    [SwaggerOperation(
    OperationId = "GetArtistById",
    Summary = "Retrieve a specific artist by their unique identifier."
    )]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ArtistDetailResponseExample))]
    [ProducesOk<ArtistDetail>]
    [ProducesNotFoundProblem]
    public async Task<ActionResult<ArtistDetail>> GetById(Guid artistId, CancellationToken ct)
    {
        var resultDto = await sender.Send(
            GetArtistByIdQueryMapper.FromContract(artistId), 
            ct);

        return Ok(ArtistMapper.ToContract(resultDto));
    }

    [HttpGet]
    [SwaggerOperation(
        OperationId = "ListArtists",
        Summary = "List artists with optional filtering, sorting, and pagination."
        )]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ArtistPagedResultExample))]
    [SwaggerRequestExample(typeof(GetArtistsRequest), typeof(GetArtistsRequestExample))]
    [ProducesOk<PagedResult<ArtistSummary>>]
    [ProducesValidationProblem]
    [ProducesTooManyRequestsProblem]
    public async Task<ActionResult<PagedResult<ArtistSummary>>> GetAll(
        [FromQuery] GetArtistsRequest request,
        CancellationToken ct)
    {
        var result = await sender.Send(
            GetAllArtistsQueryMapper.FromContract(request), 
            ct);

        return Ok(ArtistMapper.ToContract(result, Request));
    }
    #endregion

    #region WRITE
    [HttpPost]
    [SwaggerOperation(
        OperationId = "CreateArtist",
        Summary = "Create a new artist in the system."
        )]
    [SwaggerRequestExample(typeof(CreateArtistRequest), typeof(CreateArtistRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status201Created, typeof(ArtistSumaryResponseExample))]
    [ProducesCreated<ArtistSummary>]
    [ProducesValidationProblem]
    [ProducesConflictProblem]
    public async Task<IActionResult> Create([FromBody] CreateArtistRequest body, CancellationToken ct)
    {
        var createdDto = await sender.Send(
            CreateArtistCommandMapper.FromContract(body), 
            ct);

        var createdArtist = ArtistMapper.ToContractSumary(createdDto);

        return CreatedAtAction(nameof(GetById), new { artistId = createdArtist.Id }, createdArtist);
    }

    [HttpPut("{artistId:guid}")]
    [SwaggerOperation(
        OperationId = "UpdateArtist",
        Summary = "Update an existing artist's details."
        )]
    [SwaggerRequestExample(typeof(UpdateArtistRequest), typeof(UpdateArtistRequestExample))]
    [ProducesNoContent]
    [ProducesValidationProblem]
    [ProducesNotFoundProblem]
    [ProducesConflictProblem]
    public async Task<IActionResult> Update(Guid artistId, [FromBody] UpdateArtistRequest body, CancellationToken ct)
    {
        await sender.Send(
            UpdateArtistCommandMapper.FromContract(artistId, body),
            ct);

        return NoContent();
    }
    #endregion

    #region DELETE

    [HttpDelete("{artistId:guid}")]
    [SwaggerOperation(
        OperationId = "DeleteArtist",
        Summary = "Delete an artist from the system."
        )]
    [ProducesNoContent]
    [ProducesNotFoundProblem]
    [ProducesConflictProblem]
    public async Task<IActionResult> Delete(Guid artistId, CancellationToken ct)
    {
        await sender.Send(
            DeleteArtistCommandMapper.FromContract(artistId),
            ct);

        return NoContent();
    }
    #endregion

    [HttpPost("{artistId:guid}/genres")]
    [SwaggerOperation(
        OperationId = "AddGenreToArtist", 
        Summary = "Adds a genre to an artist (idempotent).")]
    [SwaggerRequestExample(typeof(AddArtistGenreRequest), typeof(AddArtistGenreRequestExample))]
    [ProducesNoContent]
    [ProducesNotFoundProblem]
    [ProducesConflictProblem]
    public async Task<IActionResult> AddGenre(Guid artistId, [FromBody] AddArtistGenreRequest body, CancellationToken ct)
    {
        await sender.Send(
            AddArtistGenreCommandMapper.FromContract(artistId, body),
            ct);

        return NoContent();
    }

    [HttpDelete("{artistId:guid}/genres/{genreId:guid}")]
    [SwaggerOperation(
        OperationId = "RemoveGenreFromArtist",
        Summary = "Removes a genre from an artist.")]
    [ProducesNoContent]
    [ProducesNotFoundProblem]
    public async Task<IActionResult> RemoveGenre(Guid artistId, Guid genreId, CancellationToken ct)
    {
        await sender.Send(
            RemoveArtistGenreCommandMapper.FromContract(artistId, genreId),
            ct);

        return NoContent();
    }

    [HttpPatch("{artistId:guid}/genres/{genreId:guid}/primary")]
    [SwaggerOperation(
        OperationId = "SetArtistPrimaryGenre",
        Summary = "Sets a specific genre as primary for an artist.")]
    [ProducesNoContent]
    [ProducesNotFoundProblem]
    [ProducesConflictProblem]
    public async Task<IActionResult> SetPrimaryGenre(Guid artistId, Guid genreId, CancellationToken ct)
    {
        await sender.Send(
            SetPrimaryArtistGenreCommandMapper.FromContract(artistId, genreId),
            ct);

        return NoContent();
    }

    [HttpPut("{artistId:guid}/genres/{genreId:guid}")]
    [SwaggerOperation(
        OperationId = "UpdateArtistGenreStatus",
        Summary = "Updates an artist-genre association status.")]
    [SwaggerRequestExample(typeof(UpdateArtistGenreStatusRequest), typeof(UpdateArtistGenreStatusRequestExample))]
    [ProducesNoContent]
    [ProducesValidationProblem]
    [ProducesNotFoundProblem]
    [ProducesConflictProblem]
    public async Task<IActionResult> UpdateGenreStatus(
        Guid artistId,
        Guid genreId,
        [FromBody] UpdateArtistGenreStatusRequest body,
        CancellationToken ct)
    {
        await sender.Send(
            SetArtistGenreStatusCommandMapper.FromContract(artistId, genreId, body),
            ct);

        return NoContent();
    }
}
