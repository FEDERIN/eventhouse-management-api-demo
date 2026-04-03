using EventHouse.Management.Application.Common.Sorting;
using EventHouse.Management.Application.Queries.EventVenueCalendars.GetAll;
using EventHouse.Management.Domain.Enums;
using EventHouse.Management.Infrastructure.Repositories;
using EventHouse.Management.Infrastructure.Tests.Extensions;
using EventHouse.Management.Infrastructure.Tests.Persistence;
using EventHouse.Management.TestUtils.Factories;
using FluentAssertions;

namespace EventHouse.Management.Infrastructure.Tests.Repositories;

public sealed class EventVenueCalendarRepositoryTests : BasePersistenceTest
{
    private readonly EventVenueCalendarRepository _repository;

    public EventVenueCalendarRepositoryTests()
    {
        _repository = new EventVenueCalendarRepository(Context);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowInvalidOperationException_WhenEntityIsDetached()
    {
        // Arrange
        var calendar = TestEntityFactory.CreateEventVenueCalendar(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
       
        var act = async () => await _repository.UpdateAsync(calendar, TestContext.Current.CancellationToken);

        await act.ShouldThrowDetachedException();


    }

    [Fact]
    public async Task GetPagedAsync_ShouldFilterByEventVenueId()
    {
        // Arrange
        var deps1 = await SeedDependenciesAsync();
        var (_, _, EventVenueId, SeatingMapId) = await SeedDependenciesAsync();

        await SeedAsync(
            TestEntityFactory.CreateEventVenueCalendar(Guid.NewGuid(), deps1.EventVenueId, deps1.SeatingMapId),
            TestEntityFactory.CreateEventVenueCalendar(Guid.NewGuid(), EventVenueId, SeatingMapId)
        );

        var criteria = new EventVenueCalendarQueryCriteria { EventVenueId = deps1.EventVenueId };

        // Act
        var result = await _repository.GetPagedAsync(criteria, TestContext.Current.CancellationToken);

        // Assert
        result.Items.Should().ContainSingle();
        result.Items[0].EventVenueId.Should().Be(deps1.EventVenueId);
    }

    [Fact]
    public async Task GetPagedAsync_ShouldFilterBySeatingMapId()
    {
        // Arrange
        var (_, VenueId, EventVenueId, SeatingMapId) = await SeedDependenciesAsync();
        var otherMapId = Guid.NewGuid();

        // Sembramos un segundo mapa para el mismo recinto
        await SeedAsync(TestEntityFactory.CreateSeatingMap(otherMapId, VenueId, "Secondary Map"));

        await SeedAsync(
            TestEntityFactory.CreateEventVenueCalendar(Guid.NewGuid(), EventVenueId, SeatingMapId),
            TestEntityFactory.CreateEventVenueCalendar(Guid.NewGuid(), EventVenueId, otherMapId)
        );

        var criteria = new EventVenueCalendarQueryCriteria { SeatingMapId = SeatingMapId };

        // Act
        var result = await _repository.GetPagedAsync(criteria, TestContext.Current.CancellationToken);

        // Assert
        result.Items.Should().ContainSingle();
        result.Items[0].SeatingMapId.Should().Be(SeatingMapId);
    }

    [Fact]
    public async Task GetPagedAsync_ShouldFilterByStartDate()
    {
        // Arrange
        var (_, _, EventVenueId, SeatingMapId) = await SeedDependenciesAsync();
        var targetStart = DateTimeOffset.UtcNow.AddDays(5);
        var oldStart = DateTimeOffset.UtcNow.AddDays(-5);

        await SeedAsync(
            TestEntityFactory.CreateEventVenueCalendar(Guid.NewGuid(), EventVenueId, SeatingMapId, startLocal: targetStart),
            TestEntityFactory.CreateEventVenueCalendar(Guid.NewGuid(), EventVenueId, SeatingMapId, startLocal: oldStart)
        );

        var criteria = new EventVenueCalendarQueryCriteria { StartDate = DateTime.UtcNow };

        // Act
        var result = await _repository.GetPagedAsync(criteria, TestContext.Current.CancellationToken);

        // Assert
        result.Items.Should().ContainSingle();
        result.Items[0].StartDate.Should().Be(targetStart.UtcDateTime);
    }

    [Fact]
    public async Task GetPagedAsync_ShouldFilterByEndDate()
    {
        // Arrange
        var (_, _, EventVenueId, SeatingMapId) = await SeedDependenciesAsync();
        var targetStart = DateTimeOffset.UtcNow.AddDays(5);
        var oldStart = DateTimeOffset.UtcNow.AddDays(-5);

        await SeedAsync(
            TestEntityFactory.CreateEventVenueCalendar(Guid.NewGuid(), EventVenueId, SeatingMapId, startLocal: targetStart),
            TestEntityFactory.CreateEventVenueCalendar(Guid.NewGuid(), EventVenueId, SeatingMapId, startLocal: oldStart)
        );

        var criteria = new EventVenueCalendarQueryCriteria { EndDate = DateTime.UtcNow };

        // Act
        var result = await _repository.GetPagedAsync(criteria, TestContext.Current.CancellationToken);

        // Assert
        result.Items.Should().ContainSingle();
        result.Items[0].StartDate.Should().Be(oldStart.UtcDateTime);
    }


    [Fact]
    public async Task GetPagedAsync_ShouldFilterByStatus()
    {
        // Arrange
        var (_, _, EventVenueId, SeatingMapId) = await SeedDependenciesAsync();
        var targetStart = DateTimeOffset.UtcNow.AddDays(5);
        var oldStart = DateTimeOffset.UtcNow.AddDays(-5);

        await SeedAsync(
            TestEntityFactory.CreateEventVenueCalendar(Guid.NewGuid(), EventVenueId, SeatingMapId, startLocal: targetStart, status: EventVenueCalendarStatus.Draft),
            TestEntityFactory.CreateEventVenueCalendar(Guid.NewGuid(), EventVenueId, SeatingMapId, startLocal: oldStart)
        );

        var criteria = new EventVenueCalendarQueryCriteria { Status = EventVenueCalendarStatus.Draft };

        // Act
        var result = await _repository.GetPagedAsync(criteria, TestContext.Current.CancellationToken);

        // Assert
        result.Items.Should().ContainSingle();
        result.Items[0].StartDate.Should().Be(targetStart.UtcDateTime);
    }

    [Fact]
    public async Task GetPagedAsync_ShouldFilterByTimeZoneId()
    {
        // Arrange
        var (_, _, EventVenueId, SeatingMapId) = await SeedDependenciesAsync();
        var targetStart = DateTimeOffset.UtcNow.AddDays(5);
        var oldStart = DateTimeOffset.UtcNow.AddDays(-5);

        await SeedAsync(
            TestEntityFactory.CreateEventVenueCalendar(Guid.NewGuid(), EventVenueId, SeatingMapId, startLocal: targetStart, timeZoneId: "America/New_York"),
            TestEntityFactory.CreateEventVenueCalendar(Guid.NewGuid(), EventVenueId, SeatingMapId, startLocal: oldStart)
        );

        var criteria = new EventVenueCalendarQueryCriteria { TimeZoneId = "America/New_York" };

        // Act
        var result = await _repository.GetPagedAsync(criteria, TestContext.Current.CancellationToken);

        // Assert
        result.Items.Should().ContainSingle();
        result.Items[0].StartDate.Should().Be(targetStart.UtcDateTime);
    }

    [Theory]
    [InlineData(EventVenueCalendarSortField.StartDate, SortDirection.Asc, "First")]
    [InlineData(EventVenueCalendarSortField.StartDate, SortDirection.Desc, "Second")]
    [InlineData(EventVenueCalendarSortField.Status, SortDirection.Asc, "First")]
    [InlineData(EventVenueCalendarSortField.Status, SortDirection.Desc, "Second")]
    [InlineData(EventVenueCalendarSortField.EndDate, SortDirection.Asc, "First")]
    [InlineData(EventVenueCalendarSortField.EndDate, SortDirection.Desc, "Second")]
    [InlineData(EventVenueCalendarSortField.TimeZoneId, SortDirection.Asc, "First")]
    [InlineData(EventVenueCalendarSortField.TimeZoneId, SortDirection.Desc, "Second")]
    [InlineData(null, SortDirection.Asc, "First")]
    [InlineData(null, SortDirection.Desc, "Second")]
    public async Task GetPagedAsync_ShouldApplyCorrectSorting(
        EventVenueCalendarSortField? sortField,
        SortDirection direction,
        string expectedOrderMarker)
    {
        // Arrange
        var (_, _, EventVenueId, SeatingMapId) = await SeedDependenciesAsync();

        var earlyCalendar = TestEntityFactory.CreateEventVenueCalendar(
            Guid.NewGuid(), EventVenueId, SeatingMapId,
            startLocal: DateTimeOffset.UtcNow.AddDays(1), status: EventVenueCalendarStatus.Draft);

        var lateCalendar = TestEntityFactory.CreateEventVenueCalendar(
            Guid.NewGuid(), EventVenueId, SeatingMapId,
            startLocal: DateTimeOffset.UtcNow.AddDays(10), status: EventVenueCalendarStatus.Published);

        await SeedAsync(earlyCalendar, lateCalendar);

        var criteria = new EventVenueCalendarQueryCriteria { SortBy = sortField, SortDirection = direction };

        // Act
        var result = await _repository.GetPagedAsync(criteria, TestContext.Current.CancellationToken);

        // Assert
        var expectedId = expectedOrderMarker == "First" ? earlyCalendar.Id : lateCalendar.Id;
        result.Items[0].Id.Should().Be(expectedId);
    }

    // --- Helper Method ---
    private async Task<(Guid EventId, Guid VenueId, Guid EventVenueId, Guid SeatingMapId)> SeedDependenciesAsync()
    {
        var eventId = Guid.NewGuid();
        var venueId = Guid.NewGuid();
        var eventVenueId = Guid.NewGuid();
        var seatingMapId = Guid.NewGuid();

        await SeedAsync(
            TestEntityFactory.CreateEvent(eventId)
        );

        await SeedAsync(
             TestEntityFactory.CreateVenue(venueId)
        );

        await SeedAsync(
            TestEntityFactory.CreateEventVenue(eventVenueId, eventId, venueId)
        );

        await SeedAsync(
            TestEntityFactory.CreateSeatingMap(seatingMapId, venueId)
        );

        return (eventId, venueId, eventVenueId, seatingMapId);
    }
}