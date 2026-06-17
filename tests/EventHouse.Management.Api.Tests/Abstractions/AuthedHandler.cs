using EventHouse.Management.Api.Tests.Common;

namespace EventHouse.Management.Api.Tests.Abstractions;

public class AuthedHandler(CustomWebApplicationFactory factory) : DelegatingHandler
{
    public static readonly HttpRequestOptionsKey<bool> SkipAuth = new("SkipAuth");

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (!request.Options.TryGetValue(SkipAuth, out var skip) || !skip)
        {
            using var client = factory.CreateClient();
            var token = await client.GetBearerTokenAsync();
            request.Headers.Authorization = System.Net.Http.Headers.AuthenticationHeaderValue.Parse(token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}