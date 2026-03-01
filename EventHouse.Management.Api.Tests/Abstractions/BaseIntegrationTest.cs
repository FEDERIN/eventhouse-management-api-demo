using EventHouse.Management.Api.Contracts.Artists;
using EventHouse.Management.Api.Contracts.Genres;
using EventHouse.Management.Api.Contracts.Venues;
using EventHouse.Management.Api.Tests.Common;
using EventHouse.Management.Api.Tests.Factories;
using EventHouse.Management.Domain.Entities;
using System.Net;
using System.Net.Http.Json;

namespace EventHouse.Management.Api.Tests.Abstractions;

public abstract class BaseIntegrationTest(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    protected readonly CustomWebApplicationFactory Factory = factory;

    protected readonly HttpClient Client = factory.CreateDefaultClient(new AuthedHandler(factory));

    private const string BaseUrlGenres = ApiRoutes.Genres;
    private const string BaseUrlArtists = ApiRoutes.Artists;
    private const string BaseUrlVenues = ApiRoutes.Venues;

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
}