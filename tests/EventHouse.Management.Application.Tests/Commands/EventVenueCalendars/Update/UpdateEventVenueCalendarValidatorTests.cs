using EventHouse.Management.Application.Commands.EventVenueCalendars.Update;
using EventHouse.Management.Application.Common.Enums;

namespace EventHouse.Management.Application.Tests.Commands.EventVenueCalendars.Update;

public sealed class UpdateEventVenueCalendarValidatorTests : ValidatorTestBase<UpdateEventVenueCalendarCommand>
{
    public UpdateEventVenueCalendarValidatorTests() : base(new UpdateEventVenueCalendarCommandValidator()) { }

    [Fact]
    public void Should_Pass_When_Command_Is_Valid()
    {
        ShouldPassValidation(CreateValidCommand());
    }

    [Fact]
    public void Should_HaveError_When_Id_Is_Empty()
    {
        var command = CreateValidCommand() with { Id = Guid.Empty };

        ShouldHaveValidationError(command, x => x.Id, "The calendar event identifier must not be empty.");
    }

    [Fact]
    public void Should_HaveError_When_End_Date_Is_Before_Start_Date()
    {
        var start = DateTimeOffset.UtcNow.AddDays(1);
        var command = CreateValidCommand() with
        {
            StartDate = start,
            EndDate = start.AddHours(-1)
        };

        ShouldHaveValidationError(command, x => x.EndDate, "EndDate must be greater than or equal to StartDate.");
    }

    [Fact]
    public void Should_HaveError_When_Status_Is_Invalid()
    {
        var command = CreateValidCommand() with { Status = (EventVenueCalendarStatusDto)99 };

        ShouldHaveValidationError(command, x => x.Status, "The provided status is not valid.");
    }

    private static UpdateEventVenueCalendarCommand CreateValidCommand() =>
        new(
            Id: Guid.NewGuid(),
            StartDate: DateTimeOffset.UtcNow.AddDays(1),
            EndDate: DateTimeOffset.UtcNow.AddDays(1).AddHours(2),
            Status: EventVenueCalendarStatusDto.Draft
        );
}