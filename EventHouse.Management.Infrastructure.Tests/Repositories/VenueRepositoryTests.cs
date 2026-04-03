using EventHouse.Management.Application.Common.Sorting;
using EventHouse.Management.Application.Queries.Venues.GetAll;
using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Infrastructure.Repositories;
using EventHouse.Management.Infrastructure.Tests.Persistence;
using EventHouse.Management.TestUtils.Factories;
using FluentAssertions;

namespace EventHouse.Management.Infrastructure.Tests.Repositories;

public sealed class VenueRepositoryTests : BasePersistenceTest
{
    private readonly VenueRepository _repository;

    public VenueRepositoryTests()
    {
        _repository = new VenueRepository(Context);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowInvalidOperationException_WhenEntityIsDetached()
    {
        // Arrange
        var venue = TestEntityFactory.CreateVenue(name: "Original Name");

        // Act
        var act = async () => await _repository.UpdateAsync(venue, TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("UpdateAsync requires a tracked entity. Use GetTrackedByIdAsync.");
    }

    [Fact]
    public async Task GetPagedAsync_ShouldFilterByName_WhenPartialMatch()
    {
        // Arrange
        await SeedAsync(
            TestEntityFactory.CreateVenue(name: "Madison Square Garden"),
            TestEntityFactory.CreateVenue(name: "Miami Arena")
        );

        var criteria = new VenueQueryCriteria { Name = "Square" };

        // Act
        var result = await _repository.GetPagedAsync(criteria, TestContext.Current.CancellationToken);

        // Assert
        result.Items.Should().ContainSingle();
        result.Items[0].Name.Should().StartWith("Madison Square Garden");
    }

    [Fact]
    public async Task GetPagedAsync_ShouldFilterByCityAndIsActive()
    {
        // Arrange
        await SeedAsync(
            TestEntityFactory.CreateVenue(name: "Arena 1", city: "Miami", isActive: true),
            TestEntityFactory.CreateVenue(name: "Arena 2", city: "Orlando", isActive: true),
            TestEntityFactory.CreateVenue(name: "Arena 3", city: "Miami", isActive: false)
            );

        var criteria = new VenueQueryCriteria { City = "Miami", IsActive = true };

        // Act
        var result = await _repository.GetPagedAsync(criteria, TestContext.Current.CancellationToken);

        // Assert
        result.Items.Should().ContainSingle();
        result.Items[0].Name.Should().StartWith("Arena 1");
    }

    [Fact]
    public async Task GetPagedAsync_ShouldFilterByCapacity_And_ExactRegion()
    {
        // Arrange
        await SeedAsync(
            TestEntityFactory.CreateVenue(name: "V1", region: "Florida", capacity: 5000),
            TestEntityFactory.CreateVenue(name: "V2", region: "California", capacity: 100)
        );

        var criteria = new VenueQueryCriteria { Capacity = 1000, Region = "Florida" };

        // Act
        var result = await _repository.GetPagedAsync(criteria, TestContext.Current.CancellationToken);

        // Assert
        result.Items.Should().ContainSingle(v => v.Name.StartsWith("V1"));
    }

    [Fact]
    public async Task GetPagedAsync_ShouldFilterByAddress_WhenPartialMatch()
    {
        // Arrange
        await SeedAsync(
            TestEntityFactory.CreateVenue(name: "Arena Principal", address: "Av. Central 123"),
            TestEntityFactory.CreateVenue(name: "Arena Secundaria", address: "Calle Olvidada 456")
        );

        var criteria = new VenueQueryCriteria { Address = "Central" };

        // Act
        var result = await _repository.GetPagedAsync(criteria, TestContext.Current.CancellationToken);

        // Assert
        result.Items.Should().ContainSingle();
        result.Items[0].Address.Should().Contain("Central");
    }

    [Fact]
    public async Task GetPagedAsync_ShouldFilterByCountryCode_Exactly()
    {
        // Arrange
        await SeedAsync(
            TestEntityFactory.CreateVenue(name: "Venue US", countryCode: "US"),
            TestEntityFactory.CreateVenue(name: "Venue CA", countryCode: "CA")
        );

        var criteria = new VenueQueryCriteria { CountryCode = "US" };

        // Act
        var result = await _repository.GetPagedAsync(criteria, TestContext.Current.CancellationToken);

        // Assert
        result.Items.Should().ContainSingle();
        result.Items[0].CountryCode.Should().Be("US");
    }

    [Theory]
    [InlineData(VenueSortField.Name, SortDirection.Asc, "A-Arena")]
    [InlineData(VenueSortField.Name, SortDirection.Desc, "Z-Arena")]
    [InlineData(VenueSortField.Address, SortDirection.Asc, "A-Arena")]
    [InlineData(VenueSortField.Address, SortDirection.Desc, "Z-Arena")]
    [InlineData(VenueSortField.City, SortDirection.Asc, "A-Arena")]
    [InlineData(VenueSortField.City, SortDirection.Desc, "Z-Arena")]
    [InlineData(VenueSortField.Region, SortDirection.Asc, "A-Arena")]
    [InlineData(VenueSortField.Region, SortDirection.Desc, "Z-Arena")]
    [InlineData(VenueSortField.CountryCode, SortDirection.Asc, "Z-Arena")]
    [InlineData(VenueSortField.CountryCode, SortDirection.Desc, "A-Arena")]
    [InlineData(VenueSortField.Capacity, SortDirection.Asc, "A-Arena")]
    [InlineData(VenueSortField.Capacity, SortDirection.Desc, "Z-Arena")]
    [InlineData(VenueSortField.IsActive, SortDirection.Asc, "Z-Arena")]
    [InlineData(VenueSortField.IsActive, SortDirection.Desc, "A-Arena")]
    [InlineData(null, SortDirection.Asc, "A-Arena")]
    [InlineData(null, SortDirection.Desc, "Z-Arena")]
    public async Task GetPagedAsync_ShouldApplyCorrectSorting(
        VenueSortField? sortField,
        SortDirection direction,
        string expectedFirstName)
    {
        // Arrange
        await SeedAsync(
            TestEntityFactory.CreateVenue(Guid.NewGuid(), "A-Arena", "Addr", "City", "Reg", "US", 0, 0, "UTC", 100, true),
            TestEntityFactory.CreateVenue(Guid.NewGuid(), "Z-Arena", "Addr2", "City2", "Reg2", "CA", 0, 0, "UTC", 500, false)
        );

        var criteria = new VenueQueryCriteria { SortBy = sortField, SortDirection = direction };

        // Act
        var result = await _repository.GetPagedAsync(criteria, TestContext.Current.CancellationToken);

        // Assert
        result.Items[0].Name.Should().StartWith(expectedFirstName);
    }
}
