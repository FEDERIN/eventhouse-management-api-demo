using EventHouse.Management.Api.Common.Errors;
using System.Diagnostics;

namespace EventHouse.Management.Api.Middlewares;

public sealed class ExceptionHandlingMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var traceId = Activity.Current?.Id ?? context.TraceIdentifier;

            var (statusCode, errorCode, title, detail) = ex switch
            {
                ArgumentException ae => (
                    StatusCodes.Status400BadRequest,
                    "BAD_REQUEST",
                    "Bad request",
                    ae.Message
                ),

                KeyNotFoundException knf => (
                    StatusCodes.Status404NotFound,
                    "NOT_FOUND",
                    "Not found",
                    knf.Message
                ),

                InvalidOperationException ioe => (
                    StatusCodes.Status409Conflict,
                    "CONFLICT",
                    "Conflict",
                    ioe.Message
                ),

                _ => (
                    StatusCodes.Status500InternalServerError,
                    "UNEXPECTED_ERROR",
                    "Unexpected error",
                    "An unexpected error occurred."
                )
            };

            var problem = new EventHouseProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Type = $"urn:eventhouse:error:{errorCode}",
                Instance = context.Request.Path,
                ErrorCode = errorCode,
                TraceId = traceId
            };

            // Dev-only extras (sin filtrar detalles en producción)
            if (context.RequestServices.GetService<IHostEnvironment>()?.IsDevelopment() == true)
            {
                problem.Extensions["exceptionType"] = ex.GetType().Name;
                problem.Extensions["exceptionMessage"] = ex.Message;
            }

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/problem+json";
            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}
