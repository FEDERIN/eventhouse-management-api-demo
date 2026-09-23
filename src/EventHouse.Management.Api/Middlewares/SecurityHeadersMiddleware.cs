namespace EventHouse.Management.Api.Middlewares;

/// <summary>
/// Adds browser-oriented security headers to every API response.
/// </summary>
public sealed class SecurityHeadersMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        context.Response.OnStarting(() =>
        {
            var headers = context.Response.Headers;
            headers.TryAdd("X-Content-Type-Options", "nosniff");
            headers.TryAdd("X-Frame-Options", "DENY");
            headers.TryAdd("Referrer-Policy", "no-referrer");
            headers.TryAdd(
                "Permissions-Policy",
                "camera=(), microphone=(), geolocation=(), payment=()");

            return Task.CompletedTask;
        });

        await next(context);
    }
}
