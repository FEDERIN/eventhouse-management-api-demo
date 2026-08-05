using Core.Idempotency.Exceptions;
using EventHouse.Management.Api.Common.Errors;
using EventHouse.Management.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Text.Json;

namespace EventHouse.Management.Api.Middlewares;

public sealed class ExceptionHandlingMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context, IExceptionMapper exceptionMapper)
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

            var (statusCode, errorCode, title, detail, type) = exceptionMapper.Map(ex);

            var problem = new EventHouseProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Type = string.IsNullOrEmpty(type) ? $"urn:eventhouse:error:{errorCode}" : type,
                Instance = context.Request.Path,
                ErrorCode = errorCode,
                TraceId = traceId
            };

            if (ex is IdempotencyFingerprintMismatchException)
            {
                problem.Extensions["idempotencyKey"] =
                    context.Request.Headers["Idempotency-Key"].ToString();
            }

            if (context.RequestServices.GetService<IHostEnvironment>()?.IsDevelopment() == true)
            {
                problem.Extensions["exceptionType"] = ex.GetType().Name;
                problem.Extensions["exceptionMessage"] = ex.Message;
            }

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/problem+json; charset=utf-8";

            var jsonOptions = context.RequestServices
                .GetRequiredService<IOptions<JsonOptions>>()
                .Value
                .SerializerOptions;

            var payload = JsonSerializer.Serialize(problem, jsonOptions);
            await context.Response.WriteAsync(payload);
        }
    }
}