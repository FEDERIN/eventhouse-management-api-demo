using EventHouse.Management.Application.Commands.Events.Update;
using EventHouse.Management.Application.Common.Enums;

namespace EventHouse.Management.Application.Tests.Commands.Events.Update;

public sealed class UpdateEventValidatorTests : ValidatorTestBase<UpdateEventCommand>
{
    public UpdateEventValidatorTests() : base(new UpdateEventCommandValidator()) { }

    [Fact]
    public void Should_HaveError_When_Id_Is_Empty()
    {
        var command = new UpdateEventCommand(Guid.Empty, "Test Event", "This is a test event.", EventScopeDto.Local);

        ShouldHaveValidationError(command, x => x.Id, "Id must be a non-empty GUID.");
    }

    [Fact]
    public void Should_HaveError_When_Name_Is_Empty()
    {
        var command = new UpdateEventCommand(Guid.NewGuid(), "", "This is a test event.", EventScopeDto.Local);

        ShouldHaveValidationError(command, x => x.Name, "Name is required.");
    }

    [Fact]
    public void Should_HaveError_When_Scope_Is_Invalid()
    {
        var command = new UpdateEventCommand(Guid.NewGuid(), "Test Event", "This is a test event.", unchecked((EventScopeDto)999));

        ShouldHaveValidationError(command, x => x.Scope, "The provided scope is not valid.");
    }

    [Fact]
    public void Should_Pass_When_Data_Is_Valid()
    {
        var command = new UpdateEventCommand(Guid.NewGuid(), "Test Event", "This is a test event.", EventScopeDto.International);

        ShouldPassValidation(command);
    }
}