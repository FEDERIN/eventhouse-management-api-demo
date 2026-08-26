using EventHouse.Management.Application.Commands.Seating.Sections.Add;

namespace EventHouse.Management.Application.Tests.Commands.Seating.Sections.Add;

public sealed class AddSeatingSectionCommandValidatorTests
    : ValidatorTestBase<AddSeatingSectionCommand>
{
    public AddSeatingSectionCommandValidatorTests()
        : base(new AddSeatingSectionCommandValidator()) { }

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

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Should_HaveError_When_Name_Is_Empty(string? invalidName)
    {
        var command = CreateValidCommand() with
        {
            Name = invalidName!
        };

        ShouldHaveValidationError(
            command,
            x => x.Name,
            "Name is required.");
    }

    [Fact]
    public void Should_HaveError_When_Name_Contains_Only_Whitespace()
    {
        var command = CreateValidCommand() with
        {
            Name = "   "
        };

        ShouldHaveValidationError(
            command,
            x => x.Name,
            "Name is required.");
    }

    [Fact]
    public void Should_HaveError_When_Name_Exceeds_MaximumLength()
    {
        var command = CreateValidCommand() with
        {
            Name = new string('A', 201)
        };

        ShouldHaveValidationError(
            command,
            x => x.Name);
    }

    private static AddSeatingSectionCommand CreateValidCommand() =>
        new(
            Guid.NewGuid(),
            "VIP",
            true,
            100);
}