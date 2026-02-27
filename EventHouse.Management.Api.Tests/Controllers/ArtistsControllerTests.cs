using EventHouse.Management.Api.Contracts.Artists;
using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.Genres;
using EventHouse.Management.Api.Tests.Abstractions;
using EventHouse.Management.Api.Tests.Common;
using EventHouse.Management.Api.Tests.Contracts;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace EventHouse.Management.Api.Tests.Controllers;

public sealed class ArtistsControllerTests(CustomWebApplicationFactory factory)
    : BaseIntegrationTest(factory)
{
    private const string BaseUrlArtists = ApiRoutes.Artists;
    private const string BaseUrlGenres = ApiRoutes.Genres;

    [Fact]
    public async Task GetAll_WithoutToken_Returns401()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, BaseUrlArtists).WithoutAuthentication();

        var res = await Client.SendAsync(request);

        res.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Create_Returns201_And_MatchesRequest()
    {
        var request = ArtistFactory.CreateRequest("The Rolling Stones");

        var response = await Client.PostAsJsonAsync(BaseUrlArtists, request);
        var created = await response.ReadContentAsync<ArtistDetail>();

        created.Should().BeEquivalentTo(request, opt => opt.ExcludingMissingMembers());
    }

    [Fact]
    public async Task GetById_WhenMissing_Returns404()
    {
        var res = await Client.GetAsync($"{BaseUrlArtists}/{Guid.NewGuid()}");

        await res.ShouldBeProblemJson(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_Returns204_And_PersistsChanges()
    {
        var artist = await CreateArtistAsync("Artist A", ArtistCategory.Singer);
        var updateRequest = new UpdateArtistRequest { Name = "Artist A Updated", Category = ArtistCategory.Singer };

        // Act
        var response = await Client.PutAsJsonAsync($"{BaseUrlArtists}/{artist.Id}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Roundtrip
        var updated = await Client.GetFromJsonAsync<ArtistDetail>($"{BaseUrlArtists}/{artist.Id}", JsonTestOptions.Default);
        updated.Should().BeEquivalentTo(updateRequest); // Ahora sí comparas DTO con DTO
    }

    [Fact]
    public async Task Update_WhenMissing_Returns404_ProblemJson()
    {
        var id = Guid.NewGuid();
        var res = await Client.PutAsJsonAsync($"{BaseUrlArtists}/{id}", new UpdateArtistRequest
        {
            Name = "Does not matter",
            Category = ArtistCategory.Band
        });

        await res.ShouldBeProblemJson(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_Returns204_And_Then_GetReturns404()
    {
        // create
        var create = await Client.PostAsJsonAsync(BaseUrlArtists, new CreateArtistRequest
        {
            Name = "Artist To Delete",
            Category = ArtistCategory.Band
        });
        create.StatusCode.Should().Be(HttpStatusCode.Created);

        var created = await create.Content.ReadFromJsonAsync<ArtistDetail>(JsonTestOptions.Default);

        // delete
        var del = await Client.DeleteAsync($"{BaseUrlArtists}/{created!.Id}");
        del.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // get -> 404
        var get = await Client.GetAsync($"{BaseUrlArtists}/{created.Id}");
        await get.ShouldBeProblemJson(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_WhenInvalid_Returns400_ValidationProblemJson()
    {
        var res = await Client.PostAsJsonAsync(BaseUrlArtists, new CreateArtistRequest
        {
            Name = "A",
            Category = ArtistCategory.Band
        });

        await res.ShouldBeProblemJson(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetAll_WithPaging_ReturnsPagedResultWithLinks()
    {
        // Arrange: create 3 artists
        foreach (var name in new[] { "A1", "A2", "A3" })
        {
            await CreateArtistAsync(name, ArtistCategory.Band);
        }

        // Act
        var res = await Client.GetAsync($"{BaseUrlArtists}?page=1&pageSize=2");
        res.StatusCode.Should().Be(HttpStatusCode.OK);

        var page = await res.Content.ReadFromJsonAsync<PagedResult<ArtistDetail>>(JsonTestOptions.Default);
        page.Should().NotBeNull();

        page!.Items.Should().NotBeNull();
        page.Items.Count.Should().BeLessOrEqualTo(2);
        page.Page.Should().Be(1);
        page.PageSize.Should().Be(2);
        page.TotalCount.Should().BeGreaterOrEqualTo(page.Items.Count);

        page.Links.Should().NotBeNull();
        page.Links!.Self.Should().NotBeNull();
        page.Links.Self!.Should().Contain("page=1");
        page.Links.Self!.Should().Contain("pageSize=2");

        if (page.TotalCount > 2)
            page.Links.Next.Should().NotBeNull();
    }

    [Fact]
    public async Task AddGenre_Returns204_And_IsVisibleInGetArtist()
    {
        // Arrange
        var artist = await CreateArtistAsync("Artist With Genre OK", ArtistCategory.DJ);
        var genre = await CreateGenreAsync("Rock 2");

        // Act
        var response = await Client.PostAsJsonAsync($"{BaseUrlArtists}/{artist!.Id}/genres", new AddArtistGenreRequest
        {
            GenreId = genre!.Id,
            Status = ArtistGenreStatus.Active,
            IsPrimary = true
        });

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task AddGenre_IsIdempotent_ShouldNotDuplicate()
    {
        var artist = await CreateArtistAsync("Artist With Genre", ArtistCategory.Host);

        // create genre
        var createGenre = await Client.PostAsJsonAsync(BaseUrlGenres, new CreateGenreRequest
        {
            Name = "Punk"
        });

        createGenre.StatusCode.Should().Be(HttpStatusCode.Created);

        var genre = await createGenre.Content.ReadFromJsonAsync<GenreResponse>(JsonTestOptions.Default);

        var body = new AddArtistGenreRequest
        {
            GenreId = genre!.Id,
            Status = ArtistGenreStatus.Active,
            IsPrimary = false
        };

        (await Client.PostAsJsonAsync($"{BaseUrlArtists}/{artist!.Id}/genres", body))
            .StatusCode.Should().Be(HttpStatusCode.NoContent);

        (await Client.PostAsJsonAsync($"{BaseUrlArtists}/{artist.Id}/genres", body))
            .StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task SetPrimaryGenre_Returns204_And_MovesPrimaryFlag()
    {
        var artist = await CreateArtistAsync("Artist Primary", ArtistCategory.Comedian);

        // create genre
        var createGenre = await Client.PostAsJsonAsync(BaseUrlGenres, new CreateGenreRequest
        {
            Name = "Trap"
        });

        createGenre.StatusCode.Should().Be(HttpStatusCode.Created);
        var genre = await createGenre.Content.ReadFromJsonAsync<GenreResponse>(JsonTestOptions.Default);

        var createGenre2 = await Client.PostAsJsonAsync(BaseUrlGenres, new CreateGenreRequest
        {
            Name = "Cumbia"
        });

        createGenre2.StatusCode.Should().Be(HttpStatusCode.Created);
        var genre2 = await createGenre2.Content.ReadFromJsonAsync<GenreResponse>(JsonTestOptions.Default);

        // add both genres
        (await Client.PostAsJsonAsync($"{BaseUrlArtists}/{artist!.Id}/genres", new AddArtistGenreRequest
        {
            GenreId = genre!.Id,
            Status = ArtistGenreStatus.Active,
            IsPrimary = true
        })).StatusCode.Should().Be(HttpStatusCode.NoContent);

        (await Client.PostAsJsonAsync($"{BaseUrlArtists}/{artist.Id}/genres", new AddArtistGenreRequest
        {
            GenreId = genre2!.Id,
            Status = ArtistGenreStatus.Active,
            IsPrimary = false
        })).StatusCode.Should().Be(HttpStatusCode.NoContent);

        // act: set g2 primary
        var patch = await Client.PatchAsync($"{BaseUrlArtists}/{artist.Id}/genres/{genre2.Id}/primary", content: null);
        patch.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task RemoveGenre_Returns204_And_RemovesAssociation()
    {
        var artist = await CreateArtistAsync("Artist Remove", ArtistCategory.Influencer);

        // create genre
        var createGenre = await Client.PostAsJsonAsync(BaseUrlGenres, new CreateGenreRequest
        {
            Name = "Metal"
        });

        createGenre.StatusCode.Should().Be(HttpStatusCode.Created);
        var genre = await createGenre.Content.ReadFromJsonAsync<GenreResponse>(JsonTestOptions.Default);


        // add
        (await Client.PostAsJsonAsync($"{BaseUrlArtists}/{artist!.Id}/genres", new AddArtistGenreRequest
        {
            GenreId = genre!.Id,
            Status = ArtistGenreStatus.Active,
            IsPrimary = false
        })).StatusCode.Should().Be(HttpStatusCode.NoContent);

        // delete
        var del = await Client.DeleteAsync($"{BaseUrlArtists}/{artist.Id}/genres/{genre.Id}");
        del.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task UpdateGenreStatus_Returns204_And_PersistsChange()
    {
        var artist = await CreateArtistAsync("Artist Status", ArtistCategory.Dancer);

        artist!.Id.Should().NotBeEmpty();

        // create genre
        var createGenre = await Client.PostAsJsonAsync(BaseUrlGenres, new CreateGenreRequest
        {
            Name = "Alternativa"
        });
        createGenre.StatusCode.Should().Be(HttpStatusCode.Created);
        var genre = await createGenre.Content.ReadFromJsonAsync<GenreResponse>(JsonTestOptions.Default);
        genre!.Id.Should().NotBeEmpty();

        // associate genre to artist (Active)
        (await Client.PostAsJsonAsync($"{BaseUrlArtists}/{artist.Id}/genres", new AddArtistGenreRequest
        {
            GenreId = genre.Id,
            Status = ArtistGenreStatus.Active,
            IsPrimary = true
        })).StatusCode.Should().Be(HttpStatusCode.NoContent);

        // act: update status to Inactive
        var put = await Client.PutAsJsonAsync(
            $"{BaseUrlArtists}/{artist.Id}/genres/{genre.Id}",
            new UpdateArtistGenreStatusRequest { Status = ArtistGenreStatus.Inactive });

        put.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    // Helpers
    private async Task<ArtistDetail> CreateArtistAsync(string name, ArtistCategory category = ArtistCategory.Band)
    {
        var response = await Client.PostAsJsonAsync(BaseUrlArtists, ArtistFactory.CreateRequest(name, category));
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        return await response.ReadContentAsync<ArtistDetail>();
    }

    private async Task<GenreResponse> CreateGenreAsync(string name)
        => await (await Client.PostAsJsonAsync(BaseUrlGenres, new CreateGenreRequest { Name = name })).ReadContentAsync<GenreResponse>();
}
