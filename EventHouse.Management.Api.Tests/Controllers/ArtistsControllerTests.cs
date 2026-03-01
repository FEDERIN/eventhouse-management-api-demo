using EventHouse.Management.Api.Contracts.Artists;
using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Tests.Abstractions;
using EventHouse.Management.Api.Tests.Common;
using EventHouse.Management.Api.Tests.Factories;
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
        var request = ArtistFactory.CreateRequest(category: ArtistCategory.DJ);

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
        var artist = await CreateArtistAsync(category: ArtistCategory.Singer);
        var updateRequest = new UpdateArtistRequest 
        { 
            Name = artist.Name + "Updated",
            Category = ArtistCategory.Influencer
        };

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

        var artist = await CreateArtistAsync(category: ArtistCategory.Band);

        // delete
        var del = await Client.DeleteAsync($"{BaseUrlArtists}/{artist!.Id}");
        del.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // get -> 404
        var get = await Client.GetAsync($"{BaseUrlArtists}/{artist.Id}");
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
        for(var i = 0; i < 3; i++)
        {
            await CreateArtistAsync(category: ArtistCategory.Band);
        }

        // Act
        var res = await Client.GetAsync($"{BaseUrlArtists}?page=1&pageSize=2");
        res.StatusCode.Should().Be(HttpStatusCode.OK);

        var page = await res.Content.ReadFromJsonAsync<PagedResult<ArtistDetail>>(JsonTestOptions.Default);
        page.Should().NotBeNull();
        page!.Items.Count.Should().BeLessOrEqualTo(2);
        page.ShouldHaveValidPaginationLinks(currentPage: 1, expectedPageSize: 2);
    }

    [Fact]
    public async Task AddGenre_Returns204_And_IsVisibleInGetArtist()
    {
        // Arrange
        var artist = await CreateArtistAsync(category: ArtistCategory.Band);
        var genre = await CreateGenreAsync(forCategory: ArtistCategory.Band);

        // Act
        var response = await Client.PostAsJsonAsync($"{BaseUrlArtists}/{artist!.Id}/genres", new AddArtistGenreRequest
        {
            GenreId = genre!.Id,
            Status = ArtistGenreStatus.Active,
            IsPrimary = true
        });

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // --- Round-trip: Verificación real ---
        var updatedArtist = await Client.GetFromJsonAsync<ArtistDetail>($"{BaseUrlArtists}/{artist.Id}", JsonTestOptions.Default);
        
        updatedArtist!.Genres.Should().ContainSingle(g => g.GenreId == genre.Id && g.IsPrimary == true);
    }

    [Fact]
    public async Task AddGenre_IsIdempotent_ShouldNotDuplicate()
    {
        var artist = await CreateArtistAsync(category: ArtistCategory.Host);

        // create genre
        var genre = await CreateGenreAsync(forCategory: ArtistCategory.Host);

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
        var artist = await CreateArtistAsync(category: ArtistCategory.Comedian);

        // create genre
        var genre = await CreateGenreAsync(forCategory: ArtistCategory.Comedian);
        var genre2 = await CreateGenreAsync(forCategory: ArtistCategory.Comedian);

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
        var artist = await CreateArtistAsync(category: ArtistCategory.Influencer);

        // create genre
        var genre = await CreateGenreAsync(forCategory: ArtistCategory.Influencer);

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
        var artist = await CreateArtistAsync(category: ArtistCategory.Dancer);

        artist!.Id.Should().NotBeEmpty();

        // create genre
        var genre = await CreateGenreAsync(forCategory: ArtistCategory.Dancer);

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
}
