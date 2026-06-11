using EventHouse.Management.Application.Common.Sorting;
using EventHouse.Management.Application.Queries.Genres.GetAll;
using EventHouse.Management.Infrastructure.Repositories;
using EventHouse.Management.Infrastructure.Tests.Persistence;
using EventHouse.Management.Tests.Shared.Factories;
using FluentAssertions;

namespace EventHouse.Management.Infrastructure.Tests.Repositories;

public class GenreRepositoryTests(SharedDatabaseFixture fixture) : BasePersistenceTest(fixture)
{
    private GenreRepository _repository = null!;

    public override async ValueTask InitializeAsync()
    {
        await base.InitializeAsync();

        _repository = new GenreRepository(Context);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowInvalidOperationException_WhenEntityIsDetached()
    {
        // Arrange
        var genre = TestEntityFactory.CreateGenre(name: "Rock");

        // Act
        var act = async () => await _repository.UpdateAsync(genre, TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("UpdateAsync requires a tracked entity. Use GetTrackedByIdAsync.");
    }
}