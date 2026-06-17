using EventHouse.Management.Domain.Enums;
using EventHouse.Management.Infrastructure.Repositories;
using EventHouse.Management.Infrastructure.Tests.Extensions;
using EventHouse.Management.Infrastructure.Tests.Persistence;
using EventHouse.Management.Tests.Shared.Factories;
using FluentAssertions;

namespace EventHouse.Management.Infrastructure.Tests.Repositories;

public class ArtistRepositoryTests(SharedDatabaseFixture fixture) : BasePersistenceTest(fixture)
{
    private ArtistRepository _repository = null!;

    public override async ValueTask InitializeAsync()
    {
        await base.InitializeAsync();

        _repository = new ArtistRepository(Context);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowInvalidOperationException_WhenEntityIsDetached()
    {
        var entity = TestEntityFactory.CreateArtist("Bad Bunny", ArtistCategory.Singer);

        var act = async () => await _repository.UpdateAsync(entity, TestContext.Current.CancellationToken);

        await act.ShouldThrowDetachedException();
    }

    [Fact]
    public async Task SetPrimaryGenreAsync_ShouldRollbackTransaction_WhenOperationFails()
    {
        // Arrange
        var artist = TestEntityFactory.CreateArtist("Bad Bunny", ArtistCategory.Singer);
        var genre1 = TestEntityFactory.CreateGenre(name: "Rock");
        var genre2 = TestEntityFactory.CreateGenre(name: "Pop");

        artist.AddGenre(genre1.Id, ArtistGenreStatus.Active, true);
        artist.AddGenre(genre2.Id, ArtistGenreStatus.Active, false);

        Context.Genres.Add(genre1);
        Context.Genres.Add(genre2);
        Context.Artists.Add(artist);

        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var act = async () => await _repository.SetPrimaryGenreAsync(
            artist.Id,
            genre1.Id,
            Guid.Empty,
            CancellationToken.None
        );

        // Assert
        await act.Should().ThrowAsync<Exception>();

        var artistReloaded = await _repository.GetTrackedByIdAsync(artist.Id, TestContext.Current.CancellationToken);

        artistReloaded.Should().NotBeNull();
        artistReloaded.Genres.Should().NotContain(g => g.GenreId == genre2.Id && g.IsPrimary == true);
    }
}
