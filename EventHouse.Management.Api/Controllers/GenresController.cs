using EventHouse.Management.Api.Common.Errors;
using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.Genres;
using EventHouse.Management.Api.Mappers.Genres;
using EventHouse.Management.Api.Swagger;
using EventHouse.Management.Application.Commands.Genres.Create;
using EventHouse.Management.Application.Commands.Genres.Delete;
using EventHouse.Management.Application.Commands.Genres.Update;
using EventHouse.Management.Application.Common;
using EventHouse.Management.Application.Queries.Genres.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHouse.Management.Api.Controllers;

[ApiController]
[Route("api/v1/genres")]
public sealed class GenresController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [SwaggerOperation(OperationId = "ListGenres")]
    [ProducesOkAttribute<PagedResult<Genre>>]
    [ProducesValidationProblemAttribute]
    [ProducesTooManyRequestsProblemAttribute]
    public async Task<ActionResult<PagedResult<Genre>>> GetAll(
        [FromQuery] GetGenresRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            GetAllGenresQueryMapper.FromContract(request), 
            cancellationToken);

        return Ok(GenreMapper.ToContract(result, Request));
    }

    [HttpGet("{genreId:guid}")]
    [SwaggerOperation(OperationId = "GetGenreById")]
    [ProducesOkAttribute<Genre>]
    [ProducesNotFoundProblem]
    public async Task<ActionResult<Genre>> GetById(Guid genreId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetGenreByIdQuery(genreId), cancellationToken);
        return result is null ? GenreNotFound(genreId) : Ok(result);
    }

    [HttpPost]
    [SwaggerOperation(OperationId = "CreateGenre")]
    [ProducesCreated<Genre>]
    [ProducesValidationProblemAttribute]
    [ProducesConflictProblem]
    public async Task<ActionResult<Genre>> Create(
        [FromBody] CreateGenreRequest body,
        CancellationToken cancellationToken)
    {
        var createdDto = await _mediator.Send(new CreateGenreCommand(body.Name), cancellationToken);
        var created = GenreMapper.ToContract(createdDto);

        return CreatedAtAction(nameof(GetById), new { genreId = created.Id }, created);
    }

    [HttpPut("{genreId:guid}")]
    [SwaggerOperation(OperationId = "UpdateGenre")]
    [ProducesNoContentAttribute]
    [ProducesValidationProblemAttribute]
    [ProducesNotFoundProblem]
    [ProducesConflictProblem]
    public async Task<IActionResult> Update(
        Guid genreId,
        [FromBody] UpdateGenreRequest body,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateGenreCommand(genreId, body.Name), cancellationToken);

        if (result == UpdateResult.NotFound)
            return GenreNotFound(genreId);

        return NoContent();
    }

    [HttpDelete("{genreId:guid}")]
    [SwaggerOperation(OperationId = "DeleteGenre")]
    [ProducesNoContentAttribute]
    [ProducesNotFoundProblem]
    [ProducesConflictProblem]
    public async Task<IActionResult> Delete(Guid genreId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteGenreCommand(genreId), cancellationToken);

        if (result.NotFound)
            return GenreNotFound(genreId);

        return NoContent();
    }

    private ObjectResult GenreNotFound(Guid genreId) =>
        NotFoundProblem(
            code: "GENRE_NOT_FOUND",
            title: "Genre not found",
            detail: $"Genre with id '{genreId}' was not found."
        );
}
