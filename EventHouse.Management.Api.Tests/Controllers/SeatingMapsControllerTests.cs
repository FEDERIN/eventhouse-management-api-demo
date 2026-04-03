using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.SeatingMaps;
using EventHouse.Management.Api.Tests.Abstractions;
using EventHouse.Management.Api.Tests.Common;
using EventHouse.Management.Api.Tests.Factories;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace EventHouse.Management.Api.Tests.Controllers;

public sealed class SeatingMapsControllerTests(CustomWebApplicationFactory factory)
    : BaseIntegrationTest(factory)
{
    #region SECURITY
    [Fact]
    public async Task GetAll_WithoutToken_Returns401()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, BaseUrlSeatingMaps).WithoutAuthentication();
        var res = await Client.SendAsync(request);

        await res.ShouldBeProblemJson(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region GET/READ
    [Fact]
    public async Task GetById_WhenExists_Returns200_And_SeatingMap()
    {
        var venue = await CreateVenueAsync();
        var seatingMap = await CreateSeatingMapAsync(venueId: venue.Id);

        var res = await Client.GetAsync($"{BaseUrlSeatingMaps}/{seatingMap.Id}");
        var returned = await res.ReadContentAsync<SeatingMapResponse>();

        res.StatusCode.Should().Be(HttpStatusCode.OK);
        returned.Should().BeEquivalentTo(seatingMap, opt => opt.ExcludingMissingMembers());
    }

    [Fact]
    public async Task GetById_WhenMissing_Returns404()
    {
        var res = await Client.GetAsync($"{BaseUrlSeatingMaps}/{Guid.NewGuid()}");

        await res.ShouldBeProblemJson(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAll_WithMultiple_Returns200_And_AllSeatingMaps()
    {
        var venue = await CreateVenueAsync();
        var seatingMap1 = await CreateSeatingMapAsync(venueId: venue.Id);
        var seatingMap2 = await CreateSeatingMapAsync(venueId: venue.Id);
        var res = await Client.GetAsync(BaseUrlSeatingMaps);
        var pagedResult = await res.ReadContentAsync<PagedResult<SeatingMapResponse>>();
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        pagedResult.Items.Should().ContainEquivalentOf(seatingMap1, opt => opt.ExcludingMissingMembers());
        pagedResult.Items.Should().ContainEquivalentOf(seatingMap2, opt => opt.ExcludingMissingMembers());
    }
    #endregion

    #region POST/CREATE
    [Fact]
    public async Task Create_Returns201_And_MatchesRequest()
    {
        var venue = await CreateVenueAsync();

        var request = SeatingMapFactory.CreateRequest(venueId: venue.Id);

        var response = await Client.PostAsJsonAsync(BaseUrlSeatingMaps, request);
        var created = await response.ReadContentAsync<SeatingMapResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        created.Should().BeEquivalentTo(request, opt => opt.ExcludingMissingMembers());
    }

    [Fact]
    public async Task Create_WhenVenueMissing_Returns404()
    {
        var request = SeatingMapFactory.CreateRequest(venueId: Guid.NewGuid());
        var response = await Client.PostAsJsonAsync(BaseUrlSeatingMaps, request);
        await response.ShouldBeProblemJson(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_WithDuplicateName_Returns409()
    {
        var venue = await CreateVenueAsync();
        var seatingMap = await CreateSeatingMapAsync(venueId: venue.Id, name: "Unique Map Name");
        var request = SeatingMapFactory.CreateRequest(venueId: venue.Id, name: "Unique Map Name");
        var response = await Client.PostAsJsonAsync(BaseUrlSeatingMaps, request);
        await response.ShouldBeProblemJson(HttpStatusCode.Conflict);
    }
    #endregion

    #region PUT/UPDATE
    [Fact]
    public async Task Update_WhenVenueMissing_Returns404()
    {
        var seatingMap = await CreateSeatingMapAsync();
        var updateRequest = SeatingMapFactory.UpdateRequest();
        var response = await Client.PutAsJsonAsync($"{BaseUrlSeatingMaps}/{seatingMap.Id}", updateRequest);

        await response.ShouldBeProblemJson(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_Returns204_And_PersistsChanges()
    {
        var venue = await CreateVenueAsync();
        var seatingMap = await CreateSeatingMapAsync(venueId: venue.Id);
        var updateRequest = SeatingMapFactory.UpdateRequest();
        var response = await Client.PutAsJsonAsync($"{BaseUrlSeatingMaps}/{seatingMap.Id}", updateRequest);
        
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        var getResponse = await Client.GetAsync($"{BaseUrlSeatingMaps}/{seatingMap.Id}");
        var updated = await getResponse.ReadContentAsync<SeatingMapResponse>();
        
        updated.Should().BeEquivalentTo(updateRequest, opt => opt.ExcludingMissingMembers());
    }

    [Fact]
    public async Task Update_WithDuplicateName_Returns409()
    {
        var venue = await CreateVenueAsync();
        var seatingMap1 = await CreateSeatingMapAsync(venueId: venue.Id, name: "First Map");
        var seatingMap2 = await CreateSeatingMapAsync(venueId: venue.Id, name: "Second Map");
        var updateRequest = SeatingMapFactory.UpdateRequest(name: "First Map");
        var response = await Client.PutAsJsonAsync($"{BaseUrlSeatingMaps}/{seatingMap2.Id}", updateRequest);
        await response.ShouldBeProblemJson(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Update_WhenMissing_Returns404()
    {
        var updateRequest = SeatingMapFactory.UpdateRequest();
        var response = await Client.PutAsJsonAsync($"{BaseUrlSeatingMaps}/{Guid.NewGuid()}", updateRequest);
        await response.ShouldBeProblemJson(HttpStatusCode.NotFound);
    }
    #endregion

    #region DELETE
    [Fact]
    public async Task Delete_Returns204_And_RemovesSeatingMap()
    {
        var venue = await CreateVenueAsync();
        var seatingMap = await CreateSeatingMapAsync(venueId: venue.Id);
        var response = await Client.DeleteAsync($"{BaseUrlSeatingMaps}/{seatingMap.Id}");
        
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_WhenMissing_Returns404()
    {
        var response = await Client.DeleteAsync($"{BaseUrlSeatingMaps}/{Guid.NewGuid()}");
        await response.ShouldBeProblemJson(HttpStatusCode.NotFound);
    }
    #endregion
}
