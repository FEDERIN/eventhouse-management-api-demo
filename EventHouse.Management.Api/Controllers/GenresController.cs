using EventHouse.Management.Api.Common.Errors;
using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.Genres;
using EventHouse.Management.Api.Mappers.Genres;
using EventHouse.Management.Api.Swagger;
using EventHouse.Management.Api.Swagger.Examples.Contracts.Genres;
using EventHouse.Management.Api.Swagger.Examples.Requests.Genres;
using EventHouse.Management.Application.Commands.Genres.Create;
using EventHouse.Management.Application.Commands.Genres.Delete;
using EventHouse.Management.Application.Commands.Genres.Update;
using EventHouse.Management.Application.Queries.Genres.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace EventHouse.Management.Api.Controllers;

[ApiController]
[Route("api/v1/genres")]
public sealed class GenresController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [SwaggerOperation(
        OperationId = "ListGenres",
        Summary = "List genres with optional filtering, sorting, and pagination.")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GenrePagedResultExample))]
    [SwaggerRequestExample(typeof(GetGenresRequest), typeof(GetGenresRequestExample))]
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
    [SwaggerOperation(
        OperationId = "GetGenreById",
        Summary = "Retrieve a specific genre by its unique identifier.")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GenreResponseExample))]
    [ProducesOkAttribute<Genre>]
    [ProducesNotFoundProblem]
    public async Task<ActionResult<Genre>> GetById(Guid genreId, CancellationToken cancellationToken)
    {
        var resultDto = await _mediator.Send(new GetGenreByIdQuery(genreId), cancellationToken);

        return Ok(GenreMapper.ToContract(resultDto));
    }

    [HttpPost]
    [SwaggerOperation(
        OperationId = "CreateGenre",
        Summary = "Create a new genre in the system.")]
    [SwaggerRequestExample(typeof(CreateGenreRequest), typeof(CreateGenreRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status201Created, typeof(GenreResponseExample))]
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
    [SwaggerOperation(
        OperationId = "UpdateGenre",
        Summary = "Update an existing genre in the system.")]
    [SwaggerRequestExample(typeof(UpdateGenreRequest), typeof(UpdateGenreRequestExample))]
    [ProducesNoContentAttribute]
    [ProducesValidationProblemAttribute]
    [ProducesNotFoundProblem]
    [ProducesConflictProblem]
    public async Task<IActionResult> Update(
        Guid genreId,
        [FromBody] UpdateGenreRequest body,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateGenreCommand(genreId, body.Name), cancellationToken);

        return NoContent();
    }

    [HttpDelete("{genreId:guid}")]
    [SwaggerOperation(
        OperationId = "DeleteGenre",
        Summary = "Delete an existing genre from the system.")]
    [ProducesNoContentAttribute]
    [ProducesNotFoundProblem]
    [ProducesConflictProblem]
    public async Task<IActionResult> Delete(Guid genreId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteGenreCommand(genreId), cancellationToken);

        return NoContent();
    }
}
