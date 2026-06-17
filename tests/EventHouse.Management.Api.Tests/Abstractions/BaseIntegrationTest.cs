using EventHouse.Management.Api.Contracts.ArtistPerformances;
using EventHouse.Management.Api.Contracts.Artists;
using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.Events;
using EventHouse.Management.Api.Contracts.EventVenueCalendars;
using EventHouse.Management.Api.Contracts.EventVenues;
using EventHouse.Management.Api.Contracts.Genres;
using EventHouse.Management.Api.Contracts.SeatingMaps;
using EventHouse.Management.Api.Contracts.Venues;
using EventHouse.Management.Api.Tests.Common;
using EventHouse.Management.Api.Tests.Factories;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace EventHouse.Management.Api.Tests.Abstractions;

public abstract class BaseIntegrationTest(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    protected readonly CustomWebApplicationFactory Factory = factory;
    protected readonly HttpClient Client = factory.CreateDefaultClient(new AuthedHandler(factory));

    #region Base URLs
    protected const string BaseUrlGenres = ApiRoutes.Genres;
    protected const string BaseUrlArtists = ApiRoutes.Artists;
    protected const string BaseUrlVenues = ApiRoutes.Venues;
    protected const string BaseUrlSeatingMaps = ApiRoutes.SeatingMaps;
    protected const string BaseUrlEvents = ApiRoutes.Events;
    protected const string BaseUrlEventVenues = ApiRoutes.EventVenues;
    protected const string BaseUrlEventVenueCalendars = ApiRoutes.EventVenueCalendars;
    protected const string BaseUrlArtistPerformances = ApiRoutes.ArtistPerformances;
    #endregion

    #region Factory Helpers
    protected async Task<ArtistDetail> CreateArtistAsync(string? name = null, ArtistCategory? category = null)
    {
        var request = ArtistFactory.CreateRequest(name, category);
        var response = await Client.PostAsJsonAsync(BaseUrlArtists, request);
        return await response.ReadContentAsync<ArtistDetail>();
    }

    protected async Task<GenreResponse> CreateGenreAsync(string? name = null, ArtistCategory? forCategory = null)
    {
        var genreName = name ?? (forCategory.HasValue ? ArtistFactory.GetRandomGenreForCategory(forCategory.Value) : "General Rock");
        var uniqueName = $"{genreName} {Guid.NewGuid().ToString()[..4]}";

        var response = await Client.PostAsJsonAsync(BaseUrlGenres, new CreateGenreRequest { Name = uniqueName });
        return await response.ReadContentAsync<GenreResponse>();
    }

    protected async Task<EventResponse> CreateEventAsync(string? name = null, string? description = null, EventScope? scope = EventScope.National)
    {
        var request = EventFactory.CreateRequest(name, description, scope);
        var response = await Client.PostAsJsonAsync(BaseUrlEvents, request);
        return await response.ReadContentAsync<EventResponse>();
    }

    protected async Task<VenueResponse> CreateVenueAsync(string? name = null, string? city = null, int? capacity = null)
    {
        var request = VenueFactory.CreateRequest(name, city, capacity);
        var response = await Client.PostAsJsonAsync(BaseUrlVenues, request);
        return await response.ReadContentAsync<VenueResponse>();
    }

    protected async Task<SeatingMapResponse> CreateSeatingMapAsync(Guid? venueId = null, string? name = null, bool isActive = true)
    {
        var request = SeatingMapFactory.CreateRequest(venueId, name, isActive);
        var response = await Client.PostAsJsonAsync(BaseUrlSeatingMaps, request);
        return await response.ReadContentAsync<SeatingMapResponse>();
    }

    protected async Task<EventVenueResponse> CreateEventVenueAsync()
    {
        var eventResponse = await CreateEventAsync();
        var venueResponse = await CreateVenueAsync();

        var request = new CreateEventVenueRequest
        {
            EventId = eventResponse.Id,
            VenueId = venueResponse.Id,
            Status = EventVenueStatus.Active
        };

        var response = await Client.PostAsJsonAsync(BaseUrlEventVenues, request);
        return await response.ReadContentAsync<EventVenueResponse>();
    }

    protected async Task<EventVenueCalendarResponse> CreateEventVenueCalendarAsync(
        Guid? eventVenueId = null,
        Guid? seatingMapId = null,
        DateTimeOffset? startDate = null,
        DateTimeOffset? endDate = null,
        EventVenueCalendarStatus status = EventVenueCalendarStatus.Draft)
    {
        var request = await CreateEventVenueCalendarRequestAsync(eventVenueId, seatingMapId, startDate, endDate, status);
        var response = await Client.PostAsJsonAsync(BaseUrlEventVenueCalendars, request);
        return await response.ReadContentAsync<EventVenueCalendarResponse>();
    }

    protected async Task<CreateEventVenueCalendarRequest> CreateEventVenueCalendarRequestAsync(
        Guid? eventVenueId = null,
        Guid? seatingMapId = null,
        DateTimeOffset? startDate = null,
        DateTimeOffset? endDate = null,
        EventVenueCalendarStatus status = EventVenueCalendarStatus.Draft)
    {
        var venueId = Guid.Empty;

        if (eventVenueId == null)
        {
            var eventVenue = await CreateEventVenueAsync();
            eventVenueId = eventVenue.Id;
            venueId = eventVenue.VenueId;
        }

        if (seatingMapId == null)
        {
            var seatingMap = await CreateSeatingMapAsync(venueId: venueId);
            seatingMapId = seatingMap.Id;
        }

        return new CreateEventVenueCalendarRequest
        {
            EventVenueId = eventVenueId.GetValueOrDefault(),
            SeatingMapId = seatingMapId.GetValueOrDefault(),
            Status = status,
            StartDate = startDate ?? DateTime.UtcNow,
            EndDate = endDate ?? (startDate?.AddHours(10) ?? DateTime.UtcNow.AddHours(10)),
            TimeZoneId = "America/New_York",
        };
    }

    protected async Task<ArtistPerformanceResponse> AddArtistToCalendarAsync(Guid calendarId, bool isHeadliner = false, DateTimeOffset? start = null, DateTimeOffset? end = null)
    {
        var artist = await CreateArtistAsync();
        var request = new CreateArtistPerformanceRequest { ArtistId = artist.Id, IsHeadliner = isHeadliner, SetStart = start, SetEnd = end };

        var response = await Client.PostAsJsonAsync($"{BaseUrlEventVenueCalendars}/{calendarId}/artist-performances", request);
        response.EnsureSuccessStatusCode();
        return await response.ReadContentAsync<ArtistPerformanceResponse>();
    }
    #endregion

    #region Assertions
    protected async Task AssertGetReturns404(string url) =>
        await (await Client.GetAsync(url, TestContext.Current.CancellationToken))
            .ShouldBeProblemJson(HttpStatusCode.NotFound);

    protected async Task PutAndAssertPersisted<TRequest, TResponse>(string url, TRequest request)
    {
        var response = await Client.PutAsJsonAsync(url, request, TestContext.Current.CancellationToken);
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var updated = await Client.GetFromJsonAsync<TResponse>(url, JsonTestOptions.Default, TestContext.Current.CancellationToken);
        updated.Should().BeEquivalentTo(request, options => options.ExcludingMissingMembers());
    }

    protected async Task AssertPutReturns404(string url, object request) =>
        await (await Client.PutAsJsonAsync(url, request, TestContext.Current.CancellationToken))
            .ShouldBeProblemJson(HttpStatusCode.NotFound);

    protected async Task AssertDeleteReturns204(string url)
    {
        var res = await Client.DeleteAsync(url, TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    #endregion

    #region Utilities
    protected static void ValidateOrder<T>(IEnumerable<T> values, SortDirection direction)
    {
        if (direction == SortDirection.Asc)
            values.Should().BeInAscendingOrder();
        else
            values.Should().BeInDescendingOrder();
    }
    #endregion
}