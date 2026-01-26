using EventHouse.Management.Api.Contracts.Artists;
using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.Genres;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace EventHouse.Management.Api.Tests.Controllers;

public sealed class ArtistsControllerTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetAll_WithoutToken_Returns401()
    {
        var res = await _client.GetAsync("/api/v1/artists");
        res.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Create_Returns201_Location_And_CanGetById()
    {
        // Arrange
        var bearer = await _client.GetBearerTokenAsync();
        _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(bearer);

        var request = new CreateArtistRequest
        {
            Name = "The Rolling Stones",
            Category = ArtistCategory.Band
        };

        // Act
        var post = await _client.PostAsJsonAsync("/api/v1/artists", request);

        // Assert: 201
        post.StatusCode.Should().Be(HttpStatusCode.Created);

        // Assert: body
        var created = await post.Content.ReadFromJsonAsync<Artist>(JsonTestOptions.Default);

        created.Should().NotBeNull();
        created!.Id.Should().NotBeEmpty();
        created.Name.Should().Be("The Rolling Stones");
        created.Category.Should().Be(ArtistCategory.Band);

        // Assert: Location header matches CreatedAtAction(GetById)
        post.Headers.Location.Should().NotBeNull();
        var location = post.Headers.Location!.ToString();

        location.Should().Contain("/api/v1/artists/");
        location.Should().EndWith(created.Id.ToString());

        // Roundtrip: GET by id returns 200 and same resource
        var get = await _client.GetAsync($"/api/v1/artists/{created.Id}");
        get.StatusCode.Should().Be(HttpStatusCode.OK);

        var fetched = await get.Content.ReadFromJsonAsync<Artist>(JsonTestOptions.Default);
        fetched.Should().NotBeNull();
        fetched!.Id.Should().Be(created.Id);
        fetched.Name.Should().Be(created.Name);
        fetched.Category.Should().Be(created.Category);
    }

    [Fact]
    public async Task GetById_WhenMissing_Returns404()
    {
        var bearer = await _client.GetBearerTokenAsync();
        _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(bearer);

        var res = await _client.GetAsync($"/api/v1/artists/{Guid.NewGuid()}");
        res.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_Returns204_And_PersistsChanges()
    {
        var bearer = await _client.GetBearerTokenAsync();
        _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(bearer);

        // create
        var create = await _client.PostAsJsonAsync("/api/v1/artists", new CreateArtistRequest
        {
            Name = "Artist A",
            Category = ArtistCategory.Band
        });
        create.StatusCode.Should().Be(HttpStatusCode.Created);

        var created = await create.Content.ReadFromJsonAsync<Artist>(JsonTestOptions.Default);
        created!.Id.Should().NotBeEmpty();

        // update
        var put = await _client.PutAsJsonAsync($"/api/v1/artists/{created.Id}", new UpdateArtistRequest
        {
            Name = "Artist A Updated",
            Category = ArtistCategory.Singer
        });

        put.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // roundtrip
        var get = await _client.GetAsync($"/api/v1/artists/{created.Id}");
        get.StatusCode.Should().Be(HttpStatusCode.OK);

        var updated = await get.Content.ReadFromJsonAsync<Artist>(JsonTestOptions.Default);
        updated!.Name.Should().Be("Artist A Updated");
        updated.Category.Should().Be(ArtistCategory.Singer);
    }

    [Fact]
    public async Task Update_WhenMissing_Returns404_ProblemJson()
    {
        var bearer = await _client.GetBearerTokenAsync();
        _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(bearer);

        var id = Guid.NewGuid();
        var res = await _client.PutAsJsonAsync($"/api/v1/artists/{id}", new UpdateArtistRequest
        {
            Name = "Does not matter",
            Category = ArtistCategory.Band
        });

        res.StatusCode.Should().Be(HttpStatusCode.NotFound);
        Assert.Equal("application/problem+json", res.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task Delete_Returns204_And_Then_GetReturns404()
    {
        var bearer = await _client.GetBearerTokenAsync();
        _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(bearer);

        // create
        var create = await _client.PostAsJsonAsync("/api/v1/artists", new CreateArtistRequest
        {
            Name = "Artist To Delete",
            Category = ArtistCategory.Band
        });
        create.StatusCode.Should().Be(HttpStatusCode.Created);

        var created = await create.Content.ReadFromJsonAsync<Artist>(JsonTestOptions.Default);

        // delete
        var del = await _client.DeleteAsync($"/api/v1/artists/{created!.Id}");
        del.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // get -> 404
        var get = await _client.GetAsync($"/api/v1/artists/{created.Id}");
        get.StatusCode.Should().Be(HttpStatusCode.NotFound);
        Assert.Equal("application/problem+json", get.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task Create_WhenInvalid_Returns400_ValidationProblemJson()
    {
        var bearer = await _client.GetBearerTokenAsync();
        _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(bearer);

        var res = await _client.PostAsJsonAsync("/api/v1/artists", new CreateArtistRequest
        {
            Name = "A",
            Category = ArtistCategory.Band
        });

        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        Assert.Equal("application/problem+json", res.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task GetAll_WithPaging_ReturnsPagedResultWithLinks()
    {
        var bearer = await _client.GetBearerTokenAsync();
        _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(bearer);

        // Arrange: create 3 artists
        foreach (var name in new[] { "A1", "A2", "A3" })
        {
            var create = await _client.PostAsJsonAsync("/api/v1/artists", new CreateArtistRequest
            {
                Name = name,
                Category = ArtistCategory.Band
            });
            create.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        // Act
        var res = await _client.GetAsync("/api/v1/artists?page=1&pageSize=2");
        res.StatusCode.Should().Be(HttpStatusCode.OK);

        var page = await res.Content.ReadFromJsonAsync<PagedResult<Artist>>(JsonTestOptions.Default);
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
        var bearer = await _client.GetBearerTokenAsync();
        _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(bearer);

        // create artist
        var createArtist = await _client.PostAsJsonAsync("/api/v1/artists", new CreateArtistRequest
        {
            Name = "Artist With Genre OK",
            Category = ArtistCategory.Band
        });

        createArtist.StatusCode.Should().Be(HttpStatusCode.Created);
        var artist = await createArtist.Content.ReadFromJsonAsync<Artist>(JsonTestOptions.Default);

        // create genre
        var createGenre = await _client.PostAsJsonAsync("/api/v1/genres", new CreateGenreRequest
        {
            Name = "Rock 2"
        });

        createGenre.StatusCode.Should().Be(HttpStatusCode.Created);

        var genre = await createGenre.Content.ReadFromJsonAsync<Genre>(JsonTestOptions.Default);

        // add genre to artist
        var add = await _client.PostAsJsonAsync($"/api/v1/artists/{artist!.Id}/genres", new AddArtistGenreRequest
        {
            GenreId = genre!.Id,
            Status = ArtistGenreStatus.Active,
            IsPrimary = true
        });

        add.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task AddGenre_IsIdempotent_ShouldNotDuplicate()
    {
        var bearer = await _client.GetBearerTokenAsync();
        _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(bearer);


        // create artist
        var createArtist = await _client.PostAsJsonAsync("/api/v1/artists", new CreateArtistRequest
        {
            Name = "Artist With Genre",
            Category = ArtistCategory.Band
        });

        createArtist.StatusCode.Should().Be(HttpStatusCode.Created);
        var artist = await createArtist.Content.ReadFromJsonAsync<Artist>(JsonTestOptions.Default);

        // create genre
        var createGenre = await _client.PostAsJsonAsync("/api/v1/genres", new CreateGenreRequest
        {
            Name = "Punk"
        });

        createGenre.StatusCode.Should().Be(HttpStatusCode.Created);

        var genre = await createGenre.Content.ReadFromJsonAsync<Genre>(JsonTestOptions.Default);

        var body = new AddArtistGenreRequest
        {
            GenreId = genre!.Id,
            Status = ArtistGenreStatus.Active,
            IsPrimary = false
        };

        (await _client.PostAsJsonAsync($"/api/v1/artists/{artist!.Id}/genres", body))
            .StatusCode.Should().Be(HttpStatusCode.NoContent);

        (await _client.PostAsJsonAsync($"/api/v1/artists/{artist.Id}/genres", body))
            .StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task SetPrimaryGenre_Returns204_And_MovesPrimaryFlag()
    {
        var bearer = await _client.GetBearerTokenAsync();
        _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(bearer);

        var createArtist = await _client.PostAsJsonAsync("/api/v1/artists", new CreateArtistRequest
        {
            Name = "Artist Primary",
            Category = ArtistCategory.Band
        });

        createArtist.StatusCode.Should().Be(HttpStatusCode.Created);

        var artist = await createArtist.Content.ReadFromJsonAsync<Artist>(JsonTestOptions.Default);

        // create genre
        var createGenre = await _client.PostAsJsonAsync("/api/v1/genres", new CreateGenreRequest
        {
            Name = "Trap"
        });

        createGenre.StatusCode.Should().Be(HttpStatusCode.Created);
        var genre = await createGenre.Content.ReadFromJsonAsync<Genre>(JsonTestOptions.Default);

        var createGenre2 = await _client.PostAsJsonAsync("/api/v1/genres", new CreateGenreRequest
        {
            Name = "Cumbia"
        });

        createGenre2.StatusCode.Should().Be(HttpStatusCode.Created);
        var genre2 = await createGenre2.Content.ReadFromJsonAsync<Genre>(JsonTestOptions.Default);

        // add both genres
        (await _client.PostAsJsonAsync($"/api/v1/artists/{artist!.Id}/genres", new AddArtistGenreRequest
        {
            GenreId = genre!.Id,
            Status = ArtistGenreStatus.Active,
            IsPrimary = true
        })).StatusCode.Should().Be(HttpStatusCode.NoContent);

        (await _client.PostAsJsonAsync($"/api/v1/artists/{artist.Id}/genres", new AddArtistGenreRequest
        {
            GenreId = genre2!.Id,
            Status = ArtistGenreStatus.Active,
            IsPrimary = false
        })).StatusCode.Should().Be(HttpStatusCode.NoContent);

        // act: set g2 primary
        var patch = await _client.PatchAsync($"/api/v1/artists/{artist.Id}/genres/{genre2.Id}/primary", content: null);
        patch.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task RemoveGenre_Returns204_And_RemovesAssociation()
    {
        var bearer = await _client.GetBearerTokenAsync();
        _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(bearer);


        var createArtist = await _client.PostAsJsonAsync("/api/v1/artists", new CreateArtistRequest
        {
            Name = "Artist Remove",
            Category = ArtistCategory.Band
        });

        createArtist.StatusCode.Should().Be(HttpStatusCode.Created);
        var artist = await createArtist.Content.ReadFromJsonAsync<Artist>(JsonTestOptions.Default);

        // create genre
        var createGenre = await _client.PostAsJsonAsync("/api/v1/genres", new CreateGenreRequest
        {
            Name = "Metal"
        });

        createGenre.StatusCode.Should().Be(HttpStatusCode.Created);
        var genre = await createGenre.Content.ReadFromJsonAsync<Genre>(JsonTestOptions.Default);


        // add
        (await _client.PostAsJsonAsync($"/api/v1/artists/{artist!.Id}/genres", new AddArtistGenreRequest
        {
            GenreId = genre!.Id,
            Status = ArtistGenreStatus.Active,
            IsPrimary = false
        })).StatusCode.Should().Be(HttpStatusCode.NoContent);

        // delete
        var del = await _client.DeleteAsync($"/api/v1/artists/{artist.Id}/genres/{genre.Id}");
        del.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task UpdateGenreStatus_Returns204_And_PersistsChange()
    {
        var bearer = await _client.GetBearerTokenAsync();
        _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(bearer);

        // create artist
        var createArtist = await _client.PostAsJsonAsync("/api/v1/artists", new CreateArtistRequest
        {
            Name = "Artist Status",
            Category = ArtistCategory.Band
        });
        createArtist.StatusCode.Should().Be(HttpStatusCode.Created);
        var artist = await createArtist.Content.ReadFromJsonAsync<Artist>(JsonTestOptions.Default);
        artist!.Id.Should().NotBeEmpty();

        // create genre
        var createGenre = await _client.PostAsJsonAsync("/api/v1/genres", new CreateGenreRequest
        {
            Name = "Alternativa"
        });
        createGenre.StatusCode.Should().Be(HttpStatusCode.Created);
        var genre = await createGenre.Content.ReadFromJsonAsync<Genre>(JsonTestOptions.Default);
        genre!.Id.Should().NotBeEmpty();

        // associate genre to artist (Active)
        (await _client.PostAsJsonAsync($"/api/v1/artists/{artist.Id}/genres", new AddArtistGenreRequest
        {
            GenreId = genre.Id,
            Status = ArtistGenreStatus.Active,
            IsPrimary = true
        })).StatusCode.Should().Be(HttpStatusCode.NoContent);

        // act: update status to Inactive
        var put = await _client.PutAsJsonAsync(
            $"/api/v1/artists/{artist.Id}/genres/{genre.Id}",
            new UpdateArtistGenreStatusRequest { Status = ArtistGenreStatus.Inactive });

        put.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
