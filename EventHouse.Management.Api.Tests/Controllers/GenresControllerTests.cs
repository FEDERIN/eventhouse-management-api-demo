using EventHouse.Management.Api.Contracts.Genres;
using EventHouse.Management.Api.Tests.Abstractions;
using EventHouse.Management.Api.Tests.Common;
using EventHouse.Management.Domain.Entities;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace EventHouse.Management.Api.Tests.Controllers;

public sealed class GenresControllerTests(CustomWebApplicationFactory factory)
    : BaseIntegrationTest(factory)
{
    private const string BaseUrl = ApiRoutes.Genres;

    [Fact]
    public async Task Create_Returns201_And_CanGetById()
    {
        var request = new CreateGenreRequest { Name = "Country" };

        var postResponse = await Client.PostAsJsonAsync(BaseUrl, request);

        postResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await postResponse.Content.ReadFromJsonAsync<Genre>(JsonTestOptions.Default);

        created.Should().NotBeNull();
        postResponse.Headers.Location!.ToString().Should().EndWith(created!.Id.ToString());
    }

    [Fact]
    public async Task GetAll_WithoutToken_Returns401()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, BaseUrl).WithoutAuthentication();

        var res = await Client.SendAsync(request);

        res.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Update_Returns204_And_PersistsChanges()
    {
        var create = await Client.PostAsJsonAsync(BaseUrl, new CreateGenreRequest { Name = "Rock" });
        var created = await create.Content.ReadFromJsonAsync<Genre>(JsonTestOptions.Default);

        var put = await Client.PutAsJsonAsync($"{BaseUrl}/{created!.Id}", new UpdateGenreRequest { Name = "Pop" });
        put.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var updated = await Client.GetFromJsonAsync<Genre>($"{BaseUrl}/{created.Id}", JsonTestOptions.Default);
        updated!.Name.Should().Be("Pop");
    }

    [Fact]
    public async Task Update_WhenMissing_Returns404_ProblemJson()
    {
        var res = await Client.PutAsJsonAsync($"{BaseUrl}/{Guid.NewGuid()}", new UpdateGenreRequest { Name = "Salsa" });

        await res.ShouldBeProblemJson(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Put_WhenDomainRuleViolation_Returns409ProblemJson()
    {
        // Arrange
        await Client.PostAsJsonAsync(BaseUrl, new CreateGenreRequest { Name = "Vallenato" });
        var create2 = await Client.PostAsJsonAsync(BaseUrl, new CreateGenreRequest { Name = "Blue" });
        var created2 = await create2.Content.ReadFromJsonAsync<Genre>(JsonTestOptions.Default);

        // Act
        var update = await Client.PutAsJsonAsync($"{BaseUrl}/{created2!.Id}", new UpdateGenreRequest { Name = "Vallenato" });

        // Assert
        await update.ShouldHaveErrorCode(HttpStatusCode.Conflict, "GENRE_NAME_ALREADY_EXISTS");
    }
}