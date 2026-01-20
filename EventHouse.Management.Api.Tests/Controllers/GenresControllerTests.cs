
using EventHouse.Management.Api.Common.Errors;
using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.Genres;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace EventHouse.Management.Api.Tests.Controllers;

public sealed class GenresControllerTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetAll_WithoutToken_Returns401()
    {
        var res = await _client.GetAsync("/api/v1/genres");
        res.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Create_Returns201_Location_And_CanGetById()
    {
        // Arrange
        var bearer = await _client.GetBearerTokenAsync();
        _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(bearer);

        var request = new CreateGenreRequest
        {
            Name = "Country"
        };

        // Act
        var post = await _client.PostAsJsonAsync("/api/v1/genres", request);

        // Assert: 201
        post.StatusCode.Should().Be(HttpStatusCode.Created);

        // Assert: body
        var created = await post.Content.ReadFromJsonAsync<Genre>(JsonTestOptions.Default);

        created.Should().NotBeNull();
        created!.Id.Should().NotBeEmpty();
        created.Name.Should().Be("Country");

        // Assert: Location header matches CreatedAtAction(GetById)
        post.Headers.Location.Should().NotBeNull();
        var location = post.Headers.Location!.ToString();

        location.Should().Contain("/api/v1/genres/");
        location.Should().EndWith(created.Id.ToString());

        // Roundtrip: GET by id returns 200 and same resource
        var get = await _client.GetAsync($"/api/v1/genres/{created.Id}");
        get.StatusCode.Should().Be(HttpStatusCode.OK);

        var fetched = await get.Content.ReadFromJsonAsync<Genre>(JsonTestOptions.Default);
        fetched.Should().NotBeNull();
        fetched!.Id.Should().Be(created.Id);
        fetched.Name.Should().Be(created.Name);
    }

    [Fact]
    public async Task GetById_WhenMissing_Returns404()
    {
        var bearer = await _client.GetBearerTokenAsync();
        _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(bearer);

        var res = await _client.GetAsync($"/api/v1/genres/{Guid.NewGuid()}");
        res.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_Returns204_And_PersistsChanges()
    {
        var bearer = await _client.GetBearerTokenAsync();
        _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(bearer);

        // create
        var create = await _client.PostAsJsonAsync("/api/v1/genres", new CreateGenreRequest
        {
            Name = "Rock"
        });

        create.StatusCode.Should().Be(HttpStatusCode.Created);

        var created = await create.Content.ReadFromJsonAsync<Genre>(JsonTestOptions.Default);
        created!.Id.Should().NotBeEmpty();

        // update
        var put = await _client.PutAsJsonAsync($"/api/v1/genres/{created.Id}", new UpdateGenreRequest
        {
            Name = "Pop"
        });

        put.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // roundtrip
        var get = await _client.GetAsync($"/api/v1/genres/{created.Id}");
        get.StatusCode.Should().Be(HttpStatusCode.OK);

        var updated = await get.Content.ReadFromJsonAsync<Genre>(JsonTestOptions.Default);
        updated.Should().NotBeNull();
        updated!.Name.Should().Be("Pop");
    }

    [Fact]
    public async Task Update_WhenMissing_Returns404_ProblemJson()
    {
        var bearer = await _client.GetBearerTokenAsync();
        _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(bearer);

        var id = Guid.NewGuid();
        var res = await _client.PutAsJsonAsync($"/api/v1/genres/{id}", new UpdateGenreRequest
        {
            Name = "Salsa",
        });

        res.StatusCode.Should().Be(HttpStatusCode.NotFound);
        Assert.Equal("application/problem+json", res.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task Delete_Returns404_ProblemJson()
    {
        var bearer = await _client.GetBearerTokenAsync();
        _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(bearer);


        // delete
        var del = await _client.DeleteAsync($"/api/v1/genres/{Guid.NewGuid()}");
        del.StatusCode.Should().Be(HttpStatusCode.NotFound);

        Assert.Equal("application/problem+json", del.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task Delete_Returns204_And_Then_GetReturns404()
    {
        var bearer = await _client.GetBearerTokenAsync();
        _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(bearer);

        // create
        var create = await _client.PostAsJsonAsync("/api/v1/genres", new CreateGenreRequest
        {
            Name = "Hip Hop"
        });
        create.StatusCode.Should().Be(HttpStatusCode.Created);

        var created = await create.Content.ReadFromJsonAsync<Genre>(JsonTestOptions.Default);

        // delete
        var del = await _client.DeleteAsync($"/api/v1/genres/{created!.Id}");
        del.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // get -> 404
        var get = await _client.GetAsync($"/api/v1/genres/{created.Id}");
        get.StatusCode.Should().Be(HttpStatusCode.NotFound);

        Assert.Equal("application/problem+json", get.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task Create_WhenInvalid_Returns400_ValidationProblemJson()
    {
        var bearer = await _client.GetBearerTokenAsync();
        _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(bearer);

        var res = await _client.PostAsJsonAsync("/api/v1/genres", new CreateGenreRequest
        {
            Name = "R"
        });

        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        Assert.Equal("application/problem+json", res.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task GetAll_WithPaging_ReturnsPagedResultWithLinks()
    {
        var bearer = await _client.GetBearerTokenAsync();
        _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(bearer);

        // Arrange: create 3 genres
        foreach (var name in new[] { "Rock", "Electronic", "Jazz" })
        {
            var create = await _client.PostAsJsonAsync("/api/v1/genres", new CreateGenreRequest
            {
                Name = name
            });
            create.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        // Act
        var res = await _client.GetAsync("/api/v1/genres?page=1&pageSize=2");
        res.StatusCode.Should().Be(HttpStatusCode.OK);

        var page = await res.Content.ReadFromJsonAsync<PagedResult<Genre>>(JsonTestOptions.Default);
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
    public async Task Put_WhenDomainRuleViolation_Returns409ProblemJson()
    {
        var bearer = await _client.GetBearerTokenAsync();
        _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(bearer);

        // create
        var create = await _client.PostAsJsonAsync("/api/v1/genres", new CreateGenreRequest
        {
            Name = "Vallenato"
        });

        create.StatusCode.Should().Be(HttpStatusCode.Created);

        var create2 = await _client.PostAsJsonAsync("/api/v1/genres", new CreateGenreRequest
        {
            Name = "Blue"
        });

        create2.StatusCode.Should().Be(HttpStatusCode.Created);

        var created2 = await create2.Content.ReadFromJsonAsync<Genre>(JsonTestOptions.Default);


        // update with same name (domain rule)
        var update = await _client.PutAsJsonAsync($"/api/v1/genres/{created2!.Id}", new UpdateGenreRequest
        {
            Name = "Vallenato"
        });

        update.StatusCode.Should().Be(HttpStatusCode.Conflict);
        Assert.Equal("application/problem+json", update.Content.Headers.ContentType?.MediaType);

        var problem = await update.Content.ReadFromJsonAsync<EventHouseProblemDetails>(JsonTestOptions.Default);
        problem!.ErrorCode.Should().Be("GENRE_NAME_ALREADY_EXISTS");
    }
}
