using EventHouse.Management.Application.Commands.Artists.SetGenreStatus;
using EventHouse.Management.Application.Common.Enums;
using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Domain.Enums;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using ArtistCategory = EventHouse.Management.Domain.Enums.ArtistCategory;

namespace EventHouse.Management.Application.Tests.Commands.Artists.SetGenreStatus;

public sealed class SetArtistGenreStatusTests
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

        var handler = new SetArtistGenreStatusCommandHandler(artistRepo);
        var cmd = new SetArtistGenreStatusCommand(
            artistId,
            genreId,
            ArtistGenreStatusDto.Active);

        // Act + Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(cmd, ct));
        
        await artistRepo.DidNotReceiveWithAnyArgs()
            .UpdateAsync(default!, ct);
    }

    [Fact]
    public async Task Handle_WhenArtistExists_ShouldSetGenreStatus_AndUpdateArtist()
    {
        // Arrange
        var artistRepo = Substitute.For<IArtistRepository>();
        var ct = new CancellationTokenSource().Token;
        var artistId = Guid.NewGuid();
        var genreId = Guid.NewGuid();
        var artist = new Artist(artistId, "I", ArtistCategory.Host);
        
        artist.AddGenre(genreId, ArtistGenreStatus.Inactive, false);
        
        artistRepo.GetTrackedByIdAsync(artistId, Arg.Any<CancellationToken>())
            .Returns(artist);
        
        var handler = new SetArtistGenreStatusCommandHandler(artistRepo);
        var cmd = new SetArtistGenreStatusCommand(
            artistId,
            genreId,
            ArtistGenreStatusDto.Active);
        
        // Act
        await handler.Handle(cmd, ct);
        
        // Assert
        var genre = artist.Genres.First(g => g.GenreId == genreId);
        Assert.Equal(ArtistGenreStatus.Active, genre.Status);
        
        await artistRepo.Received(1)
            .UpdateAsync(artist, ct);
    }
}
