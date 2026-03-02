using EventHouse.Management.Application.Commands.Events.Update;
using FluentAssertions;

namespace EventHouse.Management.Application.Tests.Commands.Events.Update;

public sealed class UpdateEventValidatorTests
{
    private readonly UpdateEventCommandValidator _validator = new();

    [Fact]
    public void Should_fail_when_id_is_empty()
    {
        var command = new UpdateEventCommand(
            Guid.Empty,
            "Test Event",
            "This is a test event.",
            Common.Enums.EventScopeDto.Local
        );
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_fail_when_name_is_empty()
    {
        var command = new UpdateEventCommand(
            Guid.NewGuid(),
            "",
            "This is a test event.",
            Common.Enums.EventScopeDto.Local
        );
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_fail_when_scope_is_invalid()
    {
        var command = new UpdateEventCommand(
            Guid.NewGuid(),
            "Test Event",
            "This is a test event.",
            unchecked((Common.Enums.EventScopeDto)999)
        );
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_pass_with_valid_command()
    {
        var command = new UpdateEventCommand(
            Guid.NewGuid(),
            "Test Event",
            "This is a test event.",
            Common.Enums.EventScopeDto.International
        );
        var result = _validator.Validate(command);
        result.IsValid.Should().BeTrue();
    }
}
