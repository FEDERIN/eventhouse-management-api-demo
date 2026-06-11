using EventHouse.Management.Infrastructure.Repositories;
using EventHouse.Management.Infrastructure.Tests.Extensions;
using EventHouse.Management.Infrastructure.Tests.Persistence;
using EventHouse.Management.Tests.Shared.Factories;
using FluentAssertions;

namespace EventHouse.Management.Infrastructure.Tests.Repositories;

public sealed class EventVenueCalendarRepositoryTests(SharedDatabaseFixture fixture) : BasePersistenceTest(fixture)
{
    private EventVenueCalendarRepository _repository = null!;

    public override async ValueTask InitializeAsync()
    {
        await base.InitializeAsync();

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
    public async Task SwapHeadlinerAsync_ShouldRollbackTransaction_WhenArtistNotFound()
    {
        // Arrange
        var calendar = TestEntityFactory.CreateEventVenueCalendar(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        var oldArtistId = Guid.NewGuid(); // ID inexistente
        var newArtistId = Guid.NewGuid(); // ID inexistente

        // Act
        var act = async () => await _repository.SwapHeadlinerAsync(
            calendar.Id,
            oldArtistId,
            newArtistId,
            CancellationToken.None
        );

        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}