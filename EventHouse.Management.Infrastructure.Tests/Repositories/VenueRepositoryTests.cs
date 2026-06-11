using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Infrastructure.Repositories;
using EventHouse.Management.Infrastructure.Tests.Extensions;
using EventHouse.Management.Infrastructure.Tests.Persistence;

namespace EventHouse.Management.Infrastructure.Tests.Repositories;

public sealed class VenueRepositoryTests(SharedDatabaseFixture fixture) : BasePersistenceTest(fixture)
{
    private VenueRepository _repository = null!;

    public override async ValueTask InitializeAsync()
    {
        await base.InitializeAsync();

        _repository = new VenueRepository(Context);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowInvalidOperationException_WhenEntityIsDetached()
    {
        // Arrange
        var venue = new Venue(
            Guid.NewGuid(),
            $"{"Madison Square Garden"} {Guid.NewGuid().ToString()[..4]}",
            "4 Pennsylvania Plaza",
            "New York",
            "NY",
            "US",
            40.7505m,
            -73.9934m,
            "Eastern Standard Time",
            2000,
            true);

        //Act
        var act = async () => await _repository.UpdateAsync(venue, TestContext.Current.CancellationToken);

        // Assert
        await act.ShouldThrowDetachedException();
    }
}