using EventHouse.Management.Application.Commands.EventVenueCalendars.Update;
using EventHouse.Management.Application.Common.Enums;
using FluentValidation.TestHelper;

namespace EventHouse.Management.Application.Tests.Commands.EventVenueCalendars.Update;

public sealed class UpdateEventVenueCalendarValidatorTests
{
    private readonly UpdateEventVenueCalendarCommandValidator _validator = new();

    [Fact]
    public void Should_PassValidation_When_CommandIsValid()
    {
        var command = CreateValidCommand();

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_HaveError_When_Id_IsEmpty()
    {
        // Uso de 'with' para modificar solo la propiedad que queremos probar
        var command = CreateValidCommand() with { Id = Guid.Empty };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Id)
              .WithErrorMessage("The calendar event identifier must not be empty.");
    }

    [Fact]
    public void Should_HaveError_When_EndDate_IsBefore_StartDate()
    {
        var start = DateTimeOffset.UtcNow.AddDays(1);
        var command = CreateValidCommand() with
        {
            StartDate = start,
            EndDate = start.AddHours(-1)
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.EndDate)
              .WithErrorMessage("EndDate must be greater than or equal to StartDate.");
    }

    [Fact]
    public void Should_HaveError_When_Status_IsInvalid()
    {
        var command = CreateValidCommand() with { Status = (EventVenueCalendarStatusDto)99 };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Status)
              .WithErrorMessage("The provided status is not valid.");
    }

    private static UpdateEventVenueCalendarCommand CreateValidCommand() =>
        new(
            Id: Guid.NewGuid(),
            StartDate: DateTimeOffset.UtcNow.AddDays(1),
            EndDate: DateTimeOffset.UtcNow.AddDays(1).AddHours(2),
            Status: EventVenueCalendarStatusDto.Draft
        );
}