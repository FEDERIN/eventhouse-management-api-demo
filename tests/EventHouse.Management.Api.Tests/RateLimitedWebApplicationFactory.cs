using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.RateLimiting;

namespace EventHouse.Management.Api.Tests;

public sealed class RateLimitedWebApplicationFactory : CustomWebApplicationFactory
{
    protected override bool IsRateLimitingEnabled => true;
    protected override int RateLimitPermitLimit => 1;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureServices(services =>
        {
            services.PostConfigure<RateLimiterOptions>(options =>
            {
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(_ =>
                    RateLimitPartition.GetFixedWindowLimiter("integration-test", _ =>
                        new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = RateLimitPermitLimit,
                            Window = TimeSpan.FromMinutes(1),
                            QueueLimit = 0,
                            AutoReplenishment = true
                        }));
            });
        });
    }
}
