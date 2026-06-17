using EventHouse.Management.Api.Common.Errors;
using EventHouse.Management.Api.Contracts.ArtistPerformances;
using EventHouse.Management.Api.Mappers.ArtistPerformances;
using EventHouse.Management.Api.Swagger;
using EventHouse.Management.Api.Swagger.Examples.Contracts.ArtistPerformances;
using EventHouse.Management.Application.Commands.ArtistPerformances.Remove;
using EventHouse.Management.Application.Queries.ArtistPerformances.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace EventHouse.Management.Api.Controllers;

[ApiController]
[Route("api/v1/artist-performances")]
public sealed class ArtistPerformancesController(IMediator mediator) : BaseApiController
{
    [HttpGet("{id:guid}")]
    [ActionName(nameof(GetById))]
    [SwaggerOperation(
        OperationId = "GetArtistPerformanceById",
        Summary = "Retrieve details of a specific artist performance.")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ArtistPerformanceResponseExample))]
    [ProducesOkAttribute<ArtistPerformanceResponse>]
    [ProducesNotFoundProblem]
    public async Task<ActionResult<ArtistPerformanceResponse>> GetById(
        Guid id,
        CancellationToken ct)
    {
        var resultDto = await mediator.Send(new GetArtistPerformanceByIdQuery(id), ct);
        return Ok(ArtistPerformanceMapper.ToContract(resultDto));
    }

    [HttpDelete("{calendarId:guid}/{artistId:guid}")]
    [SwaggerOperation(
        OperationId = "RemoveArtistPerformance",
        Summary = "Remove an artist performance from a calendar.",
        Description = "Removes the performance. If the calendar is Published and the artist is the headliner, the operation will fail.")]
    [ProducesNoContent]
    [ProducesNotFoundProblem]
    [ProducesConflictProblem]
    public async Task<IActionResult> RemovePerformance(
        Guid calendarId,
        Guid artistId,
        CancellationToken ct)
    {
        await mediator.Send(new RemoveArtistPerformanceCommand(calendarId, artistId), ct);
        return NoContent();
    }
}