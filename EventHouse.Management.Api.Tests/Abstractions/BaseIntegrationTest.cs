using EventHouse.Management.Api.Contracts.Artists;
using EventHouse.Management.Api.Contracts.Events;
using EventHouse.Management.Api.Contracts.EventVenues;
using EventHouse.Management.Api.Contracts.Genres;
using EventHouse.Management.Api.Contracts.SeatingMaps;
using EventHouse.Management.Api.Contracts.Venues;
using EventHouse.Management.Api.Tests.Common;
using EventHouse.Management.Api.Tests.Factories;
using EventHouse.Management.Domain.Entities;
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

    protected async Task<Event> CreateEventAsync(string? name = null, string? description = null, EventScope? scope = EventScope.National)
    {
        var request = EventFactory.CreateRequest(name, description, scope);
        var response = await Client.PostAsJsonAsync(ApiRoutes.Events, request);
        return await response.ReadContentAsync<Event>();
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
}