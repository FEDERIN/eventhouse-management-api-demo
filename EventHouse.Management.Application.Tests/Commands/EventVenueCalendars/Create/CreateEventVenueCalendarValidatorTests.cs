using EventHouse.Management.Application.Commands.EventVenueCalendars.Create;
using EventHouse.Management.Application.Common.Enums;
using FluentValidation.TestHelper;

namespace EventHouse.Management.Application.Tests.Commands.EventVenueCalendars.Create;

public sealed class CreateEventVenueCalendarValidatorTests
{
    private readonly CreateEventVenueCalendarCommandValidator _validator = new();

    [Fact]
    public void Should_PassValidation_When_CommandIsValid()
    {
        // Arrange
        var command =  new CreateEventVenueCalendarCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTime.Now,
            DateTime.Now.AddHours(2),
            "America/New_York",
            EventVenueCalendarStatusDto.Draft
            );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_HaveError_When_EventVenueId_IsEmpty()
    {
        // Arrange
        var command = CreateValidCommand() with { EventVenueId = Guid.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.EventVenueId)
              .WithErrorMessage("EventVenue identifier is required.");
    }

    [Fact]
    public void Should_HaveError_When_SeatingMapId_IsEmpty()
    {
        // Arrange
        var command = CreateValidCommand() with { SeatingMapId = Guid.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.SeatingMapId)
              .WithErrorMessage("SeatingMap identifier is required.");
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Should_HaveError_When_TimeZoneId_IsEmpty(string? invalidTz)
    {
        var command = CreateValidCommand() with { TimeZoneId = invalidTz! };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.TimeZoneId)
              .WithErrorMessage("TimeZone identifier is required.");
    }

    [Fact]
    public void Should_HaveError_When_TimeZoneId_IsInvalidIana()
    {
        var command = CreateValidCommand() with { TimeZoneId = "Invalid/Zone_Name" };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.TimeZoneId)
              .WithErrorMessage("TimeZone must be a valid IANA identifier (e.g., 'Europe/Madrid').");
    }

    [Fact]
    public void Should_HaveError_When_EndDate_Is_Before_StartDate()
    {
        // Arrange
        var start = DateTimeOffset.UtcNow.AddDays(1);
        var command = CreateValidCommand() with
        {
            StartDate = start,
            EndDate = start.AddHours(-1)
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.EndDate)
              .WithErrorMessage("EndDate must be greater than or equal to StartDate.");
    }

    [Fact]
    public void Should_HaveError_When_Status_Is_Invalid_Enum_Value()
    {
        // Arrange
        var command = CreateValidCommand() with
        {
            Status = (EventVenueCalendarStatusDto)99
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Status)
              .WithErrorMessage("The provided status is not valid.");
    }

    private static CreateEventVenueCalendarCommand CreateValidCommand() =>
        new(
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTime.UtcNow.AddDays(1),
            DateTime.UtcNow.AddDays(1).AddHours(2),
            "America/New_York",
            EventVenueCalendarStatusDto.Draft
        );
}
