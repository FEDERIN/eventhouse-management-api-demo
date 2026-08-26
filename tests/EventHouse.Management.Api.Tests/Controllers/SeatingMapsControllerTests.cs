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
    #region READ (GET)

    [Theory]
    [InlineData("Central", null, null)]
    [InlineData(null, true, null)]
    [InlineData(null, false, null)]
    [InlineData("Central", true, null)]
    [InlineData(null, null, null)]
    public async Task GetAll_WithFiltersAndSorting_ReturnsFilteredResults(
        string? name,
        bool? isActive,
        Guid? venueId)
    {
        // Arrange
        var venue = await CreateVenueAsync();
        await CreateSeatingMapAsync(venueId: venue.Id, name: "Central", isActive: true);
        await CreateSeatingMapAsync(venueId: venue.Id, name: "North", isActive: false);

        var url = $"{BaseUrlSeatingMaps}?" +
                  (name != null ? $"name={name}&" : "") +
                  (isActive.HasValue ? $"isActive={isActive}&" : "") +
                  (venueId.HasValue ? $"venueId={venueId}" : "");

        // Act
        var response = await Client.GetAsync(url, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData(SeatingMapSortBy.Name, SortDirection.Asc)]
    [InlineData(SeatingMapSortBy.Name, SortDirection.Desc)]
    [InlineData(SeatingMapSortBy.Version, SortDirection.Asc)]
    [InlineData(SeatingMapSortBy.Version, SortDirection.Desc)]
    [InlineData(SeatingMapSortBy.IsActive, SortDirection.Asc)]
    [InlineData(SeatingMapSortBy.IsActive, SortDirection.Desc)]
    [InlineData(null, SortDirection.Asc)]
    [InlineData(null, SortDirection.Desc)]
    public async Task GetAll_WithSorting_ReturnsSortedResults(
        SeatingMapSortBy? sortColumn,
        SortDirection direction)
    {
        // Arrange
        var venue = await CreateVenueAsync();
        await CreateSeatingMapAsync(venueId: venue.Id, name: "Charlie", isActive: false);
        await CreateSeatingMapAsync(venueId: venue.Id, name: "Alpha", isActive: true);
        await CreateSeatingMapAsync(venueId: venue.Id, name: "Delta", isActive: true);

        var url = $"{BaseUrlSeatingMaps}?sortBy={sortColumn}&sortDirection={direction}";

        // Act
        var response = await Client.GetAsync(url, TestContext.Current.CancellationToken);
        var pagedResult = await response.ReadContentAsync<PagedResult<SeatingMapResponse>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = pagedResult.Items;

        switch (sortColumn)
        {
            case SeatingMapSortBy.Name:
                ValidateOrder(items.Select(x => x.Name), direction);
                break;

            case SeatingMapSortBy.Version:
                ValidateOrder(items.Select(x => x.Version), direction);
                break;

            case SeatingMapSortBy.IsActive:
                ValidateOrder(items.Select(x => x.IsActive), direction);
                break;

            default:
                ValidateOrder(items.Select(x => x.Name), direction);
                break;
        }
    }

    [Fact]
    public async Task GetById_WhenExists_Returns200_And_SeatingMap()
    {
        var venue = await CreateVenueAsync();
        var seatingMap = await CreateSeatingMapAsync(venueId: venue.Id);

        var res = await Client.GetAsync($"{BaseUrlSeatingMaps}/{seatingMap.Id}", TestContext.Current.CancellationToken);
        var returned = await res.ReadContentAsync<SeatingMapResponse>();

        res.StatusCode.Should().Be(HttpStatusCode.OK);

        // Use BeEquivalentTo but handle the precision for DateTime properties
        returned.Should().BeEquivalentTo(seatingMap, opt => opt
            .ExcludingMissingMembers()
            .WithPostgresPrecision()
        );
    }

    [Fact]
    public async Task GetAll_WithMultiple_Returns200_And_AllSeatingMaps()
    {
        var venue = await CreateVenueAsync();
        var seatingMap1 = await CreateSeatingMapAsync(venueId: venue.Id);
        var seatingMap2 = await CreateSeatingMapAsync(venueId: venue.Id);

        var res = await Client.GetAsync(BaseUrlSeatingMaps, TestContext.Current.CancellationToken);
        var pagedResult = await res.ReadContentAsync<PagedResult<SeatingMapResponse>>();

        res.StatusCode.Should().Be(HttpStatusCode.OK);

        pagedResult.Items.Should().HaveCountGreaterThanOrEqualTo(2);
        pagedResult.TotalCount.Should().BeGreaterThanOrEqualTo(2);
    }
    #endregion

    #region CREATE (POST)
    [Fact]
    public async Task Create_Returns201_And_MatchesRequest()
    {
        var venue = await CreateVenueAsync();

        var request = SeatingMapFactory.CreateRequest(venueId: venue.Id);

        var response = await Client.PostAsJsonAsync(BaseUrlSeatingMaps, request, TestContext.Current.CancellationToken);
        var created = await response.ReadContentAsync<SeatingMapResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        created.Should().BeEquivalentTo(request, opt => opt.ExcludingMissingMembers());
    }

    [Fact]
    public async Task Create_WhenVenueMissing_Returns404()
    {
        var request = SeatingMapFactory.CreateRequest(venueId: Guid.NewGuid());
        var response = await Client.PostAsJsonAsync(BaseUrlSeatingMaps, request, TestContext.Current.CancellationToken);
        await response.ShouldBeProblemJson(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_WithDuplicateName_Returns409()
    {
        var venue = await CreateVenueAsync();
        _ = await CreateSeatingMapAsync(venueId: venue.Id, name: "Unique Map Name");
        var request = SeatingMapFactory.CreateRequest(venueId: venue.Id, name: "Unique Map Name");
        var response = await Client.PostAsJsonAsync(BaseUrlSeatingMaps, request, TestContext.Current.CancellationToken);
        await response.ShouldBeProblemJson(HttpStatusCode.Conflict);
    }
    #endregion

    #region UPDATE (PUT)
    //[Fact]
    //public async Task Update_Returns204_And_PersistsChanges()
    //{
    //    var venue = await CreateVenueAsync();
    //    var seatingMap = await CreateSeatingMapAsync(venueId: venue.Id);
    //    var updateRequest = SeatingMapFactory.UpdateRequest();

    //    await PutAndAssertPersisted<UpdateSeatingMapRequest, SeatingMapResponse>(
    //        $"{BaseUrlSeatingMaps}/{seatingMap.Id}",
    //        updateRequest
    //    );
    //}

    //[Fact]
    //public async Task Update_WithDuplicateName_Returns409()
    //{
    //    var venue = await CreateVenueAsync();
    //    _ = await CreateSeatingMapAsync(venueId: venue.Id, name: "First Map");
    //    var seatingMap2 = await CreateSeatingMapAsync(venueId: venue.Id, name: "Second Map");
    //    var updateRequest = SeatingMapFactory.UpdateRequest(name: "First Map");
    //    var response = await Client.PutAsJsonAsync($"{BaseUrlSeatingMaps}/{seatingMap2.Id}", updateRequest, TestContext.Current.CancellationToken);
    //    await response.ShouldBeProblemJson(HttpStatusCode.Conflict);
    //}

    [Fact]
    public async Task Update_WhenMissing_Returns404()
    {
        var updateRequest = SeatingMapFactory.UpdateRequest();
        var response = await Client.PutAsJsonAsync($"{BaseUrlSeatingMaps}/{Guid.NewGuid()}", updateRequest, TestContext.Current.CancellationToken);
        await response.ShouldBeProblemJson(HttpStatusCode.NotFound);
    }
    #endregion

    #region DELETE
    [Fact]
    public async Task Delete_Returns204_And_Then_GetReturns404()
    {
        // Arrange
        var venue = await CreateVenueAsync();
        var seatingMap = await CreateSeatingMapAsync(venueId: venue.Id);
        var url = $"{BaseUrlSeatingMaps}/{seatingMap.Id}";

        // Act & Assert
        await AssertDeleteReturns204(url);
        await AssertGetReturns404(url);
    }
    #endregion
}
