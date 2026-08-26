using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Infrastructure.Repositories;
using EventHouse.Management.Infrastructure.Tests.Persistence;
using FluentAssertions;

namespace EventHouse.Management.Infrastructure.Tests.Repositories;

public class SeatingMapRepositoryTests(SharedDatabaseFixture fixture) : BasePersistenceTest(fixture)
{
    private SeatingMapRepository _repository = null!;

    public override async ValueTask InitializeAsync()
    {
        await base.InitializeAsync();

        _repository = new SeatingMapRepository(Context);
    }


    [Fact]
    public async Task UpdateAsync_ShouldThrowInvalidOperationException_WhenEntityIsDetached()
    {
        // Arrange
        var seatingMap = new SeatingMap(Guid.NewGuid(), Guid.NewGuid(), "Central", 1); 
        
        // Act
        var act = async () => await _repository.UpdateAsync(seatingMap, TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("UpdateAsync requires a tracked entity. Use GetTrackedByIdAsync.");
    }
}
