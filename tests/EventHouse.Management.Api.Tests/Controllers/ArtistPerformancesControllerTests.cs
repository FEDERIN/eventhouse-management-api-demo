using EventHouse.Management.Api.Contracts.ArtistPerformances;
using EventHouse.Management.Api.Contracts.EventVenueCalendars;
using EventHouse.Management.Api.Tests.Abstractions;
using EventHouse.Management.Api.Tests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace EventHouse.Management.Api.Tests.Controllers;

public sealed class ArtistPerformancesControllerTests(CustomWebApplicationFactory factory)
    : BaseIntegrationTest(factory)
{

    #region READ (GET)

    [Fact]
    public async Task GetById_WhenExists_Returns200OK()
    {
        // Arrange
        var calendar = await CreateEventVenueCalendarAsync();
        var performance = await AddArtistToCalendarAsync(calendar.Id);

        // Act
        var response = await Client.GetAsync($"{BaseUrlArtistPerformances}/{performance.Id}", TestContext.Current.CancellationToken);
        var returned = await response.ReadContentAsync<ArtistPerformanceResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        returned.Id.Should().Be(performance.Id);
        returned.ArtistId.Should().Be(performance.ArtistId);
    }
    #endregion

    #region DELETE

    [Fact]
    public async Task RemovePerformance_WhenValid_Returns204NoContent()
    {
        var calendar = await CreateEventVenueCalendarAsync();
        var performance = await AddArtistToCalendarAsync(calendar.Id, isHeadliner: false);

        // Act
        var response = await Client.DeleteAsync($"{BaseUrlArtistPerformances}/{calendar.Id}/{performance.ArtistId}", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getRes = await Client.GetAsync($"{BaseUrlArtistPerformances}/{performance.Id}", TestContext.Current.CancellationToken);
        getRes.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task RemovePerformance_WhenHeadlinerInPublishedCalendar_Returns409Conflict()
    {
        var calendar = await CreateEventVenueCalendarAsync(
            startDate: DateTimeOffset.UtcNow.AddDays(10));

        var headliner = await AddArtistToCalendarAsync(calendar.Id, isHeadliner: true,
            start: calendar.StartDate, end: calendar.StartDate.AddHours(1));

        var updateRequest = new UpdateEventVenueCalendarRequest
        {
            StartDate = calendar.StartDate,
            EndDate = calendar.EndDate,
            Status = EventVenueCalendarStatus.Published
        };

        await Client.PutAsJsonAsync($"{BaseUrlEventVenueCalendars}/{calendar.Id}", updateRequest, TestContext.Current.CancellationToken);


        var response = await Client.DeleteAsync($"{BaseUrlArtistPerformances}/{calendar.Id}/{headliner.ArtistId}", TestContext.Current.CancellationToken);

        await response.ShouldHaveErrorCode(HttpStatusCode.Conflict, "CANNOT_REMOVE_PUBLISHED_HEADLINER");
    }

    [Fact]
    public async Task RemovePerformance_WhenCalendarNotFound_Returns404NotFound()
    {
        // Act
        var response = await Client.DeleteAsync($"{BaseUrlArtistPerformances}/{Guid.NewGuid()}/{Guid.NewGuid()}", TestContext.Current.CancellationToken);

        // Assert
        await response.ShouldBeProblemJson(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task RemovePerformance_WhenArtistNotInCalendar_Returns204NoContent()
    {
        var calendar = await CreateEventVenueCalendarAsync();
        // Act
        var response = await Client.DeleteAsync($"{BaseUrlArtistPerformances}/{calendar.Id}/{Guid.NewGuid()}", TestContext.Current.CancellationToken);
        // Assert
        await response.ShouldBeProblemJson(HttpStatusCode.NoContent);
    }

    #endregion
}