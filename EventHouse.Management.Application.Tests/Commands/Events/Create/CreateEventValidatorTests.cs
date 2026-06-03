using EventHouse.Management.Application.Commands.Events.Create;
using EventHouse.Management.Application.Common.Enums;

namespace EventHouse.Management.Application.Tests.Commands.Events.Create;

public sealed class CreateEventValidatorTests : ValidatorTestBase<CreateEventCommand>
{
    public CreateEventValidatorTests() : base(new CreateEventCommandValidator()) { }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Should_HaveError_When_Name_Is_Empty_Or_Whitespace(string invalidName)
    {
        var command = new CreateEventCommand(invalidName, "Desc", EventScopeDto.Local);

        ShouldHaveValidationError(command, x => x.Name, "Name is required.");
    }

    [Fact]
    public void Should_HaveError_When_Name_Is_Too_Long()
    {
        var command = new CreateEventCommand(new string('A', 201), "Desc", EventScopeDto.Local);

        ShouldHaveValidationError(command, x => x.Name);
    }

    [Fact]
    public void Should_HaveError_When_Description_Is_Too_Long()
    {
        var command = new CreateEventCommand("Valid Name", new string('B', 1001), EventScopeDto.Local);

        ShouldHaveValidationError(command, x => x.Description);
    }

    [Fact]
    public void Should_HaveError_When_Scope_Is_Invalid()
    {
        var command = new CreateEventCommand("Valid Name", "Valid description", unchecked((EventScopeDto)999));

        ShouldHaveValidationError(command, x => x.Scope);
    }

    [Fact]
    public void Should_Pass_When_Data_Is_Valid()
    {
        var command = new CreateEventCommand("Valid Event", "Valid description", EventScopeDto.International);

        ShouldPassValidation(command);
    }
}