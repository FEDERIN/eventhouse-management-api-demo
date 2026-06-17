using EventHouse.Management.Application.Commands.EventVenues.Create;
using EventHouse.Management.Application.Common.Enums;

namespace EventHouse.Management.Application.Tests.Commands.EventVenues.Create;

public sealed class CreateEventVenueValidatorTests : ValidatorTestBase<CreateEventVenueCommand>
{
    public CreateEventVenueValidatorTests() : base(new CreateEventVenueCommandValidator()) { }

    [Fact]
    public void Should_HaveError_When_EventId_Is_Empty()
    {
        var command = new CreateEventVenueCommand(Guid.Empty, Guid.NewGuid(), EventVenueStatusDto.Active);

        ShouldHaveValidationError(command, x => x.EventId, "The EventId cannot be empty.");
    }

    [Fact]
    public void Should_HaveError_When_VenueId_Is_Empty()
    {
        var command = new CreateEventVenueCommand(Guid.NewGuid(), Guid.Empty, EventVenueStatusDto.Active);

        ShouldHaveValidationError(command, x => x.VenueId, "The VenueId cannot be empty.");
    }

    [Fact]
    public void Should_Pass_When_Command_Is_Valid()
    {
        var command = new CreateEventVenueCommand(Guid.NewGuid(), Guid.NewGuid(), EventVenueStatusDto.Active);

        ShouldPassValidation(command);
    }

    [Fact]
    public void Should_HaveError_When_Status_Is_Invalid()
    {
        var command = new CreateEventVenueCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            (EventVenueStatusDto)99);

        ShouldHaveValidationError(command, x => x.Status, "The provided status is not valid.");
    }
}