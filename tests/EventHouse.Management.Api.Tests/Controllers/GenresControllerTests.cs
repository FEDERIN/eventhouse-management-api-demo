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
    #region READ (GET)
    [Fact]
    public async Task GetAll_WithPaging_ReturnsPagedResultWithLinks()
    {
        var categories = new[] { ArtistCategory.Influencer, ArtistCategory.Dancer, ArtistCategory.DJ };

        foreach (var category in categories)
        {
            await CreateGenreAsync(forCategory: category);
        }

        // Act
        var res = await Client.GetAsync($"{BaseUrlGenres}?page=1&pageSize=2", cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        var page = await res.Content.ReadFromJsonAsync<PagedResult<GenreResponse>>(JsonTestOptions.Default, cancellationToken: TestContext.Current.CancellationToken);

        page.Should().NotBeNull();
        page!.Items.Count.Should().Be(2);

        page.ShouldHaveValidPaginationLinks(currentPage: 1, expectedPageSize: 2);
    }

    [Theory]
    [InlineData("oc", null)]
    [InlineData(null, GenreSortBy.Name)]
    public async Task GetAll_WithFiltersAndSorting_ReturnsFilteredResults(
    string? name,
    GenreSortBy? sortBy)
    {
        // Arrange
        await CreateGenreAsync(forCategory: ArtistCategory.Band, name: "Rock");
        await CreateGenreAsync(forCategory: ArtistCategory.Singer, name: "Pop");
        await CreateGenreAsync(forCategory: ArtistCategory.Dancer, name: "Electronic");

        var url = $"{BaseUrlGenres}?" +
                  (name != null ? $"name={name}&" : "") +
                  (sortBy.HasValue ? $"sortBy={sortBy}" : "");

        // Act
        var response = await Client.GetAsync(url, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        if (!string.IsNullOrEmpty(name))
        {
            var pagedResult = await response.ReadContentAsync<PagedResult<GenreResponse>>();
            pagedResult.Items.Should().AllSatisfy(x => x.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        }
    }

    [Theory]
    [InlineData(GenreSortBy.Name, SortDirection.Asc)]
    [InlineData(GenreSortBy.Name, SortDirection.Desc)]
    [InlineData(null, SortDirection.Asc)]
    [InlineData(null, SortDirection.Desc)]
    public async Task GetAll_WithSorting_ReturnsSortedResults(
        GenreSortBy? sortColumn,
        SortDirection direction)
    {
        // Arrange
        await CreateGenreAsync(forCategory: ArtistCategory.Band, name: "Rock");
        await CreateGenreAsync(forCategory: ArtistCategory.Singer, name: "Vallenato");
        await CreateGenreAsync(forCategory: ArtistCategory.Dancer, name: "Jazz");

        var url = $"{BaseUrlGenres}?sortBy={sortColumn}&sortDirection={direction}";

        // Act
        var response = await Client.GetAsync(url, TestContext.Current.CancellationToken);
        var pagedResult = await response.ReadContentAsync<PagedResult<GenreResponse>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = pagedResult.Items;

        if (sortColumn == GenreSortBy.Name)
        {
            ValidateOrder(items.Select(x => x.Name), direction);
        }
        else
        {
            // Default sort by name
            ValidateOrder(items.Select(x => x.Name), direction);
        }
    }


    #endregion

    #region CREATE (POST)
    [Fact]
    public async Task Create_Returns201_And_CanGetById()
    {
        var request = new CreateGenreRequest { Name = "Jazz" };

        var postResponse = await Client.PostAsJsonAsync(BaseUrlGenres, request, cancellationToken: TestContext.Current.CancellationToken);

        postResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await postResponse.Content.ReadFromJsonAsync<GenreResponse>(JsonTestOptions.Default, cancellationToken: TestContext.Current.CancellationToken);

        created.Should().NotBeNull();
        postResponse.Headers.Location!.ToString().Should().EndWith(created!.Id.ToString());
    }

    [Fact]
    public async Task Create_WhenInvalid_Returns400_ValidationProblemJson()
    {
        var res = await Client.PostAsJsonAsync(BaseUrlGenres, new CreateGenreRequest
        {
            Name = "R"
        }, cancellationToken: TestContext.Current.CancellationToken);

        await res.ShouldBeProblemJson(HttpStatusCode.BadRequest);
    }
    #endregion

    #region UPDATE (PUT)
    [Fact]
    public async Task Update_Returns204_And_PersistsChanges()
    {
        var genre = await CreateGenreAsync(forCategory: ArtistCategory.Band);
        var updateRequest = new UpdateGenreRequest { Name = "Pop" };

        await PutAndAssertPersisted<UpdateGenreRequest, GenreResponse>(
            $"{BaseUrlGenres}/{genre!.Id}",
            updateRequest
        );
    }
    [Fact]
    public async Task Update_WhenMissing_Returns404_ProblemJson()
    {
        await AssertPutReturns404($"{BaseUrlGenres}/{Guid.NewGuid()}", new UpdateGenreRequest { Name = "Salsa" });
    }

    [Fact]
    public async Task Update_WhenNameDuplicate_Returns409Conflict()
    {
        // Arrange

        var genre = await CreateGenreAsync(forCategory: ArtistCategory.Comedian);
        var genre2 = await CreateGenreAsync(forCategory: ArtistCategory.Host);

        // Act
        var update = await Client.PutAsJsonAsync($"{BaseUrlGenres}/{genre2!.Id}", new UpdateGenreRequest { Name = genre.Name }, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        await update.ShouldHaveErrorCode(HttpStatusCode.Conflict, "GENRE_NAME_ALREADY_EXISTS");
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
        }, cancellationToken: TestContext.Current.CancellationToken);

        // delete
        var res = await Client.DeleteAsync($"{BaseUrlGenres}/{genre.Id}", cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        await res.ShouldHaveErrorCode(HttpStatusCode.Conflict, "GENRE_HAS_ASSOCIATIONS");
    }
    #endregion

    #region DELETE
    [Fact]
    public async Task Delete_Returns204_And_Then_GetReturns404()
    {
        //Arrange
        var genre = await CreateGenreAsync(forCategory: ArtistCategory.Singer);
        var url = $"{BaseUrlGenres}/{genre.Id}";

        //Act && Assert
        await AssertDeleteReturns204(url);
        await AssertGetReturns404(url);
    }
    #endregion
}