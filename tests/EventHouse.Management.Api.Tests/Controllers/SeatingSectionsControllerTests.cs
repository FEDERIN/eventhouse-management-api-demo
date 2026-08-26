using EventHouse.Management.Api.Contracts.Seating.Sections;
using EventHouse.Management.Api.Tests.Abstractions;
using EventHouse.Management.Api.Tests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace EventHouse.Management.Api.Tests.Controllers;

public sealed class SeatingSectionsControllerTests(
    CustomWebApplicationFactory factory)
    : BaseIntegrationTest(factory)
{
    #region ADD (POST)

    [Fact]
    public async Task Add_ReturnsNoContent()
    {
        // Arrange
        var venue = await CreateVenueAsync();
        var seatingMap = await CreateSeatingMapAsync(venue.Id);

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
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Add_WhenSectionNameAlreadyExists_Returns409()
    {
        // Arrange
        var venue = await CreateVenueAsync();
        var seatingMap = await CreateSeatingMapAsync(venue.Id);

        var request = new AddSeatingSectionRequest(
            "VIP",
            true,
            100);

        await Client.PostAsJsonAsync(
            ApiRoutes.SeatingSections(seatingMap.Id),
            request,
            TestContext.Current.CancellationToken);

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

    #region UPDATE (PUT)

    [Fact]
    public async Task Update_ReturnsNoContent()
    {
        // Arrange
        var venue = await CreateVenueAsync();
        var seatingMap = await CreateSeatingMapAsync(venue.Id);

        var sectionId = await AddSeatingSectionAsync(
            seatingMap.Id,
            "VIP",
            true,
            100);

        var request = new UpdateSeatingSectionRequest(
            "Premium",
            true,
            100);

        // Act
        var response = await Client.PutAsJsonAsync(
            $"{ApiRoutes.SeatingSections(seatingMap.Id)}/{sectionId}",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Update_WhenSectionNameAlreadyExists_Returns409()
    {
        // Arrange
        var venue = await CreateVenueAsync();
        var seatingMap = await CreateSeatingMapAsync(venue.Id);

        await AddSeatingSectionAsync(
            seatingMap.Id,
            "VIP",
            true,
            100);

        var sectionId = await AddSeatingSectionAsync(
            seatingMap.Id,
            "General",
            true,
            100);

        var request = new UpdateSeatingSectionRequest(
            "VIP",
            true,
            100);

        // Act
        var response = await Client.PutAsJsonAsync(
            $"{ApiRoutes.SeatingSections(seatingMap.Id)}/{sectionId}",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        await response.ShouldBeProblemJson(
            HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Update_WhenCapacityIsBelowSeatCount_Returns409()
    {
        // Arrange
        var venue = await CreateVenueAsync();
        var seatingMap = await CreateSeatingMapAsync(venue.Id);

        var sectionId = await AddSeatingSectionAsync(
            seatingMap.Id,
            "VIP",
            true,
            2);

        var rowId = await AddSeatingRowAsync(
            seatingMap.Id,
            sectionId,
            1,
            "A");

        await AddSeatingSeatAsync(
            seatingMap.Id,
            sectionId,
            rowId,
            1,
            "1");

        await AddSeatingSeatAsync(
            seatingMap.Id,
            sectionId,
            rowId,
            2,
            "2");

        var request = new UpdateSeatingSectionRequest(
            "VIP",
            true,
            1);

        // Act
        var response = await Client.PutAsJsonAsync(
            $"{ApiRoutes.SeatingSections(seatingMap.Id)}/{sectionId}",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        await response.ShouldBeProblemJson(
            HttpStatusCode.Conflict);
    }

    #endregion

    #region STATUS (PATCH)

    [Fact]
    public async Task UpdateStatus_WhenActive_DeactivatesSection()
    {
        // Arrange
        var venue = await CreateVenueAsync();
        var seatingMap = await CreateSeatingMapAsync(venue.Id);

        var sectionId = await AddSeatingSectionAsync(
            seatingMap.Id,
            "VIP",
            true,
            100);

        var request = new UpdateSeatingSectionStatusRequest(false);

        // Act
        var response = await Client.PatchAsJsonAsync(
            $"{ApiRoutes.SeatingSections(seatingMap.Id)}/{sectionId}/status",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var structure =
            await GetSeatingStructureAsync(seatingMap.Id);

        var section = structure.Sections
            .Single(x => x.Id == sectionId);

        section.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateStatus_WhenInactive_ActivatesSection()
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

        // Act
        var response = await Client.PatchAsJsonAsync(
            $"{ApiRoutes.SeatingSections(seatingMap.Id)}/{sectionId}/status",
            new UpdateSeatingSectionStatusRequest(true),
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var structure =
            await GetSeatingStructureAsync(seatingMap.Id);

        var section = structure.Sections
            .Single(x => x.Id == sectionId);

        section.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateStatus_WhenActive_DeactivatesSectionAndRows()
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
            $"{ApiRoutes.SeatingSections(seatingMap.Id)}/{sectionId}/status",
            new UpdateSeatingSectionStatusRequest(false),
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var structure =
            await GetSeatingStructureAsync(seatingMap.Id);

        var section = structure.Sections
            .Single(x => x.Id == sectionId);

        var row = section.Rows
            .Single(x => x.Id == rowId);

        section.IsActive.Should().BeFalse();
        row.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateStatus_WhenInactive_ActivatesSectionAndRows()
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
            $"{ApiRoutes.SeatingSections(seatingMap.Id)}/{sectionId}/status",
            new UpdateSeatingSectionStatusRequest(false),
            TestContext.Current.CancellationToken);

        // Act
        var response = await Client.PatchAsJsonAsync(
            $"{ApiRoutes.SeatingSections(seatingMap.Id)}/{sectionId}/status",
            new UpdateSeatingSectionStatusRequest(true),
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var structure =
            await GetSeatingStructureAsync(seatingMap.Id);

        var section = structure.Sections
            .Single(x => x.Id == sectionId);

        var row = section.Rows
            .Single(x => x.Id == rowId);

        section.IsActive.Should().BeTrue();
        row.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateStatus_WhenAlreadyInactive_DoesNothing()
    {
        // Arrange
        var venue = await CreateVenueAsync();
        var seatingMap = await CreateSeatingMapAsync(venue.Id);

        var sectionId = await AddSeatingSectionAsync(
            seatingMap.Id,
            "VIP",
            true,
            100);

        var firstResponse = await Client.PatchAsJsonAsync(
            $"{ApiRoutes.SeatingSections(seatingMap.Id)}/{sectionId}/status",
            new UpdateSeatingSectionStatusRequest(false),
            TestContext.Current.CancellationToken);

        firstResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Act
        var response = await Client.PatchAsJsonAsync(
            $"{ApiRoutes.SeatingSections(seatingMap.Id)}/{sectionId}/status",
            new UpdateSeatingSectionStatusRequest(false),
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }


    [Fact]
    public async Task UpdateStatus_WhenAlreadyActive_DoesNothing()
    {
        // Arrange
        var venue = await CreateVenueAsync();
        var seatingMap = await CreateSeatingMapAsync(venue.Id);

        var sectionId = await AddSeatingSectionAsync(
            seatingMap.Id,
            "VIP",
            true,
            100);

        // Act
        var response = await Client.PatchAsJsonAsync(
            $"{ApiRoutes.SeatingSections(seatingMap.Id)}/{sectionId}/status",
            new UpdateSeatingSectionStatusRequest(true),
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    #endregion
}