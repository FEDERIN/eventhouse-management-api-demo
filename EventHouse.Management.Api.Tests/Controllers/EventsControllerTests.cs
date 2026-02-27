using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.Events;
using EventHouse.Management.Api.Tests.Abstractions;
using EventHouse.Management.Api.Tests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace EventHouse.Management.Api.Tests.Controllers;

public sealed class EventsControllerTests(CustomWebApplicationFactory factory)
    : BaseIntegrationTest(factory)
{
    private const string BaseUrl = ApiRoutes.Events;

    [Fact]
    public async Task GetAll_WithoutToken_Returns401()
    {

        var request = new HttpRequestMessage(HttpMethod.Get, BaseUrl).WithoutAuthentication();

        var res = await Client.SendAsync(request);

        res.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Create_Returns201_Location_And_CanGetById()
    {
        var request = new CreateEventRequest
        {
            Name = "Summer Fest 2026",
            Description = "Annual open-air music festival.",
            Scope = EventScope.Local
        };

        // Act
        var post = await Client.PostAsJsonAsync(BaseUrl, request);

        // Assert: 201
        post.StatusCode.Should().Be(HttpStatusCode.Created);

        // Assert: body
        var created = await post.Content.ReadFromJsonAsync<EventResponse>(JsonTestOptions.Default);

        created.Should().NotBeNull();
        created!.Id.Should().NotBeEmpty();
        created.Name.Should().Be("Summer Fest 2026");
        created.Description.Should().Be("Annual open-air music festival.");
        created.Scope.Should().Be(EventScope.Local);

        // Assert: Location header matches CreatedAtAction(GetById)
        post.Headers.Location.Should().NotBeNull();
        var location = post.Headers.Location!.ToString();

        location.Should().Contain("/api/v1/events/");
        location.Should().EndWith(created.Id.ToString());

        // Roundtrip: GET by id returns 200 and same resource
        var get = await Client.GetAsync($"{BaseUrl}/{created.Id}");
        get.StatusCode.Should().Be(HttpStatusCode.OK);

        var fetched = await get.Content.ReadFromJsonAsync<EventResponse>(JsonTestOptions.Default);
        fetched.Should().NotBeNull();
        fetched!.Id.Should().Be(created.Id);
        fetched.Name.Should().Be(created.Name);
        fetched.Description.Should().Be(created.Description);
        fetched.Scope.Should().Be(created.Scope);
    }

    [Fact]
    public async Task GetById_WhenMissing_Returns404()
    {
        var res = await Client.GetAsync($"{BaseUrl}/{Guid.NewGuid()}");
        await res.ShouldBeProblemJson(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_Returns204_And_PersistsChanges()
    {
        // create
        var create = await Client.PostAsJsonAsync(BaseUrl, new CreateEventRequest
        {
            Name = "Event A",
            Description = "Initial description",
            Scope = EventScope.Local
        });
        create.StatusCode.Should().Be(HttpStatusCode.Created);

        var created = await create.Content.ReadFromJsonAsync<EventResponse>(JsonTestOptions.Default);
        created!.Id.Should().NotBeEmpty();

        // update
        var put = await Client.PutAsJsonAsync($"{BaseUrl}/{created.Id}", new UpdateEventRequest
        {
            Name = "Event A Updated",
            Description = "Updated description",
            Scope = EventScope.International
        });

        put.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // roundtrip
        var get = await Client.GetAsync($"{BaseUrl}/{created.Id}");
        get.StatusCode.Should().Be(HttpStatusCode.OK);

        var updated = await get.Content.ReadFromJsonAsync<EventResponse>(JsonTestOptions.Default);
        updated.Should().NotBeNull();
        updated!.Name.Should().Be("Event A Updated");
        updated.Description.Should().Be("Updated description");
        updated.Scope.Should().Be(EventScope.International);
    }

    [Fact]
    public async Task Update_WhenMissing_Returns404_ProblemJson()
    {
        var id = Guid.NewGuid();
        var res = await Client.PutAsJsonAsync($"{BaseUrl}/{id}", new UpdateEventRequest
        {
            Name = "Does not matter",
            Description = "Does not matter",
            Scope = EventScope.Local
        });

        await res.ShouldBeProblemJson(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_Returns204_And_Then_GetReturns404()
    {
        // create
        var create = await Client.PostAsJsonAsync(BaseUrl, new CreateEventRequest
        {
            Name = "Event To Delete",
            Description = "To be deleted",
            Scope = EventScope.Local
        });
        create.StatusCode.Should().Be(HttpStatusCode.Created);

        var created = await create.Content.ReadFromJsonAsync<EventResponse>(JsonTestOptions.Default);

        // delete
        var del = await Client.DeleteAsync($"{BaseUrl}/{created!.Id}");
        del.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // get -> 404
        var get = await Client.GetAsync($"{BaseUrl}/{created.Id}");

        await get.ShouldBeProblemJson(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_WhenInvalid_Returns400_ValidationProblemJson()
    {
        var res = await Client.PostAsJsonAsync(BaseUrl, new CreateEventRequest
        {
            Name = "A", // too short (min 2)
            Description = null,
            Scope = EventScope.Local
        });

        await res.ShouldBeProblemJson(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetAll_WithPaging_ReturnsPagedResultWithLinks()
    {
        // Arrange: create 3 events
        foreach (var name in new[] { "E1", "E2", "E3" })
        {
            var create = await Client.PostAsJsonAsync(BaseUrl, new CreateEventRequest
            {
                Name = name,
                Description = "Demo",
                Scope = EventScope.Local
            });
            create.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        // Act
        var res = await Client.GetAsync($"{BaseUrl}?page=1&pageSize=2");
        res.StatusCode.Should().Be(HttpStatusCode.OK);

        var page = await res.Content.ReadFromJsonAsync<PagedResult<EventResponse>>(JsonTestOptions.Default);
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
}
