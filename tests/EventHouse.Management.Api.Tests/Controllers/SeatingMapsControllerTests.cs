using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.Seating.Maps;
using EventHouse.Management.Api.Contracts.Seating.Sections;
using EventHouse.Management.Api.Contracts.Seating.Structure;
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
        await CreateSeatingMapAsync(venueId: venue.Id, name: "Central");
        await CreateSeatingMapAsync(venueId: venue.Id, name: "North");

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
        await CreateSeatingMapAsync(venueId: venue.Id, name: "Charlie");
        await CreateSeatingMapAsync(venueId: venue.Id, name: "Alpha");
        await CreateSeatingMapAsync(venueId: venue.Id, name: "Delta");

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
        _ = await CreateSeatingMapAsync(venueId: venue.Id);
        _ = await CreateSeatingMapAsync(venueId: venue.Id);

        var res = await Client.GetAsync(BaseUrlSeatingMaps, TestContext.Current.CancellationToken);
        var pagedResult = await res.ReadContentAsync<PagedResult<SeatingMapResponse>>();

        res.StatusCode.Should().Be(HttpStatusCode.OK);

        pagedResult.Items.Should().HaveCountGreaterThanOrEqualTo(2);
        pagedResult.TotalCount.Should().BeGreaterThanOrEqualTo(2);
    }

    [Fact]
    public async Task GetStructure_WhenExists_Returns200_And_CompleteSeatingStructure()
    {
        // Arrange
        var venue = await CreateVenueAsync();
        var seatingMap = await CreateSeatingMapAsync(venueId: venue.Id);

        var sectionId = await AddSeatingSectionAsync(
            seatingMap.Id,
            "VIP",
            true,
            100);

        var rowId = await AddSeatingRowAsync(
            seatingMap.Id,
            sectionId,
            1,
            "A");

        var seatId = await AddSeatingSeatAsync(
            seatingMap.Id,
            sectionId,
            rowId,
            1,
            "1");

        // Act
        var response = await Client.GetAsync(
            $"{BaseUrlSeatingMaps}/{seatingMap.Id}/structure",
            TestContext.Current.CancellationToken);

        var structure =
            await response.ReadContentAsync<SeatingMapStructureResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        structure.Id.Should().Be(seatingMap.Id);

        structure.Sections.Should().ContainSingle();

        var section = structure.Sections.Single();

        section.Id.Should().Be(sectionId);
        section.Name.Should().Be("VIP");

        section.Rows.Should().ContainSingle();

        var row = section.Rows.Single();

        row.Id.Should().Be(rowId);
        row.Number.Should().Be(1);
        row.Label.Should().Be("A");

        row.Seats.Should().ContainSingle();

        var seat = row.Seats.Single();

        seat.Id.Should().Be(seatId);
        seat.Number.Should().Be(1);
        seat.Label.Should().Be("1");
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
    [Fact]
    public async Task Update_Returns204_And_PersistsChanges()
    {
        var venue = await CreateVenueAsync();
        var seatingMap = await CreateSeatingMapAsync(venueId: venue.Id);
        var updateRequest = SeatingMapFactory.UpdateRequest();

        await PutAndAssertPersisted<UpdateSeatingMapRequest, SeatingMapResponse>(
            $"{BaseUrlSeatingMaps}/{seatingMap.Id}",
            updateRequest
        );
    }

    [Fact]
    public async Task Update_WithDuplicateName_Returns409()
    {
        var venue = await CreateVenueAsync();
        _ = await CreateSeatingMapAsync(venueId: venue.Id, name: "First Map");
        var seatingMap2 = await CreateSeatingMapAsync(venueId: venue.Id, name: "Second Map");
        var updateRequest = SeatingMapFactory.UpdateRequest(name: "First Map");
        var response = await Client.PutAsJsonAsync($"{BaseUrlSeatingMaps}/{seatingMap2.Id}", updateRequest, TestContext.Current.CancellationToken);
        await response.ShouldBeProblemJson(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Update_WhenMissing_Returns404()
    {
        var updateRequest = SeatingMapFactory.UpdateRequest();
        var response = await Client.PutAsJsonAsync($"{BaseUrlSeatingMaps}/{Guid.NewGuid()}", updateRequest, TestContext.Current.CancellationToken);
        await response.ShouldBeProblemJson(HttpStatusCode.NotFound);
    }
    #endregion

    #region STATUS (PATCH)

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task UpdateStatus_Returns204_And_PersistsStatus(bool isActive)
    {
        // Arrange
        var venue = await CreateVenueAsync();
        var seatingMap = await CreateSeatingMapAsync(venueId: venue.Id);

        var request = new UpdateSeatingMapStatusRequest(isActive);

        // Act
        var response = await Client.PatchAsJsonAsync(
            $"{BaseUrlSeatingMaps}/{seatingMap.Id}/status",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var updated = await Client.GetFromJsonAsync<SeatingMapResponse>(
            $"{BaseUrlSeatingMaps}/{seatingMap.Id}",
            JsonTestOptions.Default,
            TestContext.Current.CancellationToken);

        updated.Should().NotBeNull();
        updated!.IsActive.Should().Be(isActive);
    }

    [Fact]
    public async Task UpdateStatus_WhenAlreadyInactive_Returns204()
    {
        // Arrange
        var venue = await CreateVenueAsync();
        var seatingMap = await CreateSeatingMapAsync(venue.Id);

        await Client.PatchAsJsonAsync(
            $"{BaseUrlSeatingMaps}/{seatingMap.Id}/status",
            new UpdateSeatingMapStatusRequest(false),
            TestContext.Current.CancellationToken);

        // Act
        var response = await Client.PatchAsJsonAsync(
            $"{BaseUrlSeatingMaps}/{seatingMap.Id}/status",
            new UpdateSeatingMapStatusRequest(false),
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var updated = await Client.GetFromJsonAsync<SeatingMapResponse>(
            $"{BaseUrlSeatingMaps}/{seatingMap.Id}",
            JsonTestOptions.Default,
            TestContext.Current.CancellationToken);

        updated.Should().NotBeNull();
        updated!.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateStatus_WhenActive_DeactivatesSeatingMapAndSections()
    {
        // Arrange
        var venue = await CreateVenueAsync();
        var seatingMap = await CreateSeatingMapAsync(venue.Id);

        var sectionId = await AddSeatingSectionAsync(
            seatingMap.Id,
            "VIP",
            isNumbered: true,
            capacity: 100);

        // Act
        var response = await Client.PatchAsJsonAsync(
            $"{BaseUrlSeatingMaps}/{seatingMap.Id}/status",
            new UpdateSeatingMapStatusRequest(false),
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var structure =
            await GetSeatingStructureAsync(seatingMap.Id);

        structure.Sections
            .Single(x => x.Id == sectionId)
            .IsActive
            .Should()
            .BeFalse();
    }

    [Fact]
    public async Task UpdateStatus_WhenInactive_ActivatesSeatingMapAndSections()
    {
        // Arrange
        var venue = await CreateVenueAsync();
        var seatingMap = await CreateSeatingMapAsync(venue.Id);

        var sectionId = await AddSeatingSectionAsync(
            seatingMap.Id,
            "VIP",
            isNumbered: true,
            capacity: 100);

        await Client.PatchAsJsonAsync(
            $"{BaseUrlSeatingMaps}/{seatingMap.Id}/status",
            new UpdateSeatingMapStatusRequest(false),
            TestContext.Current.CancellationToken);

        // Act
        var response = await Client.PatchAsJsonAsync(
            $"{BaseUrlSeatingMaps}/{seatingMap.Id}/status",
            new UpdateSeatingMapStatusRequest(true),
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var structure =
            await GetSeatingStructureAsync(seatingMap.Id);

        structure.Sections
            .Single(x => x.Id == sectionId)
            .IsActive
            .Should()
            .BeTrue();
    }

    [Fact]
    public async Task AddSection_WhenSeatingMapIsInactive_Returns409()
    {
        // Arrange
        var venue = await CreateVenueAsync();
        var seatingMap = await CreateSeatingMapAsync(venue.Id);

        await Client.PatchAsJsonAsync(
            $"{BaseUrlSeatingMaps}/{seatingMap.Id}/status",
            new UpdateSeatingMapStatusRequest(false),
            TestContext.Current.CancellationToken);

        var request = new AddSeatingSectionRequest(
            "VIP",
            true,
            100);

        // Act
        var response = await Client.PostAsJsonAsync(
            ApiRoutes.SeatingSections(seatingMap.Id),
            request,
            TestContext.Current.CancellationToken);

        // Assert
        await response.ShouldBeProblemJson(
            HttpStatusCode.Conflict);
    }

    #endregion
}
