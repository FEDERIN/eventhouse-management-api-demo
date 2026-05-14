using Microsoft.AspNetCore.Hosting;
using System.Threading.RateLimiting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;

namespace EventHouse.Management.Api.Tests.RateLimiting;

public sealed class RateLimitingOnlyWebApplicationFactory : CustomWebApplicationFactory
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.UseEnvironment("Development");

        builder.ConfigureServices(services =>
        {
            services.PostConfigure<Microsoft.AspNetCore.RateLimiting.RateLimiterOptions>(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: "test-limit",
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 3,
                            Window = TimeSpan.FromSeconds(60),
                            QueueLimit = 0,
                            AutoReplenishment = true
                        }));
            });
        });
    }
}