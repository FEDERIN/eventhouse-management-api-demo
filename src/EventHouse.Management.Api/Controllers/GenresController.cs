using EventHouse.Management.Api.Common.Errors;
using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.Genres;
using EventHouse.Management.Api.Mappers.Genres;
using EventHouse.Management.Api.Swagger;
using EventHouse.Management.Api.Swagger.Examples.Contracts.Genres;
using EventHouse.Management.Api.Swagger.Examples.Requests.Genres;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace EventHouse.Management.Api.Controllers;

[ApiController]
[Route("api/v1/genres")]
public sealed class GenresController(ISender sender) : BaseApiController
{
    #region READ
    [HttpGet("{genreId:guid}")]
    [SwaggerOperation(
    OperationId = "GetGenreById",
    Summary = "Retrieve a specific genre by its unique identifier.")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GenreResponseExample))]
    [ProducesOk<GenreResponse>]
    [ProducesNotFoundProblem]
    public async Task<ActionResult<GenreResponse>> GetById(Guid genreId, CancellationToken ct)
    {
        var resultDto = await sender.Send(
            GetGenreByIdQueryMapper.FromContract(genreId),
            ct);

        return Ok(GenreMapper.ToContract(resultDto));
    }

    [HttpGet]
    [SwaggerOperation(
        OperationId = "ListGenres",
        Summary = "List genres with optional filtering, sorting, and pagination.")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GenrePagedResultExample))]
    [SwaggerRequestExample(typeof(GetGenresRequest), typeof(GetGenresRequestExample))]
    [ProducesOk<PagedResult<GenreResponse>>]
    [ProducesValidationProblem]
    [ProducesTooManyRequestsProblem]
    public async Task<ActionResult<PagedResult<GenreResponse>>> GetAll(
        [FromQuery] GetGenresRequest request,
        CancellationToken ct)
    {
        var result = await sender.Send(
            GetAllGenresQueryMapper.FromContract(request),
            ct);

        return Ok(GenreMapper.ToContract(result, Request));
    }
    #endregion

    #region WRITE
    [HttpPost]
    [SwaggerOperation(
        OperationId = "CreateGenre",
        Summary = "Create a new genre in the system.")]
    [SwaggerRequestExample(typeof(CreateGenreRequest), typeof(CreateGenreRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status201Created, typeof(GenreResponseExample))]
    [ProducesCreated<GenreResponse>]
    [ProducesValidationProblem]
    [ProducesConflictProblem]
    public async Task<ActionResult<GenreResponse>> Create(
        [FromBody] CreateGenreRequest body,
        CancellationToken ct)
    {
        var createdDto = await sender.Send(
            CreateGenreCommandMapper.FromContract(body),
            ct);

        var created = GenreMapper.ToContract(createdDto);

        return CreatedAtAction(nameof(GetById), new { genreId = created.Id }, created);
    }

    [HttpPut("{genreId:guid}")]
    [SwaggerOperation(
    OperationId = "UpdateGenre",
    Summary = "Update an existing genre in the system.")]
    [SwaggerRequestExample(typeof(UpdateGenreRequest), typeof(UpdateGenreRequestExample))]
    [ProducesNoContent]
    [ProducesValidationProblem]
    [ProducesNotFoundProblem]
    [ProducesConflictProblem]
    public async Task<IActionResult> Update(
    Guid genreId,
    [FromBody] UpdateGenreRequest body,
    CancellationToken ct)
    {
        await sender.Send(
            UpdateGenreCommandMapper.FromContract(genreId, body),
            ct);

        return NoContent();
    }
    #endregion

    #region DELETE
    [HttpDelete("{genreId:guid}")]
    [SwaggerOperation(
        OperationId = "DeleteGenre",
        Summary = "Delete an existing genre from the system.")]
    [ProducesNoContent]
    [ProducesNotFoundProblem]
    [ProducesConflictProblem]
    public async Task<IActionResult> Delete(Guid genreId, CancellationToken ct)
    {
        await sender.Send(
            DeleteGenreCommandMapper.FromContract(genreId),
            ct);

        return NoContent();
    }
    #endregion
}
