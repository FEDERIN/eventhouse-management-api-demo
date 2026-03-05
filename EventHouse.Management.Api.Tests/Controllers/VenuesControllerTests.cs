using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.Venues;
using EventHouse.Management.Api.Tests.Abstractions;
using EventHouse.Management.Api.Tests.Common;
using EventHouse.Management.Api.Tests.Factories;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace EventHouse.Management.Api.Tests.Controllers;

public sealed class VenuesControllerTests(CustomWebApplicationFactory factory)
    : BaseIntegrationTest(factory)
{
    private const string BaseUrl = ApiRoutes.Venues;

    [Fact]
    public async Task GetAll_WithoutToken_Returns401()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, BaseUrl)
            .WithoutAuthentication();

        var res = await Client.SendAsync(request);

        res.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Create_Returns201_And_MatchesRequest()
    {
        // Arrange
        var request = VenueFactory.CreateRequest(name: "Miami International Arena");

        // Act
        var response = await Client.PostAsJsonAsync(BaseUrl, request);
        var created = await response.ReadContentAsync<VenueResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        created.Should().BeEquivalentTo(request, opt => opt.ExcludingMissingMembers());
        response.Headers.Location.Should().NotBeNull();
    }

    [Fact]
    public async Task GetById_WhenMissing_Returns404()
    {
        var res = await Client.GetAsync($"{BaseUrl}/{Guid.NewGuid()}");
        res.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_Returns204_And_PersistsChanges()
    {
        // Arrange
        var venue = await CreateVenueAsync("Madison Square Garden");
        var updateRequest = VenueFactory.UpdateRequest("Kaseya Center");

        // Act
        var response = await Client.PutAsJsonAsync($"{BaseUrl}/{venue.Id}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Roundtrip
        var updated = await Client.GetFromJsonAsync<VenueResponse>($"{BaseUrl}/{venue.Id}", JsonTestOptions.Default);
        updated.Should().BeEquivalentTo(updateRequest);
    }

    [Fact]
    public async Task Update_WhenMissing_Returns404_ProblemJson()
    {
        var id = Guid.NewGuid();
        var res = await Client.PutAsJsonAsync($"{BaseUrl}/{id}", new UpdateVenueRequest
        {
            Name = "Kaseya Center 3",
            Address = "601 Biscayne Blvd, Miami, FL 33132",
            City = "Miami",
            Region = "FL",
            CountryCode = "US",
            Latitude = 25.7814m,
            Longitude = -80.1870m,
            TimeZoneId = "America/Miami",
            Capacity = 19600,
            IsActive = true
        });


        await res.ShouldBeProblemJson(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_WhenNameDuplicate_Returns409Conflict()
    {
        // Arrange

        var venue = await CreateVenueAsync();
        var venue2 = await CreateVenueAsync();

        var updateRequest = VenueFactory.UpdateRequest(venue.Name);

        // Act
        var update = await Client.PutAsJsonAsync($"{BaseUrl}/{venue2!.Id}", updateRequest);

        // Assert
        await update.ShouldHaveErrorCode(HttpStatusCode.Conflict, "VENUE_NAME_ALREADY_EXISTS");
    }

    [Fact]
    public async Task Delete_Returns204()
    {
        // create
        var venue = await CreateVenueAsync();

        // delete
        var del = await Client.DeleteAsync($"{BaseUrl}/{venue!.Id}");
        del.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_WhenMissing_Returns404_ProblemJson()
    {

        var id = Guid.NewGuid();

        var del = await Client.DeleteAsync($"{BaseUrl}/{id}");

        await del.ShouldBeProblemJson(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_WhenInvalid_Returns400_ValidationProblemJson()
    {
        var res = await Client.PostAsJsonAsync(BaseUrl, new CreateVenueRequest
        {
            Name = "A",
            Address = "1",
            CountryCode = "USD",
            Latitude = 0m,
            Longitude = 0m,
            TimeZoneId = "UTC",
            Capacity = 100,
            IsActive = true
        });

        await res.ShouldBeProblemJson(HttpStatusCode.BadRequest);
    }

   
    [Fact]
    public async Task GetAll_WithPaging_ReturnsPagedResult()
    {
        // Arrange
        var prefix = Guid.NewGuid().ToString();
        for (int i = 0; i < 3; i++) await CreateVenueAsync($"{prefix}_Arena_{i}");

        // Act
        var res = await Client.GetAsync($"{BaseUrl}?page=1&pageSize=2");
        var page = await res.ReadContentAsync<PagedResult<VenueResponse>>();

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        page.Items.Should().HaveCount(2);
        page.ShouldHaveValidPaginationLinks(currentPage: 1, expectedPageSize: 2);
    }
}
