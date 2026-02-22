
using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.Venues;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace EventHouse.Management.Api.Tests.Controllers;

public sealed class VenuesControllerTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetAll_WithoutToken_Returns401()
    {
        var res = await _client.GetAsync("/api/v1/venues");
        res.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Create_Returns201_Location_And_CanGetById()
    {
        var bearer = await _client.GetBearerTokenAsync();
        _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(bearer);

        var request = new CreateVenueRequest
        {
            Name = "Miami International Arena",
            Address = "500 Biscayne Blvd, Miami, FL 33132",
            City = "Miami",
            Region = "FL",
            CountryCode = "US",
            Latitude = 25.7800m,
            Longitude = -80.1880m,
            TimeZoneId = "America/New_York",
            Capacity = 18000,
            IsActive = true
        };

        // Act
        var post = await _client.PostAsJsonAsync("/api/v1/venues", request);

        // Assert: 201
        post.StatusCode.Should().Be(HttpStatusCode.Created);

        // Assert: body
        var created = await post.Content.ReadFromJsonAsync<Venue>(JsonTestOptions.Default);

        created.Should().NotBeNull();
        created!.Id.Should().NotBeEmpty();
        created.Name.Should().Be(request.Name);
        created.Address.Should().Be(request.Address);
        created.City.Should().Be(request.City);
        created.Region.Should().Be(request.Region);
        created.CountryCode.Should().Be(request.CountryCode);
        created.Latitude.Should().Be(request.Latitude);
        created.Longitude.Should().Be(request.Longitude);
        created.TimeZoneId.Should().Be(request.TimeZoneId);
        created.Capacity.Should().Be(request.Capacity);
        created.IsActive.Should().Be(request.IsActive);

        // Assert: Location header matches CreatedAtAction(GetById)
        post.Headers.Location.Should().NotBeNull();
        var location = post.Headers.Location!.ToString();

        location.Should().Contain("/api/v1/venues/");
        location.Should().EndWith(created.Id.ToString());

        // Roundtrip: GET by id returns 200 and same resource
        var get = await _client.GetAsync($"/api/v1/venues/{created.Id}");
        get.StatusCode.Should().Be(HttpStatusCode.OK);

        var fetched = await get.Content.ReadFromJsonAsync<Venue>(JsonTestOptions.Default);
        fetched.Should().NotBeNull();
        fetched!.Id.Should().Be(created.Id);
        fetched.Name.Should().Be(created.Name);
        fetched.Address.Should().Be(created.Address);
        fetched.City.Should().Be(created.City);
        fetched.Region.Should().Be(created.Region);
        fetched.Latitude.Should().Be(created.Latitude);
        fetched.Longitude.Should().Be(created.Longitude);
        fetched.TimeZoneId.Should().Be(created.TimeZoneId);
        fetched.Capacity.Should().Be(created.Capacity);
        fetched.IsActive.Should().Be(created.IsActive);
    }

    [Fact]
    public async Task GetById_WhenMissing_Returns404()
    {
        var bearer = await _client.GetBearerTokenAsync();
        _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(bearer);

        var res = await _client.GetAsync($"/api/v1/venues/{Guid.NewGuid()}");
        res.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_Returns204_And_PersistsChanges()
    {
        var bearer = await _client.GetBearerTokenAsync();
        _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(bearer);


        // create
        var create = await _client.PostAsJsonAsync("/api/v1/venues", new CreateVenueRequest
        {
            Name = "Madison Square Garden",
            Address = "4 Pennsylvania Plaza, New York, NY 10001",
            City = "New York",
            Region = "NY",
            CountryCode = "US",
            Latitude = 40.7505m,
            Longitude = -73.9934m,
            TimeZoneId = "America/New_York",
            Capacity = 20000,
            IsActive = true
        });

        create.StatusCode.Should().Be(HttpStatusCode.Created);

        var created = await create.Content.ReadFromJsonAsync<Venue>(JsonTestOptions.Default);
        created!.Id.Should().NotBeEmpty();

        // update

        var update = new UpdateVenueRequest
        {
            Name = "Kaseya Center",
            Address = "601 Biscayne Blvd, Miami, FL 33132",
            City = "Miami",
            Region = "FL",
            CountryCode = "US",
            Latitude = 25.7814m,
            Longitude = -80.1870m,
            TimeZoneId = "America/Miami",
            Capacity = 19600,
            IsActive = true
        };

        var put = await _client.PutAsJsonAsync($"/api/v1/venues/{created.Id}", update);

        put.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // roundtrip
        var get = await _client.GetAsync($"/api/v1/venues/{created.Id}");
        get.StatusCode.Should().Be(HttpStatusCode.OK);

        var updated = await get.Content.ReadFromJsonAsync<Venue>(JsonTestOptions.Default);
        updated.Should().NotBeNull();
        updated!.Id.Should().NotBeEmpty();
        updated.Name.Should().Be(update.Name);
        updated.Address.Should().Be(update.Address);
        updated.City.Should().Be(update.City);
        updated.Region.Should().Be(update.Region);
        updated.CountryCode.Should().Be(update.CountryCode);
        updated.Latitude.Should().Be(update.Latitude);
        updated.Longitude.Should().Be(update.Longitude);
        updated.TimeZoneId.Should().Be(update.TimeZoneId);
        updated.Capacity.Should().Be(update.Capacity);
        updated.IsActive.Should().Be(update.IsActive);
    }

    [Fact]
    public async Task Update_WhenMissing_Returns404_ProblemJson()
    {
        var bearer = await _client.GetBearerTokenAsync();
        _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(bearer);

        var id = Guid.NewGuid();
        var res = await _client.PutAsJsonAsync($"/api/v1/venues/{id}", new UpdateVenueRequest
        {
            Name = "Kaseya Center 3",
            Address = "601 Biscayne Blvd, Miami, FL 33132",
            City = "Miami",
            Region = "FL",
            CountryCode = "US",
            Latitude = 25.7814m,
            Longitude = -80.1870m,
            TimeZoneId = "America/Miami",
            Capacity = 19600,
            IsActive = true
        });

        res.StatusCode.Should().Be(HttpStatusCode.NotFound);
        Assert.Equal("application/problem+json", res.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task Delete_Returns204()
    {
        var bearer = await _client.GetBearerTokenAsync();
        _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(bearer);

        // create
        var create = await _client.PostAsJsonAsync("/api/v1/venues", new CreateVenueRequest
        {
            Name = "Kaseya Center 2",
            Address = "601 Biscayne Blvd, Miami, FL 33132",
            City = "Miami",
            Region = "FL",
            CountryCode = "US",
            Latitude = 25.7814m,
            Longitude = -80.1870m,
            TimeZoneId = "America/Miami",
            Capacity = 19600,
            IsActive = true
        });
        create.StatusCode.Should().Be(HttpStatusCode.Created);

        var created = await create.Content.ReadFromJsonAsync<Venue>(JsonTestOptions.Default);

        // delete
        var del = await _client.DeleteAsync($"/api/v1/venues/{created!.Id}");
        del.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_WhenMissing_Returns404_ProblemJson()
    {
        var bearer = await _client.GetBearerTokenAsync();
        _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(bearer);


        var id = Guid.NewGuid();

        // delete
        var del = await _client.DeleteAsync($"/api/v1/venues/{id}");

        del.StatusCode.Should().Be(HttpStatusCode.NotFound);
        Assert.Equal("application/problem+json", del.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task Create_WhenInvalid_Returns400_ValidationProblemJson()
    {
        var bearer = await _client.GetBearerTokenAsync();
        _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(bearer);

        var res = await _client.PostAsJsonAsync("/api/v1/venues", new CreateVenueRequest
        {
            Name = "A",
            Address = "1",
            CountryCode = "USD",
            Latitude = 0m,
            Longitude = 0m,
            TimeZoneId = "UTC",
            Capacity = 100,
            IsActive = true
        });

        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        Assert.Equal("application/problem+json", res.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task GetAll_WithPaging_ReturnsPagedResultWithLinks()
    {
        var bearer = await _client.GetBearerTokenAsync();
        _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(bearer);

        foreach (var name in new[] { "E1", "E2", "E3" })
        {
            var create = await _client.PostAsJsonAsync("/api/v1/venues", new CreateVenueRequest
            {
                Name = name,
                Address = "123 Test St, Test City, TS 12345",
                City = "Test City",
                Region = "TS",
                CountryCode = "US",
                Latitude = 0m,
                Longitude = 0m,
                TimeZoneId = "UTC",
                Capacity = 100,
                IsActive = true
            });
            create.StatusCode.Should().Be(HttpStatusCode.Created);
        }


        var res = await _client.GetAsync("/api/v1/venues?page=1&pageSize=2");
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        var page = await res.Content.ReadFromJsonAsync<PagedResult<Venue>>(JsonTestOptions.Default);

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
