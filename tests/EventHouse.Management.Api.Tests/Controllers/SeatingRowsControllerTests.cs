using EventHouse.Management.Api.Contracts.Seating.Rows;
using EventHouse.Management.Api.Contracts.Seating.Seats;
using EventHouse.Management.Api.Contracts.Seating.Sections;
using EventHouse.Management.Api.Tests.Abstractions;
using EventHouse.Management.Api.Tests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace EventHouse.Management.Api.Tests.Controllers;

public sealed class SeatingRowsControllerTests(
    CustomWebApplicationFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Add_ReturnsNoContent()
    {
        // Arrange
        var venue = await CreateVenueAsync();
        var seatingMap = await CreateSeatingMapAsync(venue.Id);

        var sectionId = await AddSeatingSectionAsync(
            seatingMap.Id,
            "VIP",
            true,
            100);

        var request = new AddSeatingRowRequest(
            1,
            "A");

        // Act
        var response = await Client.PostAsJsonAsync(
            ApiRoutes.SeatingRows(
                seatingMap.Id,
                sectionId),
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Add_WhenRowNumberAlreadyExists_Returns409()
    {
        // Arrange
        var venue = await CreateVenueAsync();
        var seatingMap = await CreateSeatingMapAsync(venue.Id);

        var sectionId = await AddSeatingSectionAsync(
            seatingMap.Id,
            "VIP",
            true,
            100);

        var firstRequest = new AddSeatingRowRequest(1, "A");

        await Client.PostAsJsonAsync(
            ApiRoutes.SeatingRows(seatingMap.Id, sectionId),
            firstRequest,
            TestContext.Current.CancellationToken);

        // Act
        var response = await Client.PostAsJsonAsync(
            ApiRoutes.SeatingRows(seatingMap.Id, sectionId),
            new AddSeatingRowRequest(1, "B"),
            TestContext.Current.CancellationToken);

        // Assert
        await response.ShouldBeProblemJson(HttpStatusCode.Conflict);
    }


    [Fact]
    public async Task Add_WhenSectionIsNonNumbered_Returns409()
    {
        // Arrange
        var venue = await CreateVenueAsync();
        var seatingMap = await CreateSeatingMapAsync(venue.Id);

        var sectionId = await AddSeatingSectionAsync(
            seatingMap.Id,
            "General",
            false,
            100);

        var request = new AddSeatingRowRequest(
            1,
            "A");

        // Act
        var response = await Client.PostAsJsonAsync(
            ApiRoutes.SeatingRows(
                seatingMap.Id,
                sectionId),
            request,
            TestContext.Current.CancellationToken);

        // Assert
        await response.ShouldBeProblemJson(
            HttpStatusCode.Conflict);
    }


    [Fact]
    public async Task Add_WhenSectionIsInactive_Returns409()
    {
        // Arrange
        var venue = await CreateVenueAsync();
        var seatingMap = await CreateSeatingMapAsync(venue.Id);

        var sectionId = await AddSeatingSectionAsync(
            seatingMap.Id,
            "VIP",
            true,
            100);

        await Client.PatchAsJsonAsync(
            $"{ApiRoutes.SeatingSections(seatingMap.Id)}/{sectionId}/status",
            new UpdateSeatingSectionStatusRequest(false),
            TestContext.Current.CancellationToken);

        var request = new AddSeatingRowRequest(
            1,
            "A");

        // Act
        var response = await Client.PostAsJsonAsync(
            ApiRoutes.SeatingRows(
                seatingMap.Id,
                sectionId),
            request,
            TestContext.Current.CancellationToken);

        // Assert
        await response.ShouldBeProblemJson(
            HttpStatusCode.Conflict);
    }


    [Fact]
    public async Task AddSeat_WhenSectionCapacityIsReached_Returns409()
    {
        // Arrange
        var venue = await CreateVenueAsync();

        var seatingMap = await CreateSeatingMapAsync(
            venue.Id);

        var sectionId = await AddSeatingSectionAsync(
            seatingMap.Id,
            "VIP",
            true,
            1);

        var rowId = await AddSeatingRowAsync(
            seatingMap.Id,
            sectionId,
            1,
            "A");

        // First seat consumes the entire capacity.
        var firstSeatResponse = await Client.PostAsJsonAsync(
            ApiRoutes.SeatingSeats(
                seatingMap.Id,
                sectionId,
                rowId),
            new AddSeatingSeatRequest(1, "1"),
            TestContext.Current.CancellationToken);

        firstSeatResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.NoContent);

        // Act
        var secondSeatResponse = await Client.PostAsJsonAsync(
            ApiRoutes.SeatingSeats(
                seatingMap.Id,
                sectionId,
                rowId),
            new AddSeatingSeatRequest(2, "2"),
            TestContext.Current.CancellationToken);

        // Assert
        await secondSeatResponse.ShouldBeProblemJson(
            HttpStatusCode.Conflict);
    }


    [Fact]
    public async Task AddSeat_WhenRowIsInactive_Returns409()
    {
        // Arrange
        var venue = await CreateVenueAsync();
        var seatingMap = await CreateSeatingMapAsync(venue.Id);

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

        await Client.PatchAsJsonAsync(
            ApiRoutes.SeatingRows(
                seatingMap.Id,
                sectionId) + $"/{rowId}/status",
            new UpdateSeatingRowStatusRequest(false),
            TestContext.Current.CancellationToken);

        // Act
        var response = await Client.PostAsJsonAsync(
            ApiRoutes.SeatingSeats(
                seatingMap.Id,
                sectionId,
                rowId),
            new AddSeatingSeatRequest(1, "1"),
            TestContext.Current.CancellationToken);

        // Assert
        await response.ShouldBeProblemJson(
            HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task AddSeat_WhenSeatNumberAlreadyExists_Returns409()
    {
        // Arrange
        var venue = await CreateVenueAsync();
        var seatingMap = await CreateSeatingMapAsync(venue.Id);

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

        var firstRequest = new AddSeatingSeatRequest(
            1,
            "1");

        var firstResponse = await Client.PostAsJsonAsync(
            ApiRoutes.SeatingSeats(
                seatingMap.Id,
                sectionId,
                rowId),
            firstRequest,
            TestContext.Current.CancellationToken);

        firstResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.NoContent);

        // Act
        var response = await Client.PostAsJsonAsync(
            ApiRoutes.SeatingSeats(
                seatingMap.Id,
                sectionId,
                rowId),
            new AddSeatingSeatRequest(1, "2"),
            TestContext.Current.CancellationToken);

        // Assert
        await response.ShouldBeProblemJson(
            HttpStatusCode.Conflict);
    }


    [Fact]
    public async Task UpdateStatus_WhenActive_DeactivatesRow()
    {
        // Arrange
        var venue = await CreateVenueAsync();
        var seatingMap = await CreateSeatingMapAsync(venue.Id);

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

        var request = new UpdateSeatingRowStatusRequest(false);

        // Act
        var response = await Client.PatchAsJsonAsync(
            $"{ApiRoutes.SeatingRows(
                seatingMap.Id,
                sectionId)}/{rowId}/status",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var structure =
            await GetSeatingStructureAsync(seatingMap.Id);

        var row = structure.Sections
            .Single(x => x.Id == sectionId)
            .Rows
            .Single(x => x.Id == rowId);

        row.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateStatus_WhenInactive_ActivatesRow()
    {
        // Arrange
        var venue = await CreateVenueAsync();
        var seatingMap = await CreateSeatingMapAsync(venue.Id);

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

        await Client.PatchAsJsonAsync(
            $"{ApiRoutes.SeatingRows(
                seatingMap.Id,
                sectionId)}/{rowId}/status",
            new UpdateSeatingRowStatusRequest(false),
            TestContext.Current.CancellationToken);

        // Act
        var response = await Client.PatchAsJsonAsync(
            $"{ApiRoutes.SeatingRows(
                seatingMap.Id,
                sectionId)}/{rowId}/status",
            new UpdateSeatingRowStatusRequest(true),
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var structure =
            await GetSeatingStructureAsync(seatingMap.Id);

        var row = structure.Sections
            .Single(x => x.Id == sectionId)
            .Rows
            .Single(x => x.Id == rowId);

        row.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task ActivateSeat_WhenRowIsInactive_Returns409()
    {
        // Arrange
        var venue = await CreateVenueAsync();
        var seatingMap = await CreateSeatingMapAsync(venue.Id);

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

        await Client.PatchAsJsonAsync(
            ApiRoutes.SeatingRows(
                seatingMap.Id,
                sectionId) + $"/{rowId}/status",
            new UpdateSeatingRowStatusRequest(false),
            TestContext.Current.CancellationToken);

        // Act
        var response = await Client.PatchAsJsonAsync(
            ApiRoutes.SeatingSeats(
                seatingMap.Id,
                sectionId,
                rowId) + $"/{seatId}/status",
            new UpdateSeatingSeatStatusRequest(true),
            TestContext.Current.CancellationToken);

        // Assert
        await response.ShouldBeProblemJson(
            HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task UpdateStatus_WhenAlreadyActive_RemainsActive()
    {
        // Arrange
        var venue = await CreateVenueAsync();
        var seatingMap = await CreateSeatingMapAsync(venue.Id);

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


        // Act
        var response = await Client.PatchAsJsonAsync(
            $"{ApiRoutes.SeatingRows(
                seatingMap.Id,
                sectionId)}/{rowId}/status",
            new UpdateSeatingRowStatusRequest(true),
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var structure =
            await GetSeatingStructureAsync(seatingMap.Id);

        var row = structure.Sections
            .Single(x => x.Id == sectionId)
            .Rows
            .Single(x => x.Id == rowId);

        row.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateStatus_WhenAlreadyInactive_RemainsInactive()
    {
        // Arrange
        var venue = await CreateVenueAsync();
        var seatingMap = await CreateSeatingMapAsync(venue.Id);

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

        await Client.PatchAsJsonAsync(
            $"{ApiRoutes.SeatingRows(
                seatingMap.Id,
                sectionId)}/{rowId}/status",
            new UpdateSeatingRowStatusRequest(false),
            TestContext.Current.CancellationToken);

        // Act
        var response = await Client.PatchAsJsonAsync(
            $"{ApiRoutes.SeatingRows(
                seatingMap.Id,
                sectionId)}/{rowId}/status",
            new UpdateSeatingRowStatusRequest(false),
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var structure =
            await GetSeatingStructureAsync(seatingMap.Id);

        var row = structure.Sections
            .Single(x => x.Id == sectionId)
            .Rows
            .Single(x => x.Id == rowId);

        row.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateStatus_WhenInactive_ActivatesRowAndItsSeats()
    {
        // Arrange
        var venue = await CreateVenueAsync();
        var seatingMap = await CreateSeatingMapAsync(venue.Id);

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

        // Deactivate row and seat.
        await Client.PatchAsJsonAsync(
            $"{ApiRoutes.SeatingRows(
                seatingMap.Id,
                sectionId)}/{rowId}/status",
            new UpdateSeatingRowStatusRequest(false),
            TestContext.Current.CancellationToken);

        // Act
        var response = await Client.PatchAsJsonAsync(
            $"{ApiRoutes.SeatingRows(
                seatingMap.Id,
                sectionId)}/{rowId}/status",
            new UpdateSeatingRowStatusRequest(true),
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var structure =
            await GetSeatingStructureAsync(seatingMap.Id);

        var row = structure.Sections
            .Single(x => x.Id == sectionId)
            .Rows
            .Single(x => x.Id == rowId);

        row.IsActive.Should().BeTrue();

        var seat = row.Seats
            .Single(x => x.Id == seatId);

        seat.IsActive.Should().BeTrue();
    }
}