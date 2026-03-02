namespace EventHouse.Management.Api.Middlewares;

public sealed class CorrelationIdMiddleware : IMiddleware
{
    public const string HeaderName = "X-Correlation-Id";

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        // 1) Toma del request o genera uno nuevo
        var correlationId = context.Request.Headers.TryGetValue(HeaderName, out var values) &&
                            !string.IsNullOrWhiteSpace(values.ToString())
            ? values.ToString()
            : Guid.NewGuid().ToString("N");

        // 2) Exponerlo en el pipeline
        context.Items[HeaderName] = correlationId;

        // 3) Devolverlo siempre en la respuesta
        context.Response.OnStarting(() =>
        {
            context.Response.Headers[HeaderName] = correlationId;
            return Task.CompletedTask;
        });

        // 4) Enriquecer logs con scope (muy útil en Azure)
        using (SerilogOrDefaultScope(context, correlationId))
        {
            await next(context);
        }
    }

    private static IDisposable SerilogOrDefaultScope(HttpContext context, string correlationId)
    {
        var loggerFactory = context.RequestServices.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("CorrelationId");
        return logger.BeginScope(new Dictionary<string, object>
        {
            ["correlationId"] = correlationId
        }) ?? Disposable.Empty;
    }

    private sealed class Disposable : IDisposable
    {
        public static readonly IDisposable Empty = new Disposable();
        public void Dispose() { }
    }
}
