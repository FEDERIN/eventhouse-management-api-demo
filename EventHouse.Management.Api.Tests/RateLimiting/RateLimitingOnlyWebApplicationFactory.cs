using Microsoft.AspNetCore.Hosting;

namespace EventHouse.Management.Api.Tests.RateLimiting;

public sealed class RateLimitingOnlyWebApplicationFactory : CustomWebApplicationFactory, IDisposable
{
    private readonly string? _prevPermit;
    private readonly string? _prevWindow;
    private readonly string? _prevQueue;

    public RateLimitingOnlyWebApplicationFactory()
    {
        _prevPermit = Environment.GetEnvironmentVariable("RateLimiting__PermitLimit");
        _prevWindow = Environment.GetEnvironmentVariable("RateLimiting__WindowSeconds");
        _prevQueue = Environment.GetEnvironmentVariable("RateLimiting__QueueLimit");

        Environment.SetEnvironmentVariable("RateLimiting__PermitLimit", "3");
        Environment.SetEnvironmentVariable("RateLimiting__WindowSeconds", "60");
        Environment.SetEnvironmentVariable("RateLimiting__QueueLimit", "0");
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.UseEnvironment("Development");
    }

    public new void Dispose()
    {
        // ✅ restore para no romper otras pruebas
        Environment.SetEnvironmentVariable("RateLimiting__PermitLimit", _prevPermit);
        Environment.SetEnvironmentVariable("RateLimiting__WindowSeconds", _prevWindow);
        Environment.SetEnvironmentVariable("RateLimiting__QueueLimit", _prevQueue);

        base.Dispose();
    }
}
