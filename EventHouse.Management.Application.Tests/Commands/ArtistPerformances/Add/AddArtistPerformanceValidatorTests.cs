using EventHouse.Management.Application.Commands.ArtistPerformances.Add;

namespace EventHouse.Management.Application.Tests.Commands.ArtistPerformances.Add;

public sealed class AddArtistPerformanceValidatorTests : ValidatorTestBase<AddArtistPerformanceCommand>
{
    public AddArtistPerformanceValidatorTests() : base(new AddArtistPerformanceCommandValidator()){}

    [Fact]
    public void Should_HaveError_When_ArtistId_Is_Empty()
    {
        var command = CreateValidCommand() with { ArtistId = Guid.Empty };

        ShouldHaveValidationError(command, x => x.ArtistId, "ArtistId must be a non-empty GUID.");
    }

    [Fact]
    public void Should_HaveError_When_EventVenueCalendarId_Is_Empty()
    {
        var command = CreateValidCommand() with { EventVenueCalendarId = Guid.Empty };

        ShouldHaveValidationError(command, x => x.EventVenueCalendarId, "EventVenueCalendarId must be a non-empty GUID.");
    }

    [Fact]
    public void Should_HaveError_When_SetStart_IsAfter_SetEnd()
    {
        var start = DateTimeOffset.UtcNow.AddHours(2);
        var command = CreateValidCommand() with
        {
            SetStart = start,
            SetEnd = start.AddHours(-1)
        };

        ShouldHaveValidationError(command, x => x,
            "SetEnd must be greater than or equal to SetStart when both are specified, and both must be provided if either is.");
    }

    [Fact]
    public void Should_Pass_When_DataIsValid()
    {
        var command = CreateValidCommand();

        ShouldPassValidation(command);
    }

    [Fact]
    public void Should_Pass_When_Set_Times_Are_Null()
    {
        var command = CreateValidCommand() with { SetStart = null, SetEnd = null };

        ShouldPassValidation(command);
    }

    [Fact]
    public void Should_HaveError_When_Only_SetStart_Is_Provided()
    {
        var command = CreateValidCommand() with { SetStart = DateTimeOffset.UtcNow, SetEnd = null };

        ShouldHaveValidationError(command, x => x,
            "SetEnd must be greater than or equal to SetStart when both are specified, and both must be provided if either is.");
    }

    private static AddArtistPerformanceCommand CreateValidCommand() =>
        new(
            EventVenueCalendarId: Guid.NewGuid(),
            ArtistId: Guid.NewGuid(),
            IsHeadliner: true,
            SetStart: DateTimeOffset.UtcNow,
            SetEnd: DateTimeOffset.UtcNow.AddHours(1)
        );
}