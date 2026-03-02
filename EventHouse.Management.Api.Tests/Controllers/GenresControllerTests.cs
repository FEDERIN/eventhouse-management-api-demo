using EventHouse.Management.Api.Contracts.Artists;
using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.Genres;
using EventHouse.Management.Api.Tests.Abstractions;
using EventHouse.Management.Api.Tests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace EventHouse.Management.Api.Tests.Controllers;

public sealed class GenresControllerTests(CustomWebApplicationFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task GetAll_WithoutToken_Returns401()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, BaseUrlGenres).WithoutAuthentication();

        var res = await Client.SendAsync(request);

        res.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Create_Returns201_And_CanGetById()
    {
        var request = new CreateGenreRequest { Name = "Jazz" };

        var postResponse = await Client.PostAsJsonAsync(BaseUrlGenres, request);

        postResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await postResponse.Content.ReadFromJsonAsync<GenreResponse>(JsonTestOptions.Default);

        created.Should().NotBeNull();
        postResponse.Headers.Location!.ToString().Should().EndWith(created!.Id.ToString());
    }

    [Fact]
    public async Task Update_Returns204_And_PersistsChanges()
    {
        var genre = await CreateGenreAsync(forCategory: ArtistCategory.Band);

        var updateRequest = new UpdateGenreRequest { Name = "Pop" };
        var put = await Client.PutAsJsonAsync($"{BaseUrlGenres}/{genre!.Id}", updateRequest);
        put.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var updated = await Client.GetFromJsonAsync<GenreResponse>($"{BaseUrlGenres}/{genre.Id}", JsonTestOptions.Default);
        updated.Should().BeEquivalentTo(updateRequest);
    }

    [Fact]
    public async Task Update_WhenMissing_Returns404_ProblemJson()
    {
        var res = await Client.PutAsJsonAsync($"{BaseUrlGenres}/{Guid.NewGuid()}", new UpdateGenreRequest { Name = "Salsa" });

        await res.ShouldBeProblemJson(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_WhenNameDuplicate_Returns409Conflict()
    {
        // Arrange

        var genre = await CreateGenreAsync(forCategory: ArtistCategory.Comedian);
        var genre2 = await CreateGenreAsync(forCategory: ArtistCategory.Host);

        // Act
        var update = await Client.PutAsJsonAsync($"{BaseUrlGenres}/{genre2!.Id}", new UpdateGenreRequest { Name = genre.Name });

        // Assert
        await update.ShouldHaveErrorCode(HttpStatusCode.Conflict, "GENRE_NAME_ALREADY_EXISTS");
    }

    [Fact]
    public async Task Delete_Returns404_ProblemJson()
    {
        // delete
        var res = await Client.DeleteAsync($"{BaseUrlGenres}/{Guid.NewGuid()}");
        await res.ShouldBeProblemJson(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_When_Is_Association_Returns409Conflict()
    {
        // Arrange
        var artist = await CreateArtistAsync(category: ArtistCategory.Band);
        var genre = await CreateGenreAsync(forCategory: ArtistCategory.Band);

        // Act
        await Client.PostAsJsonAsync($"{BaseUrlArtists}/{artist!.Id}/genres", new AddArtistGenreRequest
        {
            GenreId = genre!.Id,
            Status = ArtistGenreStatus.Active,
            IsPrimary = true
        });

        // delete
        var res = await Client.DeleteAsync($"{BaseUrlGenres}/{genre.Id}");

        // Assert
        await res.ShouldHaveErrorCode(HttpStatusCode.Conflict, "GENRE_HAS_ASSOCIATIONS");
    }

    [Fact]
    public async Task Delete_Returns204_And_Then_GetReturns404()
    {
        var genre = await CreateGenreAsync(forCategory: ArtistCategory.Singer);

        // delete
        var del = await Client.DeleteAsync($"{BaseUrlGenres}/{genre!.Id}");
        del.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // get -> 404
        var get = await Client.GetAsync($"{BaseUrlGenres}/{genre.Id}");

        await get.ShouldBeProblemJson(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_WhenInvalid_Returns400_ValidationProblemJson()
    {
        var res = await Client.PostAsJsonAsync(BaseUrlGenres, new CreateGenreRequest
        {
            Name = "R"
        });

        await res.ShouldBeProblemJson(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetAll_WithPaging_ReturnsPagedResultWithLinks()
    {
        var categories = new[] { ArtistCategory.Influencer, ArtistCategory.Dancer, ArtistCategory.DJ };

        foreach (var category in categories)
        {
            await CreateGenreAsync(forCategory: category);
        }

        // Act
        var res = await Client.GetAsync($"{BaseUrlGenres}?page=1&pageSize=2");

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        var page = await res.Content.ReadFromJsonAsync<PagedResult<GenreResponse>>(JsonTestOptions.Default);

        page.Should().NotBeNull();
        page!.Items.Count.Should().Be(2); // Al usar prefijos únicos, podemos ser exactos

        // Usamos tu método de extensión para limpiar el test
        page.ShouldHaveValidPaginationLinks(currentPage: 1, expectedPageSize: 2);
    }


    [Fact]
    public async Task GetById_WhenMissing_Returns404()
    {
        var res = await Client.GetAsync($"{BaseUrlGenres}/{Guid.NewGuid()}");
        res.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}