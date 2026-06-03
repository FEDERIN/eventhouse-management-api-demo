using EventHouse.Management.Application.Commands.Artists.AddGenre;
using EventHouse.Management.Application.Common.Enums;

namespace EventHouse.Management.Application.Tests.Commands.Artists.AddGenre;

public sealed class AddArtistGenreValidatorTests : ValidatorTestBase<AddArtistGenreCommand>
{
    public AddArtistGenreValidatorTests(): base(new AddArtistGenreCommandValidator()) { }

    [Fact]
    public void Should_HaveError_When_ArtistId_Is_Empty()
    {
        var command = CreateValidCommand() with { ArtistId = Guid.Empty };

        ShouldHaveValidationError(command, x => x.ArtistId, "ArtistId must be a non-empty GUID.");
    }

    [Fact]
    public void Should_HaveError_When_GenreId_Is_Empty()
    {
        var command = CreateValidCommand() with { GenreId = Guid.Empty };

        ShouldHaveValidationError(command, x => x.GenreId, "GenreId must be a non-empty GUID.");
    }

    [Fact]
    public void Should_HaveError_When_Status_Is_Invalid()
    {
        var command = CreateValidCommand() with { Status = (ArtistGenreStatusDto)999 };

        ShouldHaveValidationError(command, x => x.Status, "Status must be a valid ArtistGenreStatus value.");
    }

    [Fact]
    public void Should_Pass_When_Command_Is_Valid()
    {
        var command = CreateValidCommand();

        ShouldPassValidation(command);
    }

    private static AddArtistGenreCommand CreateValidCommand() =>
        new(Guid.NewGuid(), Guid.NewGuid(), ArtistGenreStatusDto.Active, true);
}