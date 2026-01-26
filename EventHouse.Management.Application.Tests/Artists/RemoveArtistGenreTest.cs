
using EventHouse.Management.Application.Commands.Artists.RemoveGenre;
using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using EventHouse.Management.Domain.Entities;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace EventHouse.Management.Application.Tests.Artists;

    public sealed class RemoveArtistGenreTest
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

            var handler = new RemoveArtistGenreCommandHandler(artistRepo);
            var cmd = new RemoveArtistGenreCommand(
                artistId,
                genreId);

            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(cmd, ct));

            await genreRepo.DidNotReceiveWithAnyArgs()
                .GetByIdAsync(default, ct);

            await artistRepo.DidNotReceiveWithAnyArgs()
                .UpdateAsync(default!, ct);
        }

    [Fact]
    public async Task Handle_WhenArtistExists_ShouldRemoveGenre_AndUpdateArtist()
    {
        // Arrange
        var artistRepo = Substitute.For<IArtistRepository>();
        var ct = new CancellationTokenSource().Token;
        var artistId = Guid.NewGuid();
        var genreId = Guid.NewGuid();
        var artist = new Artist(artistId, "H", Domain.Enums.ArtistCategory.Band);

        artist.AddGenre(genreId, Domain.Enums.ArtistGenreStatus.Active, true);
        
        artistRepo.GetTrackedByIdAsync(artistId, Arg.Any<CancellationToken>())
            .Returns(artist);
        
        var handler = new RemoveArtistGenreCommandHandler(artistRepo);
        var cmd = new RemoveArtistGenreCommand(
            artistId,
            genreId);
        
        // Act
        await handler.Handle(cmd, ct);
        // Assert
        await artistRepo.Received(1)
            .UpdateAsync(artist, ct);
    }
}
