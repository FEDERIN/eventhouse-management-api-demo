using EventHouse.Management.Api.Contracts.Venues;
using EventHouse.Management.Api.Tests.Abstractions;
using EventHouse.Management.Api.Tests.Factories;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace EventHouse.Management.Api.Tests.Controllers.GenericTest;

public sealed class CrossCuttingIntegrationTests(CustomWebApplicationFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Request_WithCorrelationId_EchoesItInResponse()
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "/ready");
        request.Headers.Add("X-Correlation-Id", "correlation-test-123");

        using var response = await Factory.CreateClient().SendAsync(request, TestContext.Current.CancellationToken);

        response.Headers.GetValues("X-Correlation-Id").Single().Should().Be("correlation-test-123");
    }

    [Fact]
    public async Task Ready_ReturnsOk_WhenDependenciesAreAvailable()
    {
        using var response = await Factory.CreateClient().GetAsync("/ready", TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}

[Collection("NonParallel")]
public sealed class RateLimitingIntegrationTests(RateLimitedWebApplicationFactory factory)
    : IClassFixture<RateLimitedWebApplicationFactory>
{
    [Fact]
    public async Task Requests_OverConfiguredPermitLimit_ReturnTooManyRequests()
    {
        using var client = factory.CreateClient();
        var credentials = new { username = "demo", password = "demo" };

        using var first = await client.PostAsJsonAsync("/auth/token", credentials, TestContext.Current.CancellationToken);
        using var second = await client.PostAsJsonAsync("/auth/token", credentials, TestContext.Current.CancellationToken);

        first.StatusCode.Should().Be(HttpStatusCode.OK);
        second.StatusCode.Should().Be(HttpStatusCode.TooManyRequests);
        second.Content.Headers.ContentType?.MediaType.Should().Be("application/problem+json");
    }
}
