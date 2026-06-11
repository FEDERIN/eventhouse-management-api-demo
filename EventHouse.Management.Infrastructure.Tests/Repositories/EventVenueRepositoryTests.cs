using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Domain.Enums;
using EventHouse.Management.Infrastructure.Repositories;
using EventHouse.Management.Infrastructure.Tests.Extensions;
using EventHouse.Management.Infrastructure.Tests.Persistence;

namespace EventHouse.Management.Infrastructure.Tests.Repositories;

public sealed class EventVenueRepositoryTests(SharedDatabaseFixture fixture) : BasePersistenceTest(fixture)
{
    private EventVenueRepository _repository = null!;

    public override async ValueTask InitializeAsync()
    {
        await base.InitializeAsync();

        _repository = new EventVenueRepository(Context);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowInvalidOperationException_WhenEntityIsDetached()
    {
        var eventVenue = new EventVenue(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            EventVenueStatus.Active);

        var act = async () => await _repository.UpdateAsync(eventVenue, TestContext.Current.CancellationToken);

        await act.ShouldThrowDetachedException();
    }
}
