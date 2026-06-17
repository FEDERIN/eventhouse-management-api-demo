using EventHouse.Management.Application.Commands.Genres.Create;

namespace EventHouse.Management.Application.Tests.Commands.Genres.Create;

public sealed class CreateGenreValidatorTests : ValidatorTestBase<CreateGenreCommand>
{
    public CreateGenreValidatorTests() : base(new CreateGenreCommandValidator()) { }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Should_HaveError_When_Name_Is_Empty_Or_Whitespace(string invalidName)
    {
        var command = new CreateGenreCommand(invalidName);

        ShouldHaveValidationError(command, x => x.Name, "Name is required.");
    }

    [Fact]
    public void Should_HaveError_When_Name_Is_Too_Long()
    {
        var command = new CreateGenreCommand(new string('A', 201));

        ShouldHaveValidationError(command, x => x.Name, "Name must not exceed 200 characters.");
    }

    [Fact]
    public void Should_Pass_When_Command_Is_Valid()
    {
        var command = new CreateGenreCommand("Rock");

        ShouldPassValidation(command);
    }
}