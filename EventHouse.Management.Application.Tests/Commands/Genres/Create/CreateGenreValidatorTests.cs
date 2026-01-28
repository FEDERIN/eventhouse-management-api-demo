
using EventHouse.Management.Application.Commands.Genres.Create;
using FluentAssertions;

namespace EventHouse.Management.Application.Tests.Commands.Genres.Create;

public sealed class CreateGenreValidatorTests
{
    private readonly CreateGenreCommandValidator _validator = new();

    [Fact]
    public void Should_fail_when_name_is_empty()
    {
        var command = new CreateGenreCommand("");
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_fail_when_name_is_too_long()
    {
        var longName = new string('A', 201);
        var command = new CreateGenreCommand(longName);
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_pass_with_valid_name()
    {
        var command = new CreateGenreCommand("Rock");
        var result = _validator.Validate(command);
        result.IsValid.Should().BeTrue();
    }
}
