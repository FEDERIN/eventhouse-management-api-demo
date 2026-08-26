using EventHouse.Management.Application.Commands.Seating.Rows.Add;

namespace EventHouse.Management.Application.Tests.Commands.Seating.Rows.Add;

public sealed class AddSeatingRowCommandValidatorTests
    : ValidatorTestBase<AddSeatingRowCommand>
{
    public AddSeatingRowCommandValidatorTests()
        : base(new AddSeatingRowCommandValidator()) { }

    [Fact]
    public void Should_Pass_When_Command_Is_Valid()
    {
        ShouldPassValidation(CreateValidCommand());
    }

    [Fact]
    public void Should_HaveError_When_SeatingMapId_Is_Empty()
    {
        var command = CreateValidCommand() with
        {
            SeatingMapId = Guid.Empty
        };

        ShouldHaveValidationError(
            command,
            x => x.SeatingMapId,
            "SeatingMapId must be a non-empty GUID.");
    }

    [Fact]
    public void Should_HaveError_When_SeatingSectionId_Is_Empty()
    {
        var command = CreateValidCommand() with
        {
            SeatingSectionId = Guid.Empty
        };

        ShouldHaveValidationError(
            command,
            x => x.SeatingSectionId,
            "SeatingSectionId must be a non-empty GUID.");
    }

    [Fact]
    public void Should_HaveError_When_Number_Is_Not_Greater_Than_Zero()
    {
        var command = CreateValidCommand() with
        {
            Number = 0
        };

        ShouldHaveValidationError(
            command,
            x => x.Number,
            "Row number must be greater than zero.");
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Should_HaveError_When_Label_Is_Empty(string? invalidLabel)
    {
        var command = CreateValidCommand() with
        {
            Label = invalidLabel!
        };

        ShouldHaveValidationError(
            command,
            x => x.Label,
            "Row label is required.");
    }

    [Fact]
    public void Should_HaveError_When_Label_Exceeds_MaximumLength()
    {
        var command = CreateValidCommand() with
        {
            Label = new string('A', 201)
        };

        ShouldHaveValidationError(
            command,
            x => x.Label,
            "Row label cannot exceed 200 characters.");
    }

    private static AddSeatingRowCommand CreateValidCommand() =>
        new(
            Guid.NewGuid(),
            Guid.NewGuid(),
            1,
            "A");
}