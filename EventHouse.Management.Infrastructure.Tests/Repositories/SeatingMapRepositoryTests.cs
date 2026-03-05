using EventHouse.Management.Application.Common.Sorting;
using EventHouse.Management.Application.Queries.SeatingMaps.GetAll;
using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Infrastructure.Repositories;
using EventHouse.Management.Infrastructure.Tests.Persistence;
using FluentAssertions;

namespace EventHouse.Management.Infrastructure.Tests.Repositories;

public class SeatingMapRepositoryTests : BasePersistenceTest
{
    private readonly SeatingMapRepository _repository;
    private readonly VenueRepository _venueRepository;

    public SeatingMapRepositoryTests()
    {
        _repository = new SeatingMapRepository(Context);
        _venueRepository = new VenueRepository(Context);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowInvalidOperationException_WhenEntityIsDetached()
    {
        // Arrange
        var seatingMap = CreateValidSeatingMap("Central", Guid.NewGuid(), 1,  true);

        // Act
        var act = async () => await _repository.UpdateAsync(seatingMap, TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("UpdateAsync requires a tracked entity. Use GetTrackedByIdAsync.");
    }

    [Fact]
    public async Task GetPagedAsync_ShouldFilterByName_WhenPartialMatch()
    {
        // Arrange
        var venueId = Guid.NewGuid();
        await _venueRepository.AddAsync(CreateValidVenue(venueId, "Venue1", "City1", true), TestContext.Current.CancellationToken);
        await SeedAsync(
            CreateValidSeatingMap("Central", venueId, 1, true),
            CreateValidSeatingMap("North", venueId, 2, true)
        );
        var criteria = new SeatingMapQueryCriteria { Name = "Cent" };
        // Act
        var result = await _repository.GetPagedAsync(criteria, TestContext.Current.CancellationToken);
        // Assert
        result.Items.Should().ContainSingle();
        result.Items[0].Name.Should().Be("Central");
    }

    [Fact]
    public async Task GetPagedAsync_ShouldFilterByIsActive()
    {
        // Arrange
        var venueId = Guid.NewGuid();
        await _venueRepository.AddAsync(CreateValidVenue(venueId, "Venue1", "City1", true), TestContext.Current.CancellationToken);
        await SeedAsync(
            CreateValidSeatingMap("Central", venueId, 1, true),
            CreateValidSeatingMap("North", venueId, 2, false)
        );
        var criteria = new SeatingMapQueryCriteria { IsActive = true };
        // Act
        var result = await _repository.GetPagedAsync(criteria, TestContext.Current.CancellationToken);
        // Assert
        result.Items.Should().ContainSingle();
        result.Items[0].Name.Should().Be("Central");
    }

    [Fact]
    public async Task GetPagedAsync_ShouldFilterByVenueId()
    {
        // Arrange
        var venueId1 = Guid.NewGuid();
        var venueId2 = Guid.NewGuid();
        await _venueRepository.AddAsync(CreateValidVenue(venueId1, "Venue1", "City1", true), TestContext.Current.CancellationToken);
        await _venueRepository.AddAsync(CreateValidVenue(venueId2, "Venue2", "City2", true), TestContext.Current.CancellationToken);
        await SeedAsync(
            CreateValidSeatingMap("Central", venueId1, 1, true),
            CreateValidSeatingMap("North", venueId2, 2, true)
        );
        var criteria = new SeatingMapQueryCriteria { VenueId = venueId1 };
        // Act
        var result = await _repository.GetPagedAsync(criteria, TestContext.Current.CancellationToken);
        // Assert
        result.Items.Should().ContainSingle();
        result.Items[0].Name.Should().Be("Central");
    }

    [Theory]
    [InlineData(SeatingMapSortField.Name, SortDirection.Asc, "Alpha")]
    [InlineData(SeatingMapSortField.Name, SortDirection.Desc, "Delta")]
    [InlineData(SeatingMapSortField.Version, SortDirection.Asc, "Alpha")]
    [InlineData(SeatingMapSortField.Version, SortDirection.Desc, "Delta")]
    [InlineData(SeatingMapSortField.IsActive, SortDirection.Asc, "Charlie")]
    [InlineData(SeatingMapSortField.IsActive, SortDirection.Desc, "Alpha")]
    public async Task GetPagedAsync_ShouldApplyCorrectSorting(
    SeatingMapSortField? sortField,
    SortDirection direction,
    string expectedFirstName)
    {         
        var venueId = Guid.NewGuid();
        await _venueRepository.AddAsync(CreateValidVenue(venueId, "Venue1", "City1", true), TestContext.Current.CancellationToken);

        // Arrange
        await SeedAsync(
            CreateValidSeatingMap("Charlie", venueId, 2, false),
            CreateValidSeatingMap("Alpha", venueId, 1, true),
            CreateValidSeatingMap("Delta", venueId, 3, true)
            );

        var criteria = new SeatingMapQueryCriteria { SortBy = sortField, SortDirection = direction };

        // Act
        var result = await _repository.GetPagedAsync(criteria, TestContext.Current.CancellationToken);
        // Assert
        result.Items[0].Name.Should().Be(expectedFirstName);
    }


    private static SeatingMap CreateValidSeatingMap(string name, Guid venueId, int version, bool isActive)
    {
        return new SeatingMap(Guid.NewGuid(), venueId, name, version, isActive);
    }

    private static Venue CreateValidVenue(Guid id, string name, string city, bool isActive)
    {
        return new Venue(
            id,
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
