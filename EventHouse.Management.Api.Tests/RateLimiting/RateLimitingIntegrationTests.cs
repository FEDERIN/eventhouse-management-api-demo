using EventHouse.Management.Api.Common.Errors;
using EventHouse.Management.Api.Contracts.Auth;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace EventHouse.Management.Api.Tests.RateLimiting;

[Collection("NonParallel")]
public sealed class RateLimitingIntegrationTests(RateLimitingOnlyWebApplicationFactory factory) : IClassFixture<RateLimitingOnlyWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task When_exceeding_rate_limit_returns_429_problem_json_and_retry_after()
    {

        var tokenResult = await _client.PostAsJsonAsync("/auth/token", new TokenRequest
        {
            Username = "demo",
            Password = "demo"
        });

        tokenResult.EnsureSuccessStatusCode();

        var token = await tokenResult.Content.ReadFromJsonAsync<TokenResponse>();
        Assert.NotNull(token);
        Assert.False(string.IsNullOrWhiteSpace(token!.AccessToken));

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token.AccessToken);

        // Act
        HttpResponseMessage? last = null;

        for (var i = 0; i < 6; i++)
        {
            if (last != null && last!.StatusCode.Equals(HttpStatusCode.TooManyRequests))
                break;

            last = await _client.GetAsync("/api/v1/artists?page=1&pageSize=1");
        }

        // Assert
        Assert.NotNull(last);
        Assert.Equal(HttpStatusCode.TooManyRequests, last!.StatusCode);

        Assert.True(last.Headers.Contains("Retry-After"));

        AssertProblemMediaType(last);
        var problem = await last.Content.ReadFromJsonAsync<EventHouseProblemDetails>();
        Assert.NotNull(problem);

        Assert.Equal("RATE_LIMIT_EXCEEDED", problem!.ErrorCode);
        Assert.Equal("urn:eventhouse:error:RATE_LIMIT_EXCEEDED", problem.Type);
        Assert.Equal(429, problem.Status);
        
    }

    private static void AssertProblemMediaType(HttpResponseMessage res)
    {
        res.Content.Headers.ContentType.Should().NotBeNull();
        var mediaType = res.Content.Headers.ContentType!.MediaType;

        mediaType.Should().BeOneOf("application/problem+json", "application/json");
    }
}
