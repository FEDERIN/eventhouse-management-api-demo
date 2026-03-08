using EventHouse.Management.Application.Common.Sorting;
using EventHouse.Management.Application.Queries.EventVenues.GetAll;
using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Domain.Enums;
using EventHouse.Management.Infrastructure.Repositories;
using EventHouse.Management.Infrastructure.Tests.Extensions;
using EventHouse.Management.Infrastructure.Tests.Persistence;
using FluentAssertions;

namespace EventHouse.Management.Infrastructure.Tests.Repositories;

public sealed class EventVenueRepositoryTests : BasePersistenceTest
{
    private readonly EventVenueRepository _repository;

    public EventVenueRepositoryTests()
    {
        _repository = new EventVenueRepository(Context);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowInvalidOperationException_WhenEntityIsDetached()
    {
        var eventVenue = CreateEventVenueNoTracking();
        var act = async () => await _repository.UpdateAsync(eventVenue, TestContext.Current.CancellationToken);

        await act.ShouldThrowDetachedException();
    }

    [Fact]
    public async Task GetPagedAsync_ShouldFilterByVenueId()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var venueId1 = Guid.NewGuid();
        var venueId2 = Guid.NewGuid();

        // Sembramos un evento y dos recintos distintos
        await SeedAsync(new Event(eventId, "Festival", "Desc", EventScope.Local));

        await SeedAsync(
            new Venue(venueId1, "Arena A", "Addr", "City", "Reg", "US", 0, 0, "UTC", 100, true),
            new Venue(venueId2, "Arena B", "Addr", "City", "Reg", "US", 0, 0, "UTC", 100, true)
        );

        await SeedAsync(
            new EventVenue(Guid.NewGuid(), eventId, venueId1, EventVenueStatus.Active),
            new EventVenue(Guid.NewGuid(), eventId, venueId2, EventVenueStatus.Active)
            );

        var criteria = new EventVenueQueryCriteria
        {
            VenueId = venueId1,
            Page = 1,
            PageSize = 10
        };

        // Act
        var result = await _repository.GetPagedAsync(criteria, TestContext.Current.CancellationToken);

        // Assert
        result.Items.Should().HaveCount(1);
        result.Items[0].VenueId.Should().Be(venueId1);
        result.Items[0].EventId.Should().Be(eventId);
    }

    [Fact]
    public async Task GetPagedAsync_ShouldFilterByEventId()
    {
        // Arrange
        var eventId1 = Guid.NewGuid();
        var eventId2 = Guid.NewGuid();
        var venueId = Guid.NewGuid();

        await SeedAsync(
            new Event(eventId1, "Rock Concert", "Desc", EventScope.Local),
            new Event(eventId2, "Pop Expo", "Desc", EventScope.Local)
        );

        await SeedAsync(
            new Venue(venueId, "Main Stadium", "Addr", "City", "Reg", "US", 0, 0, "UTC", 100, true)
        );

        await SeedAsync(
            new EventVenue(Guid.NewGuid(), eventId1, venueId, EventVenueStatus.Active),
            new EventVenue(Guid.NewGuid(), eventId2, venueId, EventVenueStatus.Active)
            );

        var criteria = new EventVenueQueryCriteria
        {
            EventId = eventId1,
            Page = 1,
            PageSize = 10
        };

        // Act
        var result = await _repository.GetPagedAsync(criteria, TestContext.Current.CancellationToken);

        // Assert
        result.Items.Should().HaveCount(1);
        result.Items[0].EventId.Should().Be(eventId1);
    }

    [Fact]
    public async Task GetPagedAsync_ShouldFilterByStatus()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var venueId = Guid.NewGuid();
        var venueId2 = Guid.NewGuid();

        await SeedAsync(
            new Event(eventId, "Cinema Night", "Desc", EventScope.Local)
        );

        await SeedAsync(
            new Venue(venueId, "Local Theater 4", "Addr", "City", "Reg", "US", 0, 0, "UTC", 100, true),
            new Venue(venueId2, "Local Theater 2", "Addr", "City", "Reg", "US", 0, 0, "UTC", 100, true)
        );

        await SeedAsync(
            new EventVenue(Guid.NewGuid(), eventId, venueId, EventVenueStatus.Active),
            new EventVenue(Guid.NewGuid(), eventId, venueId2, EventVenueStatus.Inactive)
        );

        var criteria = new EventVenueQueryCriteria
        {
            Status = EventVenueStatus.Inactive,
            Page = 1,
            PageSize = 10
        };

        // Act
        var result = await _repository.GetPagedAsync(criteria, TestContext.Current.CancellationToken);

        // Assert
        result.Items.Should().HaveCount(1);
        result.Items[0].Status.Should().Be(EventVenueStatus.Inactive);
    }

    [Theory]
    [InlineData(EventVenueSortField.Status, SortDirection.Asc, EventVenueStatus.Active)]
    [InlineData(EventVenueSortField.Status, SortDirection.Desc, EventVenueStatus.Inactive)]
    [InlineData(null, SortDirection.Asc, EventVenueStatus.Active)]
    [InlineData(null, SortDirection.Desc, EventVenueStatus.Inactive)]
    public async Task GetPagedAsync_ShouldApplyCorrectSorting(
        EventVenueSortField? sortField,
        SortDirection direction,
        EventVenueStatus expectedStatus)
    {
        var eventId = Guid.NewGuid();
        var eventId2 = Guid.NewGuid();
        var venueId = Guid.NewGuid();

        await SeedAsync(
            new Event(eventId, "Rock Fest", "Desc", EventScope.Local),
            new Event(eventId2, "Jazz Night", "Desc", EventScope.Local)
            );

        await SeedAsync(
            new Venue(venueId, "Arena", "Addr", "City", "Reg", "US", 0, 0, "UTC", 100, true)
            );

        await SeedAsync(
            new EventVenue(Guid.NewGuid(), eventId, venueId, EventVenueStatus.Active),
            new EventVenue(Guid.NewGuid(), eventId2, venueId, EventVenueStatus.Inactive)
            );

        var criteria = new EventVenueQueryCriteria { SortBy = sortField, SortDirection = direction };
        var result = await _repository.GetPagedAsync(criteria, TestContext.Current.CancellationToken);

        // Assert
        result.Items[0].Status.Should().Be(expectedStatus);
    }

    private static EventVenue CreateEventVenueNoTracking()
    {
        return new EventVenue(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            EventVenueStatus.Active);
    }
}
