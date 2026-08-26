using EventHouse.Management.Api.Contracts.Seating.Seats;
using EventHouse.Management.Api.Tests.Abstractions;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace EventHouse.Management.Api.Tests.Controllers;

public sealed class SeatingSeatsControllerTests(
    CustomWebApplicationFactory factory)
    : BaseIntegrationTest(factory)
{
    #region ADD (POST)

    [Fact]
    public async Task Add_Returns204NoContent()
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

        var request = new AddSeatingSeatRequest(
            1,
            "1");

        // Act
        var response = await Client.PostAsJsonAsync(
            ApiRoutes.SeatingSeats(
                seatingMap.Id,
                sectionId,
                rowId),
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    #endregion

    #region STATUS (PATCH)

    [Fact]
    public async Task UpdateStatus_WhenActive_DeactivatesSeat()
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

        var request = new UpdateSeatingSeatStatusRequest(false);

        // Act
        var response = await Client.PatchAsJsonAsync(
            $"{ApiRoutes.SeatingSeats(
                seatingMap.Id,
                sectionId,
                rowId)}/{seatId}/status",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var structure =
            await GetSeatingStructureAsync(seatingMap.Id);

        var seat = structure.Sections
            .Single(x => x.Id == sectionId)
            .Rows
            .Single(x => x.Id == rowId)
            .Seats
            .Single(x => x.Id == seatId);

        seat.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateStatus_WhenInactive_ActivatesSeat()
    {
        // Arrange
        var venue = await CreateVenueAsync();
        var seatingMap = await CreateSeatingMapAsync(venue.Id);

        var sectionId = await AddSeatingSectionAsync(
            seatingMap.Id,
            "VIP",
            isNumbered: true,
            capacity: 100);

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
            $"{ApiRoutes.SeatingSeats(
                seatingMap.Id,
                sectionId,
                rowId)}/{seatId}/status",
            new UpdateSeatingSeatStatusRequest(false),
            TestContext.Current.CancellationToken);

        // Act
        var response = await Client.PatchAsJsonAsync(
            $"{ApiRoutes.SeatingSeats(
                seatingMap.Id,
                sectionId,
                rowId)}/{seatId}/status",
            new UpdateSeatingSeatStatusRequest(true),
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var structure =
            await GetSeatingStructureAsync(seatingMap.Id);

        var seat = structure.Sections
            .Single(x => x.Id == sectionId)
            .Rows
            .Single(x => x.Id == rowId)
            .Seats
            .Single(x => x.Id == seatId);

        seat.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateStatus_WhenAlreadyInactive_DeactivatesSeatWithoutError()
    {
        // Arrange
        var venue = await CreateVenueAsync();
        var seatingMap = await CreateSeatingMapAsync(venue.Id);

        var sectionId = await AddSeatingSectionAsync(
            seatingMap.Id,
            "VIP",
            isNumbered: true,
            capacity: 100);

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
            $"{ApiRoutes.SeatingSeats(
                seatingMap.Id,
                sectionId,
                rowId)}/{seatId}/status",
            new UpdateSeatingSeatStatusRequest(false),
            TestContext.Current.CancellationToken);

        // Act
        var response = await Client.PatchAsJsonAsync(
            $"{ApiRoutes.SeatingSeats(
                seatingMap.Id,
                sectionId,
                rowId)}/{seatId}/status",
            new UpdateSeatingSeatStatusRequest(false),
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var structure =
            await GetSeatingStructureAsync(seatingMap.Id);

        var seat = structure.Sections
            .Single(x => x.Id == sectionId)
            .Rows
            .Single(x => x.Id == rowId)
            .Seats
            .Single(x => x.Id == seatId);

        seat.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateStatus_WhenAlreadyActive_ActivatesSeatWithoutError()
    {
        // Arrange
        var venue = await CreateVenueAsync();
        var seatingMap = await CreateSeatingMapAsync(venue.Id);

        var sectionId = await AddSeatingSectionAsync(
            seatingMap.Id,
            "VIP",
            isNumbered: true,
            capacity: 100);

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
        var response = await Client.PatchAsJsonAsync(
            $"{ApiRoutes.SeatingSeats(
                seatingMap.Id,
                sectionId,
                rowId)}/{seatId}/status",
            new UpdateSeatingSeatStatusRequest(true),
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var structure =
            await GetSeatingStructureAsync(seatingMap.Id);

        var seat = structure.Sections
            .Single(x => x.Id == sectionId)
            .Rows
            .Single(x => x.Id == rowId)
            .Seats
            .Single(x => x.Id == seatId);

        seat.IsActive.Should().BeTrue();
    }

    #endregion
}