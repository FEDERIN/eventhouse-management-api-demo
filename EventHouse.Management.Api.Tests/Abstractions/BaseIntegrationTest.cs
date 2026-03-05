using EventHouse.Management.Api.Contracts.Artists;
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
}