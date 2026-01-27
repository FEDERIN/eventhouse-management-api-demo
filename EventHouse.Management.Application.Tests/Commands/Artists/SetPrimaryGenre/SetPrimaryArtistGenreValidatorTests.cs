
using EventHouse.Management.Application.Commands.Artists.SetPrimaryGenre;
using FluentAssertions;

namespace EventHouse.Management.Application.Tests.Commands.Artists.SetPrimaryGenre;

public sealed class SetPrimaryArtistGenreValidatorTests
{
    private readonly SetPrimaryArtistGenreCommandValidator _validator = new();

    [Fact]
    public void Should_fail_when_artist_id_is_empty()
    {
        var command = new SetPrimaryArtistGenreCommand(
            ArtistId: Guid.Empty,
            GenreId: Guid.NewGuid()
        );

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_fail_when_genre_id_is_empty()
    {
        var command = new SetPrimaryArtistGenreCommand(
            ArtistId: Guid.NewGuid(),
            GenreId: Guid.Empty
        );

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }
}
