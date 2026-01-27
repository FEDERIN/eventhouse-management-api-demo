using EventHouse.Management.Application.Commands.Artists.SetGenreStatus;
using EventHouse.Management.Application.Common.Enums;
using FluentAssertions;


namespace EventHouse.Management.Application.Tests.Commands.Artists.SetGenreStatus;

public sealed class SetArtistGenreStatusValidatorTests
{
    private readonly SetArtistGenreStatusCommandValidator _validator = new();

    [Fact]
    public void Should_fail_when_artist_id_is_empty()
    {
        var command = new SetArtistGenreStatusCommand(
            ArtistId: Guid.Empty,
            GenreId: Guid.NewGuid(),
            Status: ArtistGenreStatusDto.Active
        );

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_fail_when_genre_id_is_empty()
    {
        var command = new SetArtistGenreStatusCommand(
            ArtistId: Guid.NewGuid(),
            GenreId: Guid.Empty,
            Status: ArtistGenreStatusDto.Active
        );

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_fail_when_status_is_invalid()
    {
        var command = new SetArtistGenreStatusCommand(
            ArtistId: Guid.NewGuid(),
            GenreId: Guid.NewGuid(),
            Status: unchecked((ArtistGenreStatusDto)999)
        );

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }
}
