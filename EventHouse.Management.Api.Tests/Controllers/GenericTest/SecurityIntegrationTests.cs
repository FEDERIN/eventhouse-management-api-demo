using EventHouse.Management.Api.Tests.Abstractions;
using EventHouse.Management.Api.Tests.Common;
using FluentAssertions;
using System.Net;

namespace EventHouse.Management.Api.Tests.Controllers.GenericTest;

public sealed class SecurityIntegrationTests(CustomWebApplicationFactory factory)
    : BaseIntegrationTest(factory)
{
    [Theory]
    [InlineData(BaseUrlArtists, "GET")]
    [InlineData(BaseUrlEvents, "GET")]
    [InlineData(BaseUrlEventVenues, "GET")]
    [InlineData(BaseUrlGenres, "GET")]
    [InlineData(BaseUrlSeatingMaps, "GET")]
    [InlineData(BaseUrlVenues, "GET")]
    public async Task Endpoints_WithoutToken_Return401Unauthorized(string endpoint, string method)
    {
        // Arrange
        var request = new HttpRequestMessage(new HttpMethod(method), endpoint).WithoutAuthentication();

        // Act
        var response = await Client.SendAsync(request, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
