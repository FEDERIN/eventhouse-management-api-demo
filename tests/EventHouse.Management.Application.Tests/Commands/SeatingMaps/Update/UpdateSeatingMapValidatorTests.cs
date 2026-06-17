using EventHouse.Management.Application.Commands.SeatingMaps.Update;

namespace EventHouse.Management.Application.Tests.Commands.SeatingMaps.Update;

public sealed class UpdateSeatingMapValidatorTests : ValidatorTestBase<UpdateSeatingMapCommand>
{
    public UpdateSeatingMapValidatorTests() : base(new UpdateSeatingMapCommandValidator()) { }

    [Fact]
    public void Should_Pass_When_Command_Is_Valid()
    {
        ShouldPassValidation(ValidCommand());
    }

    [Fact]
    public void Should_HaveError_When_Id_Is_Empty()
    {
        var command = ValidCommand() with { Id = Guid.Empty };

        ShouldHaveValidationError(command, x => x.Id, "Id is required.");
    }

    [Fact]
    public void Should_HaveError_When_Name_Is_Empty()
    {
        var command = ValidCommand() with { Name = string.Empty };

        ShouldHaveValidationError(command, x => x.Name, "Name is required.");
    }

    [Fact]
    public void Should_HaveError_When_Version_Is_Zero_Or_Less()
    {
        var command = ValidCommand() with { Version = 0 };

        ShouldHaveValidationError(command, x => x.Version, "Version must be greater than 0.");
    }

    [Fact]
    public void Should_HaveError_When_Name_Exceeds_Max_Length()
    {
        var command = ValidCommand() with { Name = new string('A', 201) };

        ShouldHaveValidationError(command, x => x.Name);
    }

    private static UpdateSeatingMapCommand ValidCommand() => new(
        Id: Guid.NewGuid(),
        Name: "Valid Seating Map Name",
        Version: 1,
        IsActive: true
    );
}