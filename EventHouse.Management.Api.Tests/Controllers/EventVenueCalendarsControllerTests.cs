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
    public async Task GetAll_WithoutToken_Returns401Unauthorized()
    {
        // Act
        var request = new HttpRequestMessage(HttpMethod.Get, BaseUrlEventVenueCalendars).WithoutAuthentication();

        var res = await Client.SendAsync(request);

        res.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    #endregion

    #region READ (GET)

    [Fact]
    public async Task GetById_WhenExists_Returns200OK()
    {
        var existing = await CreateEventVenueCalendarAsync();

        var response = await Client.GetAsync($"{BaseUrlEventVenueCalendars}/{existing.Id}");
        var returned = await response.ReadContentAsync<EventVenueCalendarResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        returned.Should().BeEquivalentTo(existing);
    }

    [Fact]
    public async Task GetById_WhenMissing_Returns404NotFound()
    {
        var response = await Client.GetAsync($"{BaseUrlEventVenueCalendars}/{Guid.NewGuid()}");

        await response.ShouldBeProblemJson(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAll_WhenMultiple_Returns200OK_WithPagedResult()
    {
        // Arrange
        await CreateEventVenueCalendarAsync();
        await CreateEventVenueCalendarAsync();

        // Act
        var response = await Client.GetAsync(BaseUrlEventVenueCalendars);
        var pagedResult = await response.ReadContentAsync<PagedResult<EventVenueCalendarResponse>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        pagedResult.Items.Should().HaveCountGreaterOrEqualTo(2);
        pagedResult.TotalCount.Should().BeGreaterOrEqualTo(2);
    }
    #endregion

    #region CREATE (POST)

    [Fact]
    public async Task Create_WhenValid_Returns201Created()
    {
        var request = await CreateEventVenueCalendarRequestAsync();

        var response = await Client.PostAsJsonAsync(BaseUrlEventVenueCalendars, request);
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
        var response = await Client.PostAsJsonAsync(BaseUrlEventVenueCalendars, request);

        // Assert
        await response.ShouldBeProblemJson(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_WhenSeatingMapMissing_Returns404NotFound()
    {

        var request = await CreateEventVenueCalendarRequestAsync(seatingMapId: Guid.Empty);

        var res2 = await Client.PostAsJsonAsync(BaseUrlEventVenueCalendars, request);
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
        var response = await Client.PostAsJsonAsync(BaseUrlEventVenueCalendars, request);

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
        var response = await Client.PostAsJsonAsync(BaseUrlEventVenueCalendars, request);

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

        var response = await Client.PutAsJsonAsync($"{BaseUrlEventVenueCalendars}/{existing.Id}", updateRequest);

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

        var response = await Client.PutAsJsonAsync($"{BaseUrlEventVenueCalendars}/{Guid.NewGuid()}", updateRequest);

        await response.ShouldBeProblemJson(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_WhenNewSlotIsOccupiedByAnother_Returns409Conflict()
    {
        // Arrange: Creamos dos calendarios en horarios distintos
        var first = await CreateEventVenueCalendarAsync(
            startDate: DateTimeOffset.UtcNow.AddDays(1),
            endDate: DateTimeOffset.UtcNow.AddDays(1).AddHours(2));

        var second = await CreateEventVenueCalendarAsync(
            eventVenueId: first.EventVenueId,
            seatingMapId: first.SeatingMapId,
            startDate: DateTimeOffset.UtcNow.AddDays(4),
            endDate: DateTimeOffset.UtcNow.AddDays(4).AddHours(2));

        // Intentamos mover el segundo al horario del primero
        var updateRequest = new UpdateEventVenueCalendarRequest
        {
            StartDate = first.StartDate,
            EndDate = first.EndDate,
            Status = EventVenueCalendarStatus.Published
        };

        // Act
        var response = await Client.PutAsJsonAsync($"{BaseUrlEventVenueCalendars}/{second.Id}", updateRequest);

        // Assert
        await response.ShouldHaveErrorCode(HttpStatusCode.Conflict, "CALENDAR_SLOT_OCCUPIED");
    }

    [Fact]
    public async Task Update_SameSlot_Returns204NoContent()
    {
        // Arrange: Creamos un calendario
        var existing = await CreateEventVenueCalendarAsync();

        // Enviamos el mismo horario (el repositorio debe excluir este ID de la validación)
        var updateRequest = new UpdateEventVenueCalendarRequest
        {
            StartDate = existing.StartDate,
            EndDate = existing.EndDate,
            Status = EventVenueCalendarStatus.Cancelled
        };

        // Act
        var response = await Client.PutAsJsonAsync($"{BaseUrlEventVenueCalendars}/{existing.Id}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    #endregion


}
