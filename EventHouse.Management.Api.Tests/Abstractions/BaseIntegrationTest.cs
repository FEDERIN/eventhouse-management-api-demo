using EventHouse.Management.Api.Contracts.ArtistPerformances;
using EventHouse.Management.Api.Contracts.Artists;
using EventHouse.Management.Api.Contracts.Events;
using EventHouse.Management.Api.Contracts.EventVenueCalendars;
using EventHouse.Management.Api.Contracts.EventVenues;
using EventHouse.Management.Api.Contracts.Genres;
using EventHouse.Management.Api.Contracts.SeatingMaps;
using EventHouse.Management.Api.Contracts.Venues;
using EventHouse.Management.Api.Tests.Common;
using EventHouse.Management.Api.Tests.Factories;
using System.Net.Http.Json;

namespace EventHouse.Management.Api.Tests.Abstractions;

public abstract class BaseIntegrationTest(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    protected readonly CustomWebApplicationFactory Factory = factory;

    protected readonly HttpClient Client = factory.CreateDefaultClient(new AuthedHandler(factory));

    protected const string BaseUrlGenres = ApiRoutes.Genres;
    protected const string BaseUrlArtists = ApiRoutes.Artists;
    protected const string BaseUrlVenues = ApiRoutes.Venues;
    protected const string BaseUrlSeatingMaps = ApiRoutes.SeatingMaps;
    protected const string BaseUrlEventVenues = ApiRoutes.EventVenues;
    protected const string BaseUrlEventVenueCalendars = ApiRoutes.EventVenueCalendars;
    protected const string BaseUrlArtistPerformances = ApiRoutes.ArtistPerformances;

    protected async Task<ArtistDetail> CreateArtistAsync(string? name = null, ArtistCategory? category = null)
    {
        var request = ArtistFactory.CreateRequest(name, category);
        var response = await Client.PostAsJsonAsync(BaseUrlArtists, request);
        return await response.ReadContentAsync<ArtistDetail>();
    }

    protected async Task<GenreResponse> CreateGenreAsync(string? name = null, ArtistCategory? forCategory = null)
    {
        var genreName = name ?? (forCategory.HasValue
            ? ArtistFactory.GetRandomGenreForCategory(forCategory.Value)
            : "General Rock");

        var uniqueName = $"{genreName} {Guid.NewGuid().ToString()[..4]}";

        var response = await Client.PostAsJsonAsync(BaseUrlGenres, new CreateGenreRequest { Name = uniqueName });
        return await response.ReadContentAsync<GenreResponse>();
    }

    protected async Task<EventResponse> CreateEventAsync(string? name = null, string? description = null, EventScope? scope = EventScope.National)
    {
        var request = EventFactory.CreateRequest(name, description, scope);
        var response = await Client.PostAsJsonAsync(ApiRoutes.Events, request);
        return await response.ReadContentAsync<EventResponse>();
    }

    protected async Task<VenueResponse> CreateVenueAsync(string? name = null)
    {
        var request = VenueFactory.CreateRequest(name);
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

    protected async Task<EventVenueCalendarResponse> CreateEventVenueCalendarAsync(Guid? eventVenueId = null,
        Guid? seatingMapId = null, DateTimeOffset ? startDate = null, DateTimeOffset? endDate = null,
        EventVenueCalendarStatus status = EventVenueCalendarStatus.Draft)
    {
        CreateEventVenueCalendarRequest request = await CreateEventVenueCalendarRequestAsync(eventVenueId: eventVenueId, seatingMapId: seatingMapId, startDate: startDate, endDate: endDate, status: status);

        var response = await Client.PostAsJsonAsync(BaseUrlEventVenueCalendars, request);
        return await response.ReadContentAsync<EventVenueCalendarResponse>();
    }

    protected async Task<CreateEventVenueCalendarRequest> CreateEventVenueCalendarRequestAsync
        (Guid? eventVenueId = null, Guid? seatingMapId = null, DateTimeOffset? startDate = null,
        DateTimeOffset? endDate = null, EventVenueCalendarStatus status = EventVenueCalendarStatus.Draft)
    {

        var venueId = new Guid();

        if (eventVenueId == null || !eventVenueId.HasValue)
        {
            var eventVenue = await CreateEventVenueAsync();
            eventVenueId = eventVenue.Id;
            venueId = eventVenue.VenueId;
        }

        if (seatingMapId == null || !seatingMapId.HasValue) {
            var seatingMap = await CreateSeatingMapAsync(venueId: venueId);
            seatingMapId = seatingMap.Id;
        }

        endDate =  endDate ?? (startDate.HasValue ? startDate.Value.AddHours(10) : DateTime.UtcNow.AddHours(10));

        var request = new CreateEventVenueCalendarRequest
        {
            EventVenueId = eventVenueId.GetValueOrDefault(),
            SeatingMapId = seatingMapId.GetValueOrDefault(),
            Status = status,
            StartDate = startDate ?? DateTime.UtcNow,
            EndDate = endDate,
            TimeZoneId = "America/New_York",
        };

        return request;
    }

    protected async Task<ArtistPerformanceResponse> AddArtistToCalendarAsync(
        Guid calendarId,
        bool isHeadliner = false,
        DateTimeOffset? start = null,
        DateTimeOffset? end = null)
    {
        var artist = await CreateArtistAsync();

        var request = new CreateArtistPerformanceRequest
        {
            ArtistId = artist.Id,
            IsHeadliner = isHeadliner,
            SetStart = start,
            SetEnd = end
        };

        var response = await Client.PostAsJsonAsync($"{BaseUrlEventVenueCalendars}/{calendarId}/artist-performances", request);

        response.EnsureSuccessStatusCode();

        return await response.ReadContentAsync<ArtistPerformanceResponse>();
    }

    protected async Task<HttpResponseMessage> RemoveArtistFromCalendarAsync(Guid calendarId, Guid artistId)
    {
        return await Client.DeleteAsync($"api/v1/artist-performances/{calendarId}/{artistId}");
    }

    protected async Task<HttpResponseMessage> SwapHeadlinerAsync(Guid calendarId, Guid currentId, Guid newId)
    {
        var request = new SwapHeadlinerRequest(currentId, newId);

        return await Client.PatchAsJsonAsync($"{BaseUrlEventVenueCalendars}/{calendarId}/artist-performances/swap-headliner", request);
    }
}