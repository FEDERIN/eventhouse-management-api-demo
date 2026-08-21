using EventHouse.Management.Application.Commands.EventVenueCalendars.Create;

namespace EventHouse.Management.Application.Tests.Commands.EventVenueCalendars.Create;

public sealed class CreateEventVenueCalendarValidatorTests : ValidatorTestBase<CreateEventVenueCalendarCommand>
{
    public CreateEventVenueCalendarValidatorTests() : base(new CreateEventVenueCalendarCommandValidator()) { }

    [Fact]
    public void Should_Pass_When_Command_Is_Valid()
    {
        ShouldPassValidation(CreateValidCommand());
    }

    [Fact]
    public void Should_HaveError_When_EventVenueId_Is_Empty()
    {
        var command = CreateValidCommand() with { EventVenueId = Guid.Empty };

        ShouldHaveValidationError(command, x => x.EventVenueId, "EventVenue identifier is required.");
    }

    [Fact]
    public void Should_HaveError_When_SeatingMapId_Is_Empty()
    {
        var command = CreateValidCommand() with { SeatingMapId = Guid.Empty };

        ShouldHaveValidationError(command, x => x.SeatingMapId, "SeatingMap identifier is required.");
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Should_HaveError_When_TimeZoneId_Is_Empty(string? invalidTz)
    {
        var command = CreateValidCommand() with { TimeZoneId = invalidTz! };

        ShouldHaveValidationError(command, x => x.TimeZoneId, "TimeZone identifier is required.");
    }

    [Fact]
    public void Should_HaveError_When_TimeZoneId_Is_Invalid_Iana()
    {
        var command = CreateValidCommand() with { TimeZoneId = "Invalid/Zone_Name" };

        ShouldHaveValidationError(command, x => x.TimeZoneId, "TimeZone must be a valid IANA identifier (e.g., 'Europe/Madrid').");
    }

    [Fact]
    public void Should_HaveError_When_EndDate_Is_Before_StartDate()
    {
        var start = DateTimeOffset.UtcNow.AddDays(1);
        var command = CreateValidCommand() with
        {
            StartDate = start,
            EndDate = start.AddHours(-1)
        };

        ShouldHaveValidationError(command, x => x.EndDate, "EndDate must be greater than or equal to StartDate.");
    }

    private static CreateEventVenueCalendarCommand CreateValidCommand() =>
        new(
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTime.UtcNow.AddDays(1),
            DateTime.UtcNow.AddDays(1).AddHours(2),
            "America/New_York"
        );
}