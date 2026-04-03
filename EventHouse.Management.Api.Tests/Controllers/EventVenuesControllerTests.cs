using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.EventVenues;
using EventHouse.Management.Api.Tests.Abstractions;
using EventHouse.Management.Api.Tests.Common;
using EventHouse.Management.Api.Tests.Factories;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace EventHouse.Management.Api.Tests.Controllers;

public sealed class EventVenuesControllerTests(CustomWebApplicationFactory factory)
    : BaseIntegrationTest(factory)
{
    private const string StatusPath = "status";

    #region SECURITY

    [Fact]
    public async Task GetAll_WithoutToken_Returns401Unauthorized()
    {
        // Act
        var request = new HttpRequestMessage(HttpMethod.Get, BaseUrlEventVenues).WithoutAuthentication();

        var res = await Client.SendAsync(request);

        res.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region READ (GET)

    [Fact]
    public async Task GetById_WhenExists_Returns200OK()
    {
        // Arrange
        var existing = await CreateEventVenueAsync();

        // Act
        var response = await Client.GetAsync($"{BaseUrlEventVenues}/{existing.Id}");
        var returned = await response.ReadContentAsync<EventVenueResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        returned.Should().BeEquivalentTo(existing);
    }

    [Fact]
    public async Task GetById_WhenMissing_Returns404NotFound()
    {
        // Act
        var response = await Client.GetAsync($"{BaseUrlEventVenues}/{Guid.NewGuid()}");

        // Assert
        await response.ShouldBeProblemJson(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAll_WhenMultiple_Returns200OK_WithPagedResult()
    {
        // Arrange
        await CreateEventVenueAsync();
        await CreateEventVenueAsync();

        // Act
        var response = await Client.GetAsync(BaseUrlEventVenues);
        var pagedResult = await response.ReadContentAsync<PagedResult<EventVenueResponse>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        pagedResult.Items.Should().HaveCountGreaterOrEqualTo(2);
        pagedResult.TotalCount.Should().BeGreaterOrEqualTo(2);
    }

    [Fact]
    public async Task GetAll_WhenFilteredByEventAndVenue_ShouldReturnEnrichedNames()
    {

        var eventVenue = await CreateEventVenueAsync();

        // Act
        // Filtramos por EventId y VenueId para disparar la lógica de "cached names" en el Handler
        var url = $"{BaseUrlEventVenues}?eventId={eventVenue.EventId}&venueId={eventVenue.VenueId}";
        var response = await Client.GetAsync(url);
        var pagedResult = await response.ReadContentAsync<PagedResult<EventVenueResponse>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        pagedResult.Items.Should().NotBeEmpty();

        var item = pagedResult.Items[0];

        item.EventId.Should().Be(eventVenue.EventId);
        item.EventName.Should().Be(eventVenue.EventName);

        item.VenueId.Should().Be(eventVenue.VenueId);
        item.VenueName.Should().Be(eventVenue.VenueName);
    }

    #endregion

    #region CREATE (POST)

    [Fact]
    public async Task Create_WhenValid_Returns201Created()
    {
        // Arrange
        var @event = await CreateEventAsync();
        var venue = await CreateVenueAsync();
        var request = EventVenueFactory.CreateRequest(@event.Id, venue.Id, EventVenueStatus.Active);

        // Act
        var response = await Client.PostAsJsonAsync(BaseUrlEventVenues, request);
        var created = await response.ReadContentAsync<EventVenueResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
        created.EventId.Should().Be(request.EventId);
        created.VenueId.Should().Be(request.VenueId);
    }

    [Fact]
    public async Task Create_WhenDuplicate_Returns409Conflict()
    {
        // Arrange
        var existing = await CreateEventVenueAsync();
        var request = new CreateEventVenueRequest
        {
            EventId = existing.EventId,
            VenueId = existing.VenueId,
            Status = EventVenueStatus.Active
        };

        // Act
        var response = await Client.PostAsJsonAsync(BaseUrlEventVenues, request);

        // Assert
        await response.ShouldBeProblemJson(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Create_WhenEventOrVenueMissing_Returns404NotFound()
    {
        // Arrange 1: Evento existe, Venue no
        var @event = await CreateEventAsync();
        var requestMissingVenue = EventVenueFactory.CreateRequest(@event.Id, Guid.NewGuid());

        // Act & Assert 1
        var res1 = await Client.PostAsJsonAsync(BaseUrlEventVenues, requestMissingVenue);
        await res1.ShouldBeProblemJson(HttpStatusCode.NotFound);

        // Arrange 2: Venue existe, Evento no
        var venue = await CreateVenueAsync();
        var requestMissingEvent = EventVenueFactory.CreateRequest(Guid.NewGuid(), venue.Id);

        // Act & Assert 2
        var res2 = await Client.PostAsJsonAsync(BaseUrlEventVenues, requestMissingEvent);
        await res2.ShouldBeProblemJson(HttpStatusCode.NotFound);
    }

    #endregion

    #region UPDATE (PUT)

    [Fact]
    public async Task UpdateStatus_WhenExists_Returns204NoContent()
    {
        // Arrange
        var existing = await CreateEventVenueAsync();
        var updateRequest = new UpdateEventVenueStatusRequest { Status = EventVenueStatus.Inactive };

        // Act
        var response = await Client.PutAsJsonAsync($"{BaseUrlEventVenues}/{existing.Id}/{StatusPath}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify: Volvemos a consultar para asegurar persistencia
        var verifyRes = await Client.GetAsync($"{BaseUrlEventVenues}/{existing.Id}");
        var updated = await verifyRes.ReadContentAsync<EventVenueResponse>();
        updated.Status.Should().Be(EventVenueStatus.Inactive);
    }

    [Fact]
    public async Task UpdateStatus_WhenCalledMultipleTimesWithSameStatus_Returns204NoContent()
    {
        // Arrange
        var existing = await CreateEventVenueAsync();
        var updateRequest = new UpdateEventVenueStatusRequest { Status = EventVenueStatus.Inactive };

        var response1 = await Client.PutAsJsonAsync($"{BaseUrlEventVenues}/{existing.Id}/{StatusPath}", updateRequest);
        var response2 = await Client.PutAsJsonAsync($"{BaseUrlEventVenues}/{existing.Id}/{StatusPath}", updateRequest);

        // Assert
        response1.StatusCode.Should().Be(HttpStatusCode.NoContent);
        response2.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verificación final en BD
        var verifyRes = await Client.GetAsync($"{BaseUrlEventVenues}/{existing.Id}");
        var updated = await verifyRes.ReadContentAsync<EventVenueResponse>();
        updated.Status.Should().Be(EventVenueStatus.Inactive);
    }

    [Fact]
    public async Task UpdateStatus_WhenMissing_Returns404NotFound()
    {
        // Arrange
        var updateRequest = new UpdateEventVenueStatusRequest { Status = EventVenueStatus.Inactive };

        // Act
        var response = await Client.PutAsJsonAsync($"{BaseUrlEventVenues}/{Guid.NewGuid()}/{StatusPath}", updateRequest);

        // Assert
        await response.ShouldBeProblemJson(HttpStatusCode.NotFound);
    }
    #endregion
}