using EventHouse.Management.Api.Tests.Abstractions;
using EventHouse.Management.Api.Tests.Common;
using System.Net;

namespace EventHouse.Management.Api.Tests.Controllers.GenericTest;

public sealed class EndpointConsistencyTests(CustomWebApplicationFactory factory)
    : BaseIntegrationTest(factory)
{
    [Theory]
    [InlineData(BaseUrlArtistPerformances)]
    [InlineData(BaseUrlArtists)]
    [InlineData(BaseUrlEvents)]
    [InlineData(BaseUrlEventVenueCalendars)]
    [InlineData(BaseUrlEventVenues)]
    [InlineData(BaseUrlGenres)]
    [InlineData(BaseUrlSeatingMaps)]
    [InlineData(BaseUrlVenues)]
    public async Task GetById_WhenMissing_Returns404(string baseUrl)
    {
        await AssertGetReturns404($"{baseUrl}/{Guid.NewGuid()}");
    }

    [Theory]
    [InlineData(BaseUrlArtists)]
    [InlineData(BaseUrlEvents)]
    [InlineData(BaseUrlGenres)]
    [InlineData(BaseUrlSeatingMaps)]
    [InlineData(BaseUrlVenues)]
    public async Task Delete_WhenMissing_Returns404(string baseUrl)
    {
        var res = await Client.DeleteAsync($"{baseUrl}/{Guid.NewGuid()}", TestContext.Current.CancellationToken);

        await res.ShouldBeProblemJson(HttpStatusCode.NotFound);
    }

}