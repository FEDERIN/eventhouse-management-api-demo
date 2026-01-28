

using EventHouse.Management.Application.Commands.Genres.Update;
using FluentAssertions;

namespace EventHouse.Management.Application.Tests.Commands.Genres.Update;

public sealed class UpdateGenreValidatorTests
{
    private readonly UpdateGenreCommandValidator _validator = new();

    [Fact]
    public void Should_fail_when_id_is_empty()
    {
        var command = new UpdateGenreCommand(
            Guid.Empty,
            "Rock"
        );
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_fail_when_name_is_empty()
    {
        var command = new UpdateGenreCommand(
            Guid.NewGuid(),
            ""
        );
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_pass_with_valid_command()
    {
        var command = new UpdateGenreCommand(
            Guid.NewGuid(),
            "Jazz"
        );
        var result = _validator.Validate(command);
        result.IsValid.Should().BeTrue();
    }
}
