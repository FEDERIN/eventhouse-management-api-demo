using EventHouse.Management.Application.Commands.Artists.AddGenre;
using EventHouse.Management.Application.Common.Enums;
using FluentAssertions;

namespace EventHouse.Management.Application.Tests.Commands.Artists.AddGenre;

public class AddArtistGenreValidatorTests
{
    private readonly AddArtistGenreCommandValidator _validator = new();

    [Fact]
    public void Should_fail_when_artist_id_is_empty()
    {
        var command = new AddArtistGenreCommand(
            ArtistId: Guid.Empty,
            GenreId: Guid.NewGuid(),
            Status: ArtistGenreStatusDto.Active,
            IsPrimary: true
        );

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_fail_when_genre_id_is_empty()
    {
        var command = new AddArtistGenreCommand(
            ArtistId: Guid.NewGuid(),
            GenreId: Guid.Empty,
            Status: ArtistGenreStatusDto.Active,
            IsPrimary: true
        );

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_fail_when_status_is_invalid()
    {
        var command = new AddArtistGenreCommand(
            ArtistId: Guid.NewGuid(),
            GenreId: Guid.NewGuid(),
            Status: unchecked((ArtistGenreStatusDto)999),
            IsPrimary: false
        );

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }
}
