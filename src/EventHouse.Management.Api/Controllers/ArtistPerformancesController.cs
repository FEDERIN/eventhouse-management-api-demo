using EventHouse.Management.Api.Common.Errors;
using EventHouse.Management.Api.Contracts.ArtistPerformances;
using EventHouse.Management.Api.Mappers.ArtistPerformances;
using EventHouse.Management.Api.Swagger;
using EventHouse.Management.Api.Swagger.Examples.Contracts.ArtistPerformances;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace EventHouse.Management.Api.Controllers;

[ApiController]
[Route("api/v1/artist-performances")]
public sealed class ArtistPerformancesController(IMediator mediator) : BaseApiController
{
    #region READ
    [HttpGet("{id:guid}")]
    [ActionName(nameof(GetById))]
    [SwaggerOperation(
        OperationId = "GetArtistPerformanceById",
        Summary = "Retrieve details of a specific artist performance.")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ArtistPerformanceResponseExample))]
    [ProducesOk<ArtistPerformanceResponse>]
    [ProducesNotFoundProblem]
    public async Task<ActionResult<ArtistPerformanceResponse>> GetById(
        Guid id,
        CancellationToken ct = default)
    {
        var resultDto = await mediator.Send(
            GetArtistPerformanceByIdQueryMapper.FromContract(id),
            ct);

        return Ok(ArtistPerformanceMapper.ToContract(resultDto));
    }
    #endregion

    #region DELETE
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
        CancellationToken ct = default)
    {
        await mediator.Send(
            RemoveArtistPerformanceCommandMapper.FromContract(calendarId, artistId)
            , ct);

        return NoContent();
    }
    #endregion
}