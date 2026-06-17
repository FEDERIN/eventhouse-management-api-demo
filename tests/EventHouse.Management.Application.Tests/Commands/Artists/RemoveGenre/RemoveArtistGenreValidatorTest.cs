using EventHouse.Management.Application.Commands.Artists.RemoveGenre;

namespace EventHouse.Management.Application.Tests.Commands.Artists.RemoveGenre;

public sealed class RemoveArtistGenreValidatorTest : ValidatorTestBase<RemoveArtistGenreCommand>
{
    public RemoveArtistGenreValidatorTest() : base(new RemoveArtistGenreCommandValidator()) { }

    [Fact]
    public void Should_HaveError_When_ArtistId_Is_Empty()
    {
        var command = new RemoveArtistGenreCommand(
            ArtistId: Guid.Empty,
            GenreId: Guid.NewGuid()
        );

        ShouldHaveValidationError(command, x => x.ArtistId, "ArtistId must be a non-empty GUID.");
    }

    [Fact]
    public void Should_HaveError_When_GenreId_Is_Empty()
    {
        var command = new RemoveArtistGenreCommand(
            ArtistId: Guid.NewGuid(),
            GenreId: Guid.Empty
        );

        ShouldHaveValidationError(command, x => x.GenreId, "GenreId must be a non-empty GUID.");
    }
}