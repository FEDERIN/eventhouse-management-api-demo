using EventHouse.Management.Application.Commands.Seating.Maps.Create;

namespace EventHouse.Management.Application.Tests.Commands.Seating.Maps.Create;

public sealed class CreateSeatingMapValidatorTests : ValidatorTestBase<CreateSeatingMapCommand>
{
    public CreateSeatingMapValidatorTests() : base(new CreateSeatingMapCommandValidator()) { }

    [Fact]
    public void Should_Pass_When_Command_Is_Valid()
    {
        ShouldPassValidation(ValidCommand());
    }

    [Fact]
    public void Should_HaveError_When_Name_Is_Empty()
    {
        ShouldHaveValidationError(ValidCommand() with { Name = string.Empty }, x => x.Name);
    }

    [Fact]
    public void Should_HaveError_When_Name_Is_Null()
    {
        ShouldHaveValidationError(ValidCommand() with { Name = null! }, x => x.Name);
    }

    [Fact]
    public void Should_HaveError_When_Name_Is_Whitespace()
    {
        ShouldHaveValidationError(ValidCommand() with { Name = "   " }, x => x.Name);
    }

    [Fact]
    public void Should_HaveError_When_VenueId_Is_Empty()
    {
        ShouldHaveValidationError(ValidCommand() with { VenueId = Guid.Empty }, x => x.VenueId);
    }

    [Fact]
    public void Should_HaveError_When_Version_Is_Less_Than_One()
    {
        ShouldHaveValidationError(ValidCommand() with { Version = 0 }, x => x.Version);
    }

    private static CreateSeatingMapCommand ValidCommand() =>
        new(
            VenueId: Guid.NewGuid(),
            Name: "Main Hall",
            Version: 1
        );
}