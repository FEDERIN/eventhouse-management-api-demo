using EventHouse.Management.Application.Commands.Artists.SetPrimaryGenre;

namespace EventHouse.Management.Application.Tests.Commands.Artists.SetPrimaryGenre;

public sealed class SetPrimaryArtistGenreValidatorTests : ValidatorTestBase<SetPrimaryArtistGenreCommand>
{
    public SetPrimaryArtistGenreValidatorTests() : base(new SetPrimaryArtistGenreCommandValidator()) { }

    [Fact]
    public void Should_HaveError_When_ArtistId_Is_Empty()
    {
        var command = new SetPrimaryArtistGenreCommand(Guid.Empty, Guid.NewGuid());

        ShouldHaveValidationError(command, x => x.ArtistId, "ArtistId must be a non-empty GUID.");
    }

    [Fact]
    public void Should_HaveError_When_GenreId_Is_Empty()
    {
        var command = new SetPrimaryArtistGenreCommand(Guid.NewGuid(), Guid.Empty);

        ShouldHaveValidationError(command, x => x.GenreId, "GenreId must be a non-empty GUID.");
    }

    [Fact]
    public void Should_Pass_When_Command_Is_Valid()
    {
        var command = new SetPrimaryArtistGenreCommand(Guid.NewGuid(), Guid.NewGuid());

        ShouldPassValidation(command);
    }
}