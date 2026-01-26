using EventHouse.Management.Application.Commands.Artists.AddGenre;
using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Domain.Enums;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace EventHouse.Management.Application.Tests.Artists;

public sealed class AddArtistGenreTests
{
    [Fact]
    public async Task Handle_WhenArtistDoesNotExist_ShouldThrowNotFoundException_AndNotCallOtherDependencies()
    {
        // Arrange
        var artistRepo = Substitute.For<IArtistRepository>();
        var genreRepo = Substitute.For<IGenreRepository>();
        var ct = new CancellationTokenSource().Token;

        var artistId = Guid.NewGuid();
        var genreId = Guid.NewGuid();

        artistRepo.GetTrackedByIdAsync(artistId, Arg.Any<CancellationToken>())
            .ReturnsNull();

        var handler = new AddArtistGenreCommandHandler(artistRepo, genreRepo);
        var cmd = new AddArtistGenreCommand(
            artistId,
            genreId,
            Common.Enums.ArtistGenreStatus.Active,
            true);

        // Act + Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(cmd, ct));

        await genreRepo.DidNotReceiveWithAnyArgs()
            .GetByIdAsync(default, ct);

        await artistRepo.DidNotReceiveWithAnyArgs()
            .UpdateAsync(default!, ct);
    }

    [Fact]
    public async Task Handle_WhenGenreDoesNotExistInCatalog_ShouldThrowNotFoundException_AndNotUpdateArtist()
    {
        // Arrange
        var artistRepo = Substitute.For<IArtistRepository>();
        var genreRepo = Substitute.For<IGenreRepository>();
        var ct = new CancellationTokenSource().Token;

        var artistId = Guid.NewGuid();
        var genreId = Guid.NewGuid();

        var artist = new Artist(artistId, "E", ArtistCategory.Band);

        artistRepo.GetTrackedByIdAsync(artistId, Arg.Any<CancellationToken>())
            .Returns(artist);

        genreRepo.GetByIdAsync(genreId, Arg.Any<CancellationToken>())
            .ReturnsNull();

        var handler = new AddArtistGenreCommandHandler(artistRepo, genreRepo);
        var cmd = new AddArtistGenreCommand(
            artistId,
            genreId,
            Common.Enums.ArtistGenreStatus.Active,
            true);

        // Act + Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(cmd, ct));

        await genreRepo.Received(1)
            .GetByIdAsync(genreId, Arg.Any<CancellationToken>());

        await artistRepo.DidNotReceiveWithAnyArgs()
            .UpdateAsync(default!, ct);
    }

    [Fact]
    public async Task Handle_WhenGenreAlreadyAssociated_ShouldReturnWithoutFetchingGenre_AndWithoutUpdatingArtist()
    {
        // Arrange
        var artistRepo = Substitute.For<IArtistRepository>();
        var genreRepo = Substitute.For<IGenreRepository>();
        var ct = new CancellationTokenSource().Token;

        var artistId = Guid.NewGuid();
        var genreId = Guid.NewGuid();

        var artist = new Artist(artistId, "F", ArtistCategory.Dancer);

        artist.AddGenre(genreId, ArtistGenreStatus.Active, isPrimary: false);

        artistRepo.GetTrackedByIdAsync(artistId, Arg.Any<CancellationToken>())
            .Returns(artist);

        var handler = new AddArtistGenreCommandHandler(artistRepo, genreRepo);
        var cmd = new AddArtistGenreCommand(
            artistId,
            genreId,
            Common.Enums.ArtistGenreStatus.Active,
            false);

        // Act
        await handler.Handle(cmd, ct);

        await genreRepo.DidNotReceiveWithAnyArgs()
            .GetByIdAsync(default, ct);

        await artistRepo.DidNotReceiveWithAnyArgs()
            .UpdateAsync(default!, ct);
    }

    [Fact]
    public async Task Handle_WhenGenreIsNewAndExistsInCatalog_ShouldUpdateArtist()
    {
        // Arrange
        var artistRepo = Substitute.For<IArtistRepository>();
        var genreRepo = Substitute.For<IGenreRepository>();
        var ct = new CancellationTokenSource().Token;

        var artistId = Guid.NewGuid();
        var genreId = Guid.NewGuid();

        var artist = new Artist(artistId, "G", ArtistCategory.Band);
        var genre = new Genre(genreId, "Jazz");

        artistRepo.GetTrackedByIdAsync(artistId, Arg.Any<CancellationToken>())
            .Returns(artist);

        genreRepo.GetByIdAsync(genreId, Arg.Any<CancellationToken>())
            .Returns(genre);

        var handler = new AddArtistGenreCommandHandler(artistRepo, genreRepo);
        var cmd = new AddArtistGenreCommand(
            artistId,
            genreId,
            Common.Enums.ArtistGenreStatus.Active,
            true);

        // Act
        await handler.Handle(cmd, ct);

        // Assert
        await genreRepo.Received(1)
            .GetByIdAsync(genreId, Arg.Any<CancellationToken>());

        await artistRepo.Received(1)
            .UpdateAsync(artist, Arg.Any<CancellationToken>());
    }
}
