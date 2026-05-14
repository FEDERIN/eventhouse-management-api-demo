using EventHouse.Management.Api.Contracts.ArtistPerformances;
using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.EventVenueCalendars;
using EventHouse.Management.Api.Tests.Abstractions;
using EventHouse.Management.Api.Tests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace EventHouse.Management.Api.Tests.Controllers;

public sealed class EventVenueCalendarsControllerTests(CustomWebApplicationFactory factory)
    : BaseIntegrationTest(factory)
{
    #region SECURITY

    [Fact]
    public async Task GetArtistPerformances_WithoutToken_Returns401Unauthorized()
    {
        // Act
        var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrlEventVenueCalendars}/{Guid.NewGuid()}/artist-performances").WithoutAuthentication();

        var res = await Client.SendAsync(request, TestContext.Current.CancellationToken);

        res.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    #endregion

    #region READ (GET)

    [Fact]
    public async Task GetById_WhenExists_Returns200OK()
    {
        var existing = await CreateEventVenueCalendarAsync();

        var response = await Client.GetAsync($"{BaseUrlEventVenueCalendars}/{existing.Id}", cancellationToken: TestContext.Current.CancellationToken);
        var returned = await response.ReadContentAsync<EventVenueCalendarResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        returned.Should().BeEquivalentTo(existing);
    }

    [Fact]
    public async Task GetById_WhenMissing_Returns404NotFound()
    {
        var response = await Client.GetAsync($"{BaseUrlEventVenueCalendars}/{Guid.NewGuid()}", TestContext.Current.CancellationToken);

        await response.ShouldBeProblemJson(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAll_WhenMultiple_Returns200OK_WithPagedResult()
    {
        // Arrange
        await CreateEventVenueCalendarAsync();
        await CreateEventVenueCalendarAsync();

        // Act
        var response = await Client.GetAsync(BaseUrlEventVenueCalendars, cancellationToken: TestContext.Current.CancellationToken);
        var pagedResult = await response.ReadContentAsync<PagedResult<EventVenueCalendarResponse>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        pagedResult.Items.Should().HaveCountGreaterThanOrEqualTo(2);
        pagedResult.TotalCount.Should().BeGreaterThanOrEqualTo(2);
    }

    [Fact]
    public async Task GetArtistPerformances_WhenMultipleExist_Returns200OK_WithPagedResult()
    {
        var calendar = await CreateEventVenueCalendarAsync(startDate: DateTime.UtcNow, endDate: DateTime.UtcNow.AddHours(5));

        await AddArtistToCalendarAsync(calendar.Id, isHeadliner: true,
            start: calendar.StartDate, end: calendar.StartDate.AddHours(1));

        await AddArtistToCalendarAsync(calendar.Id, isHeadliner: false,
            start: calendar.StartDate.AddHours(1), end: calendar.StartDate.AddHours(2));

        await AddArtistToCalendarAsync(calendar.Id, isHeadliner: false,
            start: calendar.StartDate.AddHours(2), end: calendar.StartDate.AddHours(3));

        var response = await Client.GetAsync($"{BaseUrlEventVenueCalendars}/{calendar.Id}/artist-performances?Page=1&PageSize=10", cancellationToken: TestContext.Current.CancellationToken);

        var pagedResult = await response.ReadContentAsync<PagedResult<ArtistPerformanceResponse>>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        pagedResult.Items.Should().HaveCount(3);
        pagedResult.TotalCount.Should().Be(3);

        pagedResult.Items.Should().ContainSingle(p => p.IsHeadliner);
    }

    [Fact]
    public async Task GetArtistPerformances_WhenCalendarMissing_Returns404NotFound()
    {
        var response = await Client.GetAsync($"{BaseUrlEventVenueCalendars}/{Guid.NewGuid()}/artist-performances", TestContext.Current.CancellationToken);

        await response.ShouldBeProblemJson(HttpStatusCode.NotFound);
    }

    #endregion

    #region CREATE (POST)

    [Fact]
    public async Task Create_WhenValid_Returns201Created()
    {
        var request = await CreateEventVenueCalendarRequestAsync();

        var response = await Client.PostAsJsonAsync(BaseUrlEventVenueCalendars, request, cancellationToken: TestContext.Current.CancellationToken);
        var created = await response.ReadContentAsync<EventVenueCalendarResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();

        created.Should().BeEquivalentTo(request, opt => opt.ExcludingMissingMembers());
    }


    [Fact]
    public async Task Create_WhenEventVenueNotFoundInHandler_Returns404NotFound()
    {
        var nonExistentVenueId = Guid.NewGuid();
        var request = await CreateEventVenueCalendarRequestAsync(eventVenueId: nonExistentVenueId);

        // Act
        var response = await Client.PostAsJsonAsync(BaseUrlEventVenueCalendars, request, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        await response.ShouldBeProblemJson(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_WhenSeatingMapMissing_Returns404NotFound()
    {

        var request = await CreateEventVenueCalendarRequestAsync(seatingMapId: Guid.Empty);

        var res2 = await Client.PostAsJsonAsync(BaseUrlEventVenueCalendars, request, cancellationToken: TestContext.Current.CancellationToken);
        await res2.ShouldBeProblemJson(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_WhenSlotIsOccupied_Returns409Conflict()
    {
        var existing = await CreateEventVenueCalendarAsync();

        var request = new CreateEventVenueCalendarRequest
        {
            EventVenueId = existing.EventVenueId,
            SeatingMapId = existing.SeatingMapId,
            Status = EventVenueCalendarStatus.Draft,
            StartDate = existing.StartDate,
            EndDate = existing.EndDate,
            TimeZoneId = existing.TimeZoneId
        };

        // Act
        var response = await Client.PostAsJsonAsync(BaseUrlEventVenueCalendars, request, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        await response.ShouldHaveErrorCode(HttpStatusCode.Conflict, "CALENDAR_SLOT_OCCUPIED");
    }

    [Fact]
    public async Task Create_WhenSlotOverlapsPartially_Returns409Conflict()
    {

        var start = DateTimeOffset.UtcNow.AddDays(1).Date.AddHours(10);
        var end = start.AddHours(2);
        var existing = await CreateEventVenueCalendarAsync(startDate: start, endDate: end);

        var request = new CreateEventVenueCalendarRequest {
            EventVenueId = existing.EventVenueId,
            SeatingMapId = existing.SeatingMapId,
            Status = EventVenueCalendarStatus.Published,
            StartDate = start.AddHours(1),
            EndDate = end.AddHours(1),
            TimeZoneId = existing.TimeZoneId
        };

        // Act
        var response = await Client.PostAsJsonAsync(BaseUrlEventVenueCalendars, request, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        await response.ShouldBeProblemJson(HttpStatusCode.Conflict);
    }

    #endregion

    #region UPDATE (PUT)

    [Fact]
    public async Task Update_WhenExists_Returns204NoContent()
    {
        var existing = await CreateEventVenueCalendarAsync();

        var updateRequest = new UpdateEventVenueCalendarRequest {
            StartDate = DateTime.UtcNow,
            EndDate = null,
            Status = EventVenueCalendarStatus.Cancelled
        };

        var response = await Client.PutAsJsonAsync($"{BaseUrlEventVenueCalendars}/{existing.Id}", updateRequest, cancellationToken: TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }


    [Fact]
    public async Task Update_WhenMissing_Returns404NotFound()
    {
        var updateRequest = new UpdateEventVenueCalendarRequest {
            StartDate = DateTime.UtcNow,
            EndDate = null,
            Status = EventVenueCalendarStatus.Published
        };

        var response = await Client.PutAsJsonAsync($"{BaseUrlEventVenueCalendars}/{Guid.NewGuid()}", updateRequest, cancellationToken: TestContext.Current.CancellationToken);

        await response.ShouldBeProblemJson(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_WhenNewSlotIsOccupiedByAnother_Returns409Conflict()
    {
        var first = await CreateEventVenueCalendarAsync(
            startDate: DateTimeOffset.UtcNow.AddDays(1),
            endDate: DateTimeOffset.UtcNow.AddDays(1).AddHours(2));

        var second = await CreateEventVenueCalendarAsync(
            eventVenueId: first.EventVenueId,
            seatingMapId: first.SeatingMapId,
            startDate: DateTimeOffset.UtcNow.AddDays(4),
            endDate: DateTimeOffset.UtcNow.AddDays(4).AddHours(2));

        var updateRequest = new UpdateEventVenueCalendarRequest
        {
            StartDate = first.StartDate,
            EndDate = first.EndDate,
            Status = EventVenueCalendarStatus.Published
        };

        // Act
        var response = await Client.PutAsJsonAsync($"{BaseUrlEventVenueCalendars}/{second.Id}", updateRequest, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        await response.ShouldHaveErrorCode(HttpStatusCode.Conflict, "CALENDAR_SLOT_OCCUPIED");
    }

    [Fact]
    public async Task Update_SameSlot_Returns204NoContent()
    {
        var existing = await CreateEventVenueCalendarAsync();

        var updateRequest = new UpdateEventVenueCalendarRequest
        {
            StartDate = existing.StartDate,
            EndDate = existing.EndDate,
            Status = EventVenueCalendarStatus.Cancelled
        };

        // Act
        var response = await Client.PutAsJsonAsync($"{BaseUrlEventVenueCalendars}/{existing.Id}", updateRequest, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Update_ToPublished_WhenArtistDatesAreMissing_Returns409Conflict()
    {
        var eventVenueCalendar = await CreateEventVenueCalendarAsync();

        var artist = await CreateArtistAsync();
        var addPerformanceRequest = new CreateArtistPerformanceRequest
        {
            ArtistId = artist.Id,
            IsHeadliner = true,
            SetStart = null,
            SetEnd = null
        };

        var addResponse = await Client.PostAsJsonAsync(
            $"{BaseUrlEventVenueCalendars}/{eventVenueCalendar.Id}/artist-performances",
            addPerformanceRequest,
            cancellationToken: TestContext.Current.CancellationToken);
        addResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var updateRequest = new UpdateEventVenueCalendarRequest
        {
            StartDate = eventVenueCalendar.StartDate,
            EndDate = eventVenueCalendar.EndDate,
            Status = EventVenueCalendarStatus.Published
        };

        var response = await Client.PutAsJsonAsync($"{BaseUrlEventVenueCalendars}/{eventVenueCalendar.Id}", updateRequest, cancellationToken: TestContext.Current.CancellationToken);

        await response.ShouldHaveErrorCode(HttpStatusCode.Conflict, "REQUIRED_DATES");
    }
    #endregion

    #region ARTIST PERFORMANCES (POST & PATCH)

    [Fact]
    public async Task AddPerformance_WhenValid_Returns201Created()
    {
        // Arrange
        var calendar = await CreateEventVenueCalendarAsync(startDate: DateTimeOffset.UtcNow.AddDays(4), endDate: DateTimeOffset.UtcNow.AddDays(4).AddHours(5));

        var artist = await CreateArtistAsync();

        var request = new CreateArtistPerformanceRequest
        {
            ArtistId = artist.Id,
            IsHeadliner = true,
            SetStart = DateTimeOffset.UtcNow.AddDays(4).AddMinutes(15),
            SetEnd = DateTimeOffset.UtcNow.AddDays(4).AddMinutes(30)
        };

        // Act
        var response = await Client.PostAsJsonAsync($"{BaseUrlEventVenueCalendars}/{calendar.Id}/artist-performances", request, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var created = await response.ReadContentAsync<ArtistPerformanceResponse>();
        created.ArtistId.Should().Be(artist.Id);
        created.IsHeadliner.Should().BeTrue();

        response.Headers.Location.Should().NotBeNull();
        response.Headers.Location.ToString().Should().Contain($"api/v1/artist-performances/");
    }

    [Fact]
    public async Task AddPerformance_WhenOverlapInSameCalendar_Returns409Conflict()
    {
        var calendar = await CreateEventVenueCalendarAsync(startDate: DateTimeOffset.UtcNow.AddDays(4), endDate: DateTimeOffset.UtcNow.AddDays(4).AddHours(5));
        await AddArtistToCalendarAsync(calendar.Id, isHeadliner: true,
            start: DateTimeOffset.UtcNow.AddDays(4), end: DateTimeOffset.UtcNow.AddDays(4).AddHours(1));

        var artist = await CreateArtistAsync();

        var request = new CreateArtistPerformanceRequest
        {
            ArtistId = artist.Id,
            SetStart = DateTimeOffset.UtcNow.AddDays(4).AddMinutes(15),
            SetEnd = DateTimeOffset.UtcNow.AddDays(4).AddMinutes(30)
        };
        var response = await Client.PostAsJsonAsync($"{BaseUrlEventVenueCalendars}/{calendar.Id}/artist-performances", request, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        await response.ShouldBeProblemJson(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task AddPerformance_WhenPublishedAndDatesMissing_Returns409Conflict()
    {
        var calendar = await CreateEventVenueCalendarAsync(startDate: DateTimeOffset.UtcNow.AddDays(1), status: EventVenueCalendarStatus.Published);
        var artist = await CreateArtistAsync();

        var request = new CreateArtistPerformanceRequest
        {
            ArtistId = artist.Id,
            IsHeadliner = false,
            SetStart = null,
            SetEnd = null
        };

        var response = await Client.PostAsJsonAsync($"{BaseUrlEventVenueCalendars}/{calendar.Id}/artist-performances", request, cancellationToken: TestContext.Current.CancellationToken);

        await response.ShouldBeProblemJson(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task AddPerformance_WhenArtistDuplicated_Returns409Conflict()
    {
        var calendar = await CreateEventVenueCalendarAsync(startDate: DateTimeOffset.UtcNow.AddDays(4), endDate: DateTimeOffset.UtcNow.AddDays(4).AddHours(5));
        var artistPerformance = await AddArtistToCalendarAsync(calendar.Id, isHeadliner: true,
            start: DateTimeOffset.UtcNow.AddDays(4), end: DateTimeOffset.UtcNow.AddDays(4).AddHours(1));

        var request = new CreateArtistPerformanceRequest
        {
            ArtistId = artistPerformance.ArtistId,
            IsHeadliner = false,
            SetStart = DateTimeOffset.UtcNow.AddDays(4).AddMinutes(15),
            SetEnd = DateTimeOffset.UtcNow.AddDays(4).AddMinutes(30)
        };

        var response = await Client.PostAsJsonAsync($"{BaseUrlEventVenueCalendars}/{calendar.Id}/artist-performances", request, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        await response.ShouldBeProblemJson(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task AddPerformance_WhenDuplicateHeadlinerException_Returns409Conflict()
    {
        var calendar = await CreateEventVenueCalendarAsync(startDate: DateTimeOffset.UtcNow.AddDays(4), endDate: DateTimeOffset.UtcNow.AddDays(4).AddHours(5));
        await AddArtistToCalendarAsync(calendar.Id, isHeadliner: true,
            start: DateTimeOffset.UtcNow.AddDays(4), end: DateTimeOffset.UtcNow.AddDays(4).AddHours(1));
        var anotherArtist = await CreateArtistAsync();
        var request = new CreateArtistPerformanceRequest
        {
            ArtistId = anotherArtist.Id,
            IsHeadliner = true,
            SetStart = DateTimeOffset.UtcNow.AddDays(4).AddMinutes(15),
            SetEnd = DateTimeOffset.UtcNow.AddDays(4).AddMinutes(30)
        };
        var response = await Client.PostAsJsonAsync($"{BaseUrlEventVenueCalendars}/{calendar.Id}/artist-performances", request, cancellationToken: TestContext.Current.CancellationToken);
        // Assert
        await response.ShouldHaveErrorCode(HttpStatusCode.Conflict, "DUPLICATE_HEADLINER");
    }


    [Fact]
    public async Task SwapHeadliner_WhenValid_Returns204NoContent()
    {
        // Arrange
        var calendar = await CreateEventVenueCalendarAsync();
        var headliner = await AddArtistToCalendarAsync(calendar.Id, isHeadliner: true);
        var support = await AddArtistToCalendarAsync(calendar.Id, isHeadliner: false);

        var request = new SwapHeadlinerRequest
        {
            OldArtistId = headliner.ArtistId,
            NewArtistId = support.ArtistId
        };

        // Act
        var response = await Client.PatchAsJsonAsync($"{BaseUrlEventVenueCalendars}/{calendar.Id}/artist-performances/swap-headliner", request, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    #endregion

    #region UPDATE PERFORMANCE TIMES (PATCH)

    [Fact]
    public async Task UpdatePerformanceTimes_WhenValid_Returns204NoContent()
    {
        var calendar = await CreateEventVenueCalendarAsync(startDate: DateTimeOffset.UtcNow.AddDays(1), endDate: DateTimeOffset.UtcNow.AddDays(10));
        var performance = await AddArtistToCalendarAsync(calendar.Id,
            start: calendar.StartDate,
            end: calendar.StartDate.AddHours(1));

        var newStart = calendar.StartDate.AddMinutes(30);
        var newEnd = calendar.StartDate.AddMinutes(90);
        var request = new UpdatePerformanceDatesRequest
        {
            SetStart = newStart,
            SetEnd = newEnd
        };

        // 2. Act
        var response = await Client.PatchAsJsonAsync(
            $"{BaseUrlEventVenueCalendars}/{calendar.Id}/artist-performances/{performance.ArtistId}/times",
            request, cancellationToken: TestContext.Current.CancellationToken);

        // 3. Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task UpdatePerformanceTimes_WhenOverlap_Returns409Conflict()
    {
        // 1. Arrange: Two artists in the same calendar with non-overlapping times
        var calendar = await CreateEventVenueCalendarAsync();

        // Artist A: 10:00 - 11:00
        _ = await AddArtistToCalendarAsync(calendar.Id,
            start: calendar.StartDate, end: calendar.StartDate.AddHours(1));

        // Artist B: 11:00 - 12:00
        var artistB = await AddArtistToCalendarAsync(calendar.Id,
            start: calendar.StartDate.AddHours(1), end: calendar.StartDate.AddHours(2));

        // 2. Act: Try to update Artist B's performance to overlap with Artist A (e.g., 10:30 - 11:30)
        var request = new UpdatePerformanceDatesRequest
        {
            SetStart = calendar.StartDate.AddMinutes(30),
            SetEnd = calendar.StartDate.AddHours(2)
        };


        var response = await Client.PatchAsJsonAsync(
            $"{BaseUrlEventVenueCalendars}/{calendar.Id}/artist-performances/{artistB.ArtistId}/times",
            request, cancellationToken: TestContext.Current.CancellationToken);

        await response.ShouldHaveErrorCode(HttpStatusCode.Conflict, "STAGE_OVERLAP");
    }

    [Fact]
    public async Task UpdatePerformanceTimes_WhenExceedsCalendarBounds_Returns400BadRequest()
    {
        var start = DateTimeOffset.UtcNow.AddDays(1);
        var end = start.AddHours(2);
        var calendar = await CreateEventVenueCalendarAsync(startDate: start, endDate: end);
        var performance = await AddArtistToCalendarAsync(calendar.Id, start: start, end: start.AddHours(1));

        var request = new UpdatePerformanceDatesRequest
        {
            SetStart = start.AddHours(1),
            SetEnd = start.AddHours(3)
        };

        var response = await Client.PatchAsJsonAsync(
            $"{BaseUrlEventVenueCalendars}/{calendar.Id}/artist-performances/{performance.ArtistId}/times",
            request, cancellationToken: TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion
}
