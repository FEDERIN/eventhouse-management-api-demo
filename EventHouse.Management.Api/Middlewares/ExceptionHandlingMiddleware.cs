using EventHouse.Management.Api.Common.Errors;
using EventHouse.Management.Application.Exceptions;
using EventHouse.Management.Domain.Exceptions;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Text.Json;

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
            if (context.Response.HasStarted)
                throw;

            context.Response.Clear();

            var traceId = Activity.Current?.Id ?? context.TraceIdentifier;

            var (statusCode, errorCode, title, detail) = ex switch
            {
                ArgumentException ae => (
                    StatusCodes.Status400BadRequest, "BAD_REQUEST", "Bad request", ae.Message),

                NotFoundException nf => (
                StatusCodes.Status404NotFound, nf.Code, nf.Title, nf.Message),

                NotAssociatedException nae => (
                StatusCodes.Status404NotFound, "RESOURCE_NOT_ASSOCIATED", "Resource not associated", nae.Message),

                InvalidOperationException ioe => (
                    StatusCodes.Status409Conflict, "CONFLICT", "Conflict", ioe.Message),

                ConflictException ce => (
                    StatusCodes.Status409Conflict,
                    ce.Code,
                    ce.Title,
                    ce.Message
                ),
                _ => (
                    StatusCodes.Status500InternalServerError, "UNEXPECTED_ERROR", "Unexpected error",
                    "An unexpected error occurred.")
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

            if (context.RequestServices.GetService<IHostEnvironment>()?.IsDevelopment() == true)
            {
                problem.Extensions["exceptionType"] = ex.GetType().Name;
                problem.Extensions["exceptionMessage"] = ex.Message;
            }

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/problem+json; charset=utf-8";

            // usar las mismas opciones JSON de ASP.NET (camelCase, etc.)
            var jsonOptions = context.RequestServices
                .GetRequiredService<IOptions<JsonOptions>>()
                .Value
                .SerializerOptions;

            var payload = JsonSerializer.Serialize(problem, jsonOptions);
            await context.Response.WriteAsync(payload);
        }
    }
}
