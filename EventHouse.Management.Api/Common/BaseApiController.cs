using EventHouse.Management.Api.Common.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EventHouse.Management.Api.Common;

[Authorize]
[ProducesResponseType(typeof(EventHouseProblemDetails), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(EventHouseProblemDetails), StatusCodes.Status403Forbidden)]
[ProducesResponseType(typeof(EventHouseProblemDetails), StatusCodes.Status500InternalServerError)]
public abstract class BaseApiController : ControllerBase
{
    protected ObjectResult NotFoundProblem(string code, string title, string detail, IDictionary<string, object?>? ext = null)
        => CreateProblem(StatusCodes.Status404NotFound, code, title, detail, ext);

    protected ObjectResult ConflictProblem(string code, string title, string detail, IDictionary<string, object?>? ext = null)
        => CreateProblem(StatusCodes.Status409Conflict, code, title, detail, ext);

    protected ObjectResult BadRequestProblem(string code, string title, string detail, IDictionary<string, object?>? ext = null)
        => CreateProblem(StatusCodes.Status400BadRequest, code, title, detail, ext);

    // Para validaciones “tipo formulario” (campo -> errores)
    protected ObjectResult ValidationProblemWithCode(
        string code,
        string title,
        string detail,
        IDictionary<string, string[]> errors,
        IDictionary<string, object?>? ext = null)
    {
        var problem = new ValidationProblemDetails(errors)
        {
            Status = StatusCodes.Status400BadRequest,
            Title = title,
            Detail = detail,
            Instance = HttpContext?.Request?.Path.Value,
            Type = $"urn:eventhouse:error:{code}"
        };

        problem.Extensions["errorCode"] = code;
        problem.Extensions["traceId"] = GetTraceId();

        if (ext is not null)
        {
            foreach (var kv in ext)
                problem.Extensions[kv.Key] = kv.Value;
        }

        return new ObjectResult(problem) { StatusCode = problem.Status };
    }

    protected string? GetTraceId()
    {
        return Activity.Current?.Id ?? HttpContext?.TraceIdentifier;
    }

    private ObjectResult CreateProblem(
    int status,
    string code,
    string title,
    string detail,
    IDictionary<string, object?>? ext)
    {
        var problem = new EventHouseProblemDetails
        {
            Status = status,
            Title = title,
            Detail = detail,
            Instance = HttpContext?.Request?.Path.Value,
            Type = $"urn:eventhouse:error:{code}",
            ErrorCode = code,
            TraceId = GetTraceId()
        };

        if (ext is not null)
        {
            foreach (var kv in ext)
                problem.Extensions[kv.Key] = kv.Value;
        }

        return new ObjectResult(problem)
        {
            StatusCode = status,
            ContentTypes = { "application/problem+json" }
        };
    }

}
