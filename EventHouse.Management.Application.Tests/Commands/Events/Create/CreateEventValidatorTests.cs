
using EventHouse.Management.Application.Commands.Events.Create;
using EventHouse.Management.Application.Common.Enums;
using FluentAssertions;

namespace EventHouse.Management.Application.Tests.Commands.Events.Create;

public sealed class CreateEventValidatorTests
{

    private readonly CreateEventCommandValidator _validator = new();

    [Fact]
    public void Should_fail_when_name_is_empty()
    {
        var command = new CreateEventCommand(
            "", "Some description", EventScopeDto.Local
        );

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_fail_when_name_is_too_long()
    {
        var longName = new string('A', 201); // Assuming max length is 200
        var command = new CreateEventCommand(
            longName, "Some description", EventScopeDto.National
        );
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_fail_when_description_is_too_long()
    {
        var longDescription = new string('B', 1001); // Assuming max length is 1000
        var command = new CreateEventCommand(
            "Test Event", longDescription, EventScopeDto.International
        );
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_fail_when_scope_is_invalid()
    {
        var command = new CreateEventCommand(
            "Test Event", "Some description", unchecked((EventScopeDto)999)
        );
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_pass_with_valid_data()
    {
        var command = new CreateEventCommand(
            "Valid Event", "A valid event description", EventScopeDto.International
        );
        var result = _validator.Validate(command);
        result.IsValid.Should().BeTrue();
    }
}
