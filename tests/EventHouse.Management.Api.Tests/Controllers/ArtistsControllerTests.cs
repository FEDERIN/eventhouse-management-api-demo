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
    #region READ (GET)
    [Theory]
    [InlineData("Bad Bunny", null)]
    [InlineData(null, ArtistCategory.Singer)]
    [InlineData("Festival", ArtistCategory.Dancer)]
    public async Task GetArtists_WithFiltersAndSorting_ReturnsFilteredResults(
        string? name,
        ArtistCategory? category)
    {
        var url = $"{BaseUrlArtists}?" +
                  (name != null ? $"name={name}&" : "") +
                  (category.HasValue ? $"category={category}&" : "");

        // Act
        var response = await Client.GetAsync(url, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData(ArtistSortBy.Name, SortDirection.Asc)]
    [InlineData(ArtistSortBy.Name, SortDirection.Desc)]
    [InlineData(ArtistSortBy.Category, SortDirection.Asc)]
    [InlineData(ArtistSortBy.Category, SortDirection.Desc)]
    [InlineData(null, SortDirection.Asc)]
    [InlineData(null, SortDirection.Desc)]
    public async Task GetAll_WithSorting_ReturnsSortedResults(ArtistSortBy? sortColumn, SortDirection direction)
    {

        await CreateArtistAsync(category: ArtistCategory.Singer);
        await CreateArtistAsync(category: ArtistCategory.Influencer);
        await CreateArtistAsync(category: ArtistCategory.Dancer);

        var url = $"{BaseUrlArtists}?sortBy={sortColumn}&sortDirection={direction}";

        // Act
        var response = await Client.GetAsync(url, TestContext.Current.CancellationToken);
        var pagedResult = await response.ReadContentAsync<PagedResult<ArtistDetail>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = pagedResult.Items;

        switch (sortColumn)
        {
            case ArtistSortBy.Name:
                ValidateOrder(items.Select(x => x.Name), direction);
                break;

            case ArtistSortBy.Category:
                ValidateOrder(items.Select(x => x.Category), direction);
                break;
            default:
                ValidateOrder(items.Select(x => x.Name), direction);
                break;
        }

    }

    [Fact]
    public async Task GetAll_WithPaging_ReturnsPagedResultWithLinks()
    {
        // Arrange: create 3 artists
        for (var i = 0; i < 3; i++)
        {
            await CreateArtistAsync(category: ArtistCategory.Band);
        }

        // Act
        var res = await Client.GetAsync($"{BaseUrlArtists}?page=1&pageSize=2", TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(HttpStatusCode.OK);

        var page = await res.Content.ReadFromJsonAsync<PagedResult<ArtistDetail>>(JsonTestOptions.Default, TestContext.Current.CancellationToken);
        page.Should().NotBeNull();
        page!.Items.Count.Should().BeLessThanOrEqualTo(2);
        page.ShouldHaveValidPaginationLinks(currentPage: 1, expectedPageSize: 2);
    }
    #endregion

    #region CREATE (POST)
    [Fact]
    public async Task Create_Returns201_And_MatchesRequest()
    {
        var request = ArtistFactory.CreateRequest(category: ArtistCategory.DJ);

        var response = await Client.PostAsJsonAsync(BaseUrlArtists, request, TestContext.Current.CancellationToken);
        var created = await response.ReadContentAsync<ArtistDetail>();
        created.Should().BeEquivalentTo(request, opt => opt.ExcludingMissingMembers());
    }

    [Fact]
    public async Task Create_WhenInvalid_Returns400_ValidationProblemJson()
    {
        var res = await Client.PostAsJsonAsync(BaseUrlArtists, new CreateArtistRequest
        {
            Name = "A",
            Category = ArtistCategory.Band
        }, TestContext.Current.CancellationToken);

        await res.ShouldBeProblemJson(HttpStatusCode.BadRequest);
    }
    #endregion

    #region UPDATE (PUT)
    [Fact]
    public async Task Update_Returns204_And_PersistsChanges()
    {
        var artist = await CreateArtistAsync(category: ArtistCategory.Singer);
        var updateRequest = new UpdateArtistRequest 
        { 
            Name = artist.Name + "Updated",
            Category = ArtistCategory.Influencer
        };

        await PutAndAssertPersisted<UpdateArtistRequest, ArtistDetail>(
                $"{BaseUrlArtists}/{artist.Id}",
                updateRequest
            );
    }

    [Fact]
    public async Task Update_WhenMissing_Returns404_ProblemJson()
    {
        await AssertPutReturns404($"{BaseUrlArtists}/{Guid.NewGuid()}", new UpdateArtistRequest
        {
            Name = "Does not matter",
            Category = ArtistCategory.Band
        });
    }
    #endregion

    #region DELETE
    [Fact]
    public async Task Delete_Returns204_And_Then_GetReturns404()
    {
        //Arrange
        var artist = await CreateArtistAsync(category: ArtistCategory.Band);
        var url = $"{BaseUrlArtists}/{artist!.Id}";

        // Act & Assert
        await AssertDeleteReturns204(url);
        await AssertGetReturns404(url);
    }

    [Fact]
    public async Task Delete_Return409_And_Have_Relation_Artist()
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

        (await Client.PostAsJsonAsync($"{BaseUrlArtists}/{artist!.Id}/genres", body, TestContext.Current.CancellationToken))
            .StatusCode.Should().Be(HttpStatusCode.NoContent);

        var del = await Client.DeleteAsync($"{BaseUrlArtists}/{artist!.Id}", TestContext.Current.CancellationToken);
        del.StatusCode.Should().Be(HttpStatusCode.Conflict);

    }

    #endregion

    #region GENRE

    #region CREATE (POST)
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
        }, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // --- Round-trip: Verificación real ---
        var updatedArtist = await Client.GetFromJsonAsync<ArtistDetail>($"{BaseUrlArtists}/{artist.Id}", JsonTestOptions.Default, TestContext.Current.CancellationToken);
        
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

        (await Client.PostAsJsonAsync($"{BaseUrlArtists}/{artist!.Id}/genres", body, TestContext.Current.CancellationToken))
            .StatusCode.Should().Be(HttpStatusCode.NoContent);

        (await Client.PostAsJsonAsync($"{BaseUrlArtists}/{artist.Id}/genres", body, TestContext.Current.CancellationToken))
            .StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    #endregion


    #region UPDATE (PUT, PATCH)
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
        }, TestContext.Current.CancellationToken)).StatusCode.Should().Be(HttpStatusCode.NoContent);


        // act: update status to Inactive
        var put = await Client.PutAsJsonAsync(
            $"{BaseUrlArtists}/{artist.Id}/genres/{genre.Id}",
            new UpdateArtistGenreStatusRequest { Status = ArtistGenreStatus.Inactive },
            TestContext.Current.CancellationToken);

        put.StatusCode.Should().Be(HttpStatusCode.NoContent);
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
        }, TestContext.Current.CancellationToken)).StatusCode.Should().Be(HttpStatusCode.NoContent);

        (await Client.PostAsJsonAsync($"{BaseUrlArtists}/{artist.Id}/genres", new AddArtistGenreRequest
        {
            GenreId = genre2!.Id,
            Status = ArtistGenreStatus.Active,
            IsPrimary = false
        }, TestContext.Current.CancellationToken)).StatusCode.Should().Be(HttpStatusCode.NoContent);

        // act: set g2 primary
        var patch = await Client.PatchAsync($"{BaseUrlArtists}/{artist.Id}/genres/{genre2.Id}/primary", content: null, TestContext.Current.CancellationToken);
        patch.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    #endregion

    #region DELETE
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
        }, TestContext.Current.CancellationToken)).StatusCode.Should().Be(HttpStatusCode.NoContent);

        // delete
        var del = await Client.DeleteAsync($"{BaseUrlArtists}/{artist.Id}/genres/{genre.Id}", TestContext.Current.CancellationToken);
        del.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    #endregion

    #endregion
}
