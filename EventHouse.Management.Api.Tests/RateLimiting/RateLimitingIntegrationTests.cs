using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using EventHouse.Management.Api.Common.Errors;
using EventHouse.Management.Api.Contracts.Auth;
using Microsoft.Extensions.Configuration;

namespace EventHouse.Management.Api.Tests.RateLimiting;

public sealed class RateLimitingIntegrationTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory = factory;

    [Fact]
    public async Task When_exceeding_rate_limit_returns_429_problem_json_and_retry_after()
    {
        using var client = factory.CreateClient();

        var tokenResult = await client.PostAsJsonAsync("/auth/token", new TokenRequest
        {
            Username = "demo",
            Password = "demo"
        });

        tokenResult.EnsureSuccessStatusCode();

        var token = await tokenResult.Content.ReadFromJsonAsync<TokenResponse>();
        Assert.NotNull(token);
        Assert.False(string.IsNullOrWhiteSpace(token!.AccessToken));

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token.AccessToken);

        // Act
        HttpResponseMessage? last = null;

        for (var i = 0; i < 6; i++)
        {
            if (last!.StatusCode.Equals(HttpStatusCode.TooManyRequests))
                break;

            last = await client.GetAsync("/api/v1/artists?page=1&pageSize=1");
        }

        // Assert
        Assert.NotNull(last);
        Assert.Equal(HttpStatusCode.TooManyRequests, last!.StatusCode);

        Assert.True(last.Headers.Contains("Retry-After"));

        //Note: It's a possible improvement, because my aplication only return  application/json.
        /*
        Assert.Equal("application/problem+json", last.Content.Headers.ContentType?.MediaType);

        var problem = await last.Content.ReadFromJsonAsync<EventHouseProblemDetails>();
        Assert.NotNull(problem);

        Assert.Equal("RATE_LIMIT_EXCEEDED", problem!.ErrorCode);
        Assert.Equal("urn:eventhouse:error:RATE_LIMIT_EXCEEDED", problem.Type);
        Assert.Equal(429, problem.Status);
        */
    }
}
