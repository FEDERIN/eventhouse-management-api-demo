using EventHouse.Management.Application.Commands.Seating.Sections.Update;

namespace EventHouse.Management.Application.Tests.Commands.Seating.Sections.Update;

public sealed class UpdateSeatingSectionCommandValidatorTests
    : ValidatorTestBase<UpdateSeatingSectionCommand>
{
    public UpdateSeatingSectionCommandValidatorTests()
        : base(new UpdateSeatingSectionCommandValidator()) { }

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
    public void Should_HaveError_When_SectionId_Is_Empty()
    {
        var command = CreateValidCommand() with
        {
            SectionId = Guid.Empty
        };

        ShouldHaveValidationError(
            command,
            x => x.SectionId,
            "SectionId must be a non-empty GUID.");
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
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

    private static UpdateSeatingSectionCommand CreateValidCommand() =>
        new(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "VIP",
            100);
}