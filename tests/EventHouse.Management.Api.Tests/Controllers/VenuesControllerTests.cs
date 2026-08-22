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

    #region READ (GET)
    [Fact]
    public async Task GetAll_WithPaging_ReturnsPagedResult()
    {
        // Arrange
        var prefix = Guid.NewGuid().ToString();
        for (int i = 0; i < 3; i++) await CreateVenueAsync($"{prefix}_Arena_{i}");

        // Act
        var res = await Client.GetAsync($"{BaseUrlVenues}?page=1&pageSize=2", TestContext.Current.CancellationToken);
        var page = await res.ReadContentAsync<PagedResult<VenueResponse>>();

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        page.Items.Should().HaveCount(2);
        page.ShouldHaveValidPaginationLinks(currentPage: 1, expectedPageSize: 2);
    }

    [Theory]
    [InlineData("Arena", "EL Dorado", null, null, null, null, true)]
    [InlineData(null, null, "Miami", null, null, null, false)]
    [InlineData(null, null, null, "FL", null, null, null)]
    [InlineData(null, null, null, null, "US", null, null)]
    [InlineData("Arena", null, "Miami", "FL", "US", 100, true)]
    public async Task GetAll_WithFiltersAndSorting_ReturnsFilteredResults(
    string? name,
    string? address,
    string? city,
    string? region,
    string? countryCode,
    int? capacity,
    bool? isActive)
    {
        // Arrange
        await CreateVenueAsync(name, city, capacity);
        await CreateVenueAsync(name: "Arena Toronto", city, capacity);

        var url = $"{BaseUrlVenues}?" +
                  (name != null ? $"name={name}&" : "") +
                  (address != null ? $"address={address}&" : "") +
                  (city != null ? $"city={city}&" : "") +
                  (region != null ? $"region={region}&" : "") +
                  (countryCode != null ? $"countryCode={countryCode}&" : "") +
                  (capacity != null ? $"capacity={capacity}&" : "") +
                  (isActive != null ? $"isActive={isActive}" : "");
        // Act
        var response = await Client.GetAsync(url, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }


    [Theory]
    [InlineData(VenueSortBy.Name, SortDirection.Asc)]
    [InlineData(VenueSortBy.Name, SortDirection.Desc)]
    [InlineData(VenueSortBy.Address, SortDirection.Asc)]
    [InlineData(VenueSortBy.Address, SortDirection.Desc)]
    [InlineData(VenueSortBy.City, SortDirection.Asc)]
    [InlineData(VenueSortBy.City, SortDirection.Desc)]
    [InlineData(VenueSortBy.Region, SortDirection.Asc)]
    [InlineData(VenueSortBy.CountryCode, SortDirection.Desc)]
    [InlineData(VenueSortBy.Capacity, SortDirection.Asc)]
    [InlineData(VenueSortBy.Capacity, SortDirection.Desc)]
    [InlineData(VenueSortBy.IsActive, SortDirection.Asc)]
    [InlineData(VenueSortBy.IsActive, SortDirection.Desc)]
    [InlineData(null,  SortDirection.Asc)]
    [InlineData(null, SortDirection.Desc)]
    public async Task GetAll_WithSorting_ReturnsSortedResults(VenueSortBy? sortBy, SortDirection direction)
    {
        // Arrange
        await CreateVenueAsync();
        await CreateVenueAsync();
        await CreateVenueAsync();

        var url = $"{BaseUrlVenues}?sortBy={sortBy}&sortDirection={direction}";

        // Act
        var res = await Client.GetAsync(url, TestContext.Current.CancellationToken);
        var page = await res.ReadContentAsync<PagedResult<VenueResponse>>();

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.OK);

        switch (sortBy)
        {
            case VenueSortBy.Name:
                ValidateOrder(page.Items.Select(v => v.Name), direction);
                break;
            case VenueSortBy.Address:
                ValidateOrder(page.Items.Select(v => v.Address), direction);
                break;
            case VenueSortBy.City:
                ValidateOrder(page.Items.Select(v => v.City), direction);
                break;
            case VenueSortBy.Region:
                ValidateOrder(page.Items.Select(v => v.Region), direction);
                break;
            case VenueSortBy.CountryCode:
                ValidateOrder(page.Items.Select(v => v.CountryCode), direction);
                break;
            case VenueSortBy.Capacity:
                ValidateOrder(page.Items.Select(v => v.Capacity), direction);
                break;
            case VenueSortBy.IsActive:
                ValidateOrder(page.Items.Select(v => v.IsActive), direction);
                break;
            default:
                ValidateOrder(page.Items.Select(v => v.Name), direction);
                break;
        }
    }

    #endregion

    #region CREATE (POST)
    [Fact]
    public async Task Create_Returns201_And_MatchesRequest()
    {
        // Arrange
        var request = VenueFactory.CreateRequest(name: "Miami International Arena");

        // Act
        var response = await Client.PostAsJsonAsync(BaseUrlVenues, request, TestContext.Current.CancellationToken);
        var created = await response.ReadContentAsync<VenueResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        created.Should().BeEquivalentTo(request, opt => opt.ExcludingMissingMembers());
        response.Headers.Location.Should().NotBeNull();
    }

    [Fact]
    public async Task Create_WhenInvalid_Returns400_ValidationProblemJson()
    {
        var res = await Client.PostAsJsonAsync(BaseUrlVenues, new CreateVenueRequest
        {
            Name = "A",
            Address = "1",
            CountryCode = "USD",
            Latitude = 0m,
            Longitude = 0m,
            TimeZoneId = "UTC",
            Capacity = 100,
            IsActive = true
        }, TestContext.Current.CancellationToken);

        await res.ShouldBeProblemJson(HttpStatusCode.BadRequest);
    }
    #endregion

    #region UPDATE (PUT)
    [Fact]
    public async Task Update_Returns204_And_PersistsChanges()
    {
        // Arrange
        var venue = await CreateVenueAsync("Madison Square Garden");
        var updateRequest = VenueFactory.UpdateRequest("Kaseya Center");

        // Act & Assert
        await PutAndAssertPersisted<UpdateVenueRequest, VenueResponse>($"{BaseUrlVenues}/{venue.Id}",updateRequest);
    }

    [Fact]
    public async Task Update_WhenMissing_Returns404_ProblemJson()
    {
        // Arrange
        var venue = new UpdateVenueRequest
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
        };

        // Act & Assert
        await AssertPutReturns404($"{BaseUrlVenues}/{Guid.NewGuid()}", venue);
    }

    [Fact]
    public async Task Update_WhenNameDuplicate_Returns409Conflict()
    {
        // Arrange
        var venue = await CreateVenueAsync();
        var venue2 = await CreateVenueAsync();
        var updateRequest = VenueFactory.UpdateRequest(venue.Name);

        // Act
        var update = await Client.PutAsJsonAsync($"{BaseUrlVenues}/{venue2!.Id}", updateRequest, TestContext.Current.CancellationToken);

        // Assert
        await update.ShouldHaveErrorCode(HttpStatusCode.Conflict, "VENUE_NAME_ALREADY_EXISTS");
    }
    #endregion

    #region DELETE
    [Fact]
    public async Task Delete_Returns204()
    {
        // Arrange 
        var venue = await CreateVenueAsync();

        // Act & Assert
        await AssertDeleteReturns204($"{BaseUrlVenues}/{venue!.Id}");
    }

    [Fact]
    public async Task Delete_Returns204_And_Then_GetReturns404()
    {
        // Arrange
        var venue = await CreateVenueAsync();
        var url = $"{BaseUrlVenues}/{venue!.Id}";

        // Act & Assert
        await AssertDeleteReturns204(url);
        await AssertGetReturns404(url);
    }

    #endregion
}
