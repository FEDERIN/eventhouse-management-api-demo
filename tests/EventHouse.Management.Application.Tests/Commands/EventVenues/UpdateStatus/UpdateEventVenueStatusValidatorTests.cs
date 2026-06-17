using EventHouse.Management.Application.Commands.EventVenues.UpdateStatus;
using EventHouse.Management.Application.Common.Enums;

namespace EventHouse.Management.Application.Tests.Commands.EventVenues.UpdateStatus;

public sealed class UpdateEventVenueStatusValidatorTests : ValidatorTestBase<UpdateEventVenueStatusCommand>
{
    public UpdateEventVenueStatusValidatorTests() : base(new UpdateEventVenueStatusCommandValidator()) { }

    [Fact]
    public void Should_Pass_When_Command_Is_Valid()
    {
        var command = new UpdateEventVenueStatusCommand(
            Guid.NewGuid(),
            EventVenueStatusDto.Active);

        ShouldPassValidation(command);
    }

    [Fact]
    public void Should_HaveError_When_Id_Is_Empty()
    {
        var command = new UpdateEventVenueStatusCommand(
            Guid.Empty,
            EventVenueStatusDto.Active);

        ShouldHaveValidationError(command, x => x.Id, "Id must be a non-empty GUID.");
    }

    [Fact]
    public void Should_HaveError_When_Status_Is_Invalid()
    {
        var command = new UpdateEventVenueStatusCommand(
            Guid.NewGuid(),
            (EventVenueStatusDto)99);

        ShouldHaveValidationError(command, x => x.Status, "The provided status is not valid.");
    }
}