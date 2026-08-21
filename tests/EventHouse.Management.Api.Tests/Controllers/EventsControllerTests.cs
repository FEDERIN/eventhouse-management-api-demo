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
    #region READ (GET)

    [Fact]
    public async Task GetAll_WithPaging_ReturnsPagedResultWithLinks()
    {
        // Arrange: create 3 events
        foreach (var name in new[] { "E1", "E2", "E3" })
        {
            await CreateEventAsync(name);
        }

        // Act
        var res = await Client.GetAsync($"{BaseUrlEvents}?page=1&pageSize=2", TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(HttpStatusCode.OK);

        var page = await res.Content.ReadFromJsonAsync<PagedResult<EventResponse>>(JsonTestOptions.Default, TestContext.Current.CancellationToken);
        page.Should().NotBeNull();

        page!.Items.Should().NotBeNull();
        page.Items.Count.Should().BeLessThanOrEqualTo(2);
        page.ShouldHaveValidPaginationLinks(currentPage: 1, expectedPageSize: 2);

    }

    [Theory]
    [InlineData("Concierto", null, null, EventSortBy.Name, SortDirection.Asc)]
    [InlineData(null, "Rock", EventScope.National, EventSortBy.Name, SortDirection.Desc)]
    [InlineData("Festival", "Electronica", EventScope.International, EventSortBy.Name, SortDirection.Asc)]
    public async Task GetEvents_WithFiltersAndSorting_ReturnsFilteredResults(
            string? name,
            string? description,
            EventScope? scope,
            EventSortBy sortBy,
            SortDirection sortDirection)
    {
        var url = $"{BaseUrlEvents}?" +
                  (name != null ? $"name={name}&" : "") +
                  (description != null ? $"description={description}&" : "") +
                  (scope.HasValue ? $"scope={scope}&" : "") +
                  $"sortBy={sortBy}&sortDirection={sortDirection}";

        // Act
        var response = await Client.GetAsync(url, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData(EventSortBy.Name, SortDirection.Asc)]
    [InlineData(EventSortBy.Name, SortDirection.Desc)]
    [InlineData(EventSortBy.Description, SortDirection.Asc)]
    [InlineData(EventSortBy.Description, SortDirection.Desc)]
    [InlineData(EventSortBy.Scope, SortDirection.Asc)]
    [InlineData(EventSortBy.Scope, SortDirection.Desc)]
    [InlineData(null, SortDirection.Asc)]
    [InlineData(null, SortDirection.Desc)]
    public async Task GetAll_WithSorting_ReturnsSortedResults(EventSortBy? sortColumn, SortDirection direction)
    {
        // Arrange: create events with varying properties
        var eventsToCreate = new[]
        {
            new CreateEventRequest { Name = "Alpha", Description = "First", Scope = EventScope.Local },
            new CreateEventRequest { Name = "Charlie", Description = "Third", Scope = EventScope.International },
            new CreateEventRequest { Name = "Bravo", Description = "Second", Scope = EventScope.Local }
        };

        foreach (var req in eventsToCreate)
        {
            await CreateEventAsync(name : req.Name, description :req.Description, scope : req.Scope);
        }

        var url = $"{BaseUrlEvents}?sortBy={sortColumn}&sortDirection={direction}";


        // Act
        var response = await Client.GetAsync(url, TestContext.Current.CancellationToken);
        var pagedResult = await response.ReadContentAsync<PagedResult<EventResponse>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = pagedResult.Items;

        switch (sortColumn)
        {
            case EventSortBy.Description:
                ValidateOrder(items.Select(x => x.Description), direction);
                break;

            case EventSortBy.Scope:
                ValidateOrder(items.Select(x => x.Scope), direction);
                break;

            case EventSortBy.Name:
                ValidateOrder(items.Select(x => x.Name), direction);
                break;
        }
    }

    #endregion

    #region CREATE (POST)

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
        var post = await Client.PostAsJsonAsync(BaseUrlEvents, request, TestContext.Current.CancellationToken);

        // Assert: 201
        post.StatusCode.Should().Be(HttpStatusCode.Created);

        // Assert: body
        var created = await post.Content.ReadFromJsonAsync<EventResponse>(JsonTestOptions.Default, TestContext.Current.CancellationToken);

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
        var get = await Client.GetAsync($"{BaseUrlEvents}/{created.Id}", TestContext.Current.CancellationToken);
        get.StatusCode.Should().Be(HttpStatusCode.OK);

        var fetched = await get.Content.ReadFromJsonAsync<EventResponse>(JsonTestOptions.Default, TestContext.Current.CancellationToken);

        fetched.Should().BeEquivalentTo(created, opt => opt
            .ExcludingMissingMembers()
        );
    }


    [Fact]
    public async Task Create_WhenInvalid_Returns400_ValidationProblemJson()
    {
        var res = await Client.PostAsJsonAsync(BaseUrlEvents, new CreateEventRequest
        {
            Name = "A", // too short (min 2)
            Description = null,
            Scope = EventScope.Local
        }, TestContext.Current.CancellationToken);

        await res.ShouldBeProblemJson(HttpStatusCode.BadRequest);
    }
    #endregion

    #region UPDATE (PUT)
    [Fact]
    public async Task Update_Returns204_And_PersistsChanges()
    {
        // Arrange
        var created = await CreateEventAsync(name: "Event A", description: "Initial description", scope: EventScope.Local);

        var updateRequest = new UpdateEventRequest
        {
            Name = "Event A Updated",
            Description = "Updated description",
            Scope = EventScope.International
        };

        // Act & Assert
        await PutAndAssertPersisted<UpdateEventRequest, EventResponse>(
            $"{BaseUrlEvents}/{created.Id}",
            updateRequest
        );
    }

    [Fact]
    public async Task Update_WhenMissing_Returns404_ProblemJson()
    {
        await AssertPutReturns404($"{BaseUrlEvents}/{Guid.NewGuid()}", new UpdateEventRequest
        {
            Name = "Does not matter",
            Description = "Does not matter",
            Scope = EventScope.Local
        });
    }

    #endregion

    #region DELETE
    [Fact]
    public async Task Delete_Returns204_And_Then_GetReturns404()
    {
        //Arrange
        var created = await CreateEventAsync(name: "Event To Delete", description: "To be deleted", scope: EventScope.Local);
        var url = $"{BaseUrlEvents}/{created!.Id}";

        //Act && Assert
        await AssertDeleteReturns204(url);
        await AssertGetReturns404(url);

    }
    #endregion
}
