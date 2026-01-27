using EventHouse.Management.Application.Commands.Artists.SetPrimaryGenre;
using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Domain.Enums;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace EventHouse.Management.Application.Tests.Commands.Artists.SetPrimaryGenre;

public sealed class SetPrimaryArtistGenreTests
{
    [Fact]
    public async Task Handle_WhenArtistDoesNotExist_ShouldThrowNotFoundException_AndNotCallOtherDependencies()
    {
        // Arrange
        var artistRepo = Substitute.For<IArtistRepository>();
        var ct = new CancellationTokenSource().Token;
        var artistId = Guid.NewGuid();
        var genreId = Guid.NewGuid();

        artistRepo.GetTrackedByIdAsync(artistId, Arg.Any<CancellationToken>())
            .ReturnsNull();

        var handler = new SetPrimaryArtistGenreCommandHandler(artistRepo);
        var cmd = new SetPrimaryArtistGenreCommand(
            artistId,
            genreId);

        // Act + Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(cmd, ct));
        
        await artistRepo.DidNotReceiveWithAnyArgs()
            .UpdateAsync(default!, ct);
    }

    [Fact]
    public async Task Handle_WhenArtistExists_ShouldSetPrimaryGenre_AndUpdateArtist()
    {
        // Arrange
        var artistRepo = Substitute.For<IArtistRepository>();
        var ct = new CancellationTokenSource().Token;
        var artistId = Guid.NewGuid();
        var genreId = Guid.NewGuid();
        var genreId2 = Guid.NewGuid();

        var artist = new Artist(artistId, "J", ArtistCategory.Band);
        
        artist.AddGenre(genreId, ArtistGenreStatus.Active, false);
        artist.AddGenre(genreId2, ArtistGenreStatus.Active, true);

        artistRepo.GetTrackedByIdAsync(artistId, Arg.Any<CancellationToken>())
            .Returns(artist);
        
        var handler = new SetPrimaryArtistGenreCommandHandler(artistRepo);
        var cmd = new SetPrimaryArtistGenreCommand(
            artistId,
            genreId);

        // Act
        await handler.Handle(cmd, ct);

        // Assert
        var artistGenre = artist.Genres.First(g => g.GenreId == genreId);
        
        Assert.True(artistGenre.IsPrimary);
        
        await artistRepo.Received(1)
            .SetPrimaryGenreAsync(artistId, genreId2, genreId, ct);
    }

    [Fact]
    public async Task Handle_WhenGenreIsAlreadyPrimary_ShouldNotUpdateArtist()
    {
        // Arrange
        var artistRepo = Substitute.For<IArtistRepository>();
        var ct = new CancellationTokenSource().Token;
        var artistId = Guid.NewGuid();
        var genreId = Guid.NewGuid();
        
        var artist = new Artist(artistId, "K", ArtistCategory.Singer);
        
        artist.AddGenre(genreId, ArtistGenreStatus.Active, true);
        
        artistRepo.GetTrackedByIdAsync(artistId, Arg.Any<CancellationToken>())
            .Returns(artist);
        
        var handler = new SetPrimaryArtistGenreCommandHandler(artistRepo);
        var cmd = new SetPrimaryArtistGenreCommand(
            artistId,
            genreId);

        // Act
        await handler.Handle(cmd, ct);

        // Assert
        await artistRepo.DidNotReceiveWithAnyArgs()
            .SetPrimaryGenreAsync(default, default, default, ct);
    }
}
