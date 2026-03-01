using EventHouse.Management.Application.Common.Sorting;
using EventHouse.Management.Application.Queries.Venues.GetAll;
using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Infrastructure.Repositories;
using EventHouse.Management.Infrastructure.Tests.Persistence;
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
        var venue = CreateValidVenue("Original Name", "Miami", true);
        // No lo agregamos vía repositorio para que no esté "Tracked", 
        // o simplemente creamos una instancia nueva con un ID existente.

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
            CreateValidVenue("Madison Square Garden", "New York", true),
            CreateValidVenue("Miami Arena", "Miami", true)
        );

        var criteria = new VenueQueryCriteria { Name = "Square" };

        // Act
        var result = await _repository.GetPagedAsync(criteria, TestContext.Current.CancellationToken);

        // Assert
        result.Items.Should().ContainSingle();
        result.Items.First().Name.Should().Be("Madison Square Garden");
    }

    [Fact]
    public async Task GetPagedAsync_ShouldFilterByCityAndIsActive()
    {
        // Arrange
        await SeedAsync(
            CreateValidVenue("Arena 1", "Miami", true),
            CreateValidVenue("Arena 2", "Orlando", true),
            CreateValidVenue("Arena 3", "Miami", false)
            );

        var criteria = new VenueQueryCriteria
        {
            City = "Miami",
            IsActive = true
        };

        // Act
        var result = await _repository.GetPagedAsync(criteria, TestContext.Current.CancellationToken);

        // Assert
        result.Items.Should().ContainSingle();
        result.Items[0].Name.Should().Be("Arena 1");
        result.TotalCount.Should().Be(1);
    }

    [Fact]
    public async Task GetPagedAsync_ShouldFilterByCapacity_And_ExactRegion()
    {
        // Arrange
        var v1 = new Venue(Guid.NewGuid(), "V1", "Add", "City", "Florida", "US", 0, 0, "UTC", 5000, true);
        var v2 = new Venue(Guid.NewGuid(), "V2", "Add", "City", "California", "US", 0, 0, "UTC", 100, true);
        await SeedAsync(v1, v2);

        var criteria = new VenueQueryCriteria { Capacity = 1000, Region = "Florida" };

        // Act
        var result = await _repository.GetPagedAsync(criteria, TestContext.Current.CancellationToken);

        // Assert
        result.Items.Should().ContainSingle(v => v.Name == "V1");
    }

    [Fact]
    public async Task GetPagedAsync_ShouldFilterByAddress_WhenPartialMatch()
    {
        // Arrange
        await SeedAsync(
            CreateValidVenueWithAddress("Arena Principal", "Av. Central 123"),
            CreateValidVenueWithAddress("Arena Secundaria", "Calle Olvidada 456")
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
            new Venue(Guid.NewGuid(), "Venue US", "Addr", "City", "Reg", "US", 0, 0, "UTC", 100, true),
            new Venue(Guid.NewGuid(), "Venue CA", "Addr", "City", "Reg", "CA", 0, 0, "UTC", 100, true)
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
        // Arrange: Limpiamos y creamos datos frescos
        await SeedAsync(
            new Venue(Guid.NewGuid(), "A-Arena", "Addr", "City", "Reg", "US", 0, 0, "UTC", 100, true),
            new Venue(Guid.NewGuid(), "Z-Arena", "Addr2", "City2", "Reg2", "CA", 0, 0, "UTC", 500, false)
        );

        var criteria = new VenueQueryCriteria
        {
            SortBy = sortField,
            SortDirection = direction
        };

        // Act
        var result = await _repository.GetPagedAsync(criteria, TestContext.Current.CancellationToken);

        // Assert
        result.Items[0].Name.Should().Be(expectedFirstName);
    }


    // Helper adicional para address
    private static Venue CreateValidVenueWithAddress(string name, string address)
    {
        return new Venue(Guid.NewGuid(), name, address, "City", "Region", "US", 0, 0, "UTC", 100, true);
    }


    private static Venue CreateValidVenue(string name, string city, bool isActive)
    {
        return new Venue(
            Guid.NewGuid(),
            name,
            "Direccion Test",
            city,
            "Region Test",
            "US",
            0, 0,
            "UTC",
            100,
            isActive);
    }
}
