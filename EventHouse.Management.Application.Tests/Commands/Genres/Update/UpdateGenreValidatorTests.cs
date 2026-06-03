using EventHouse.Management.Application.Commands.Genres.Update;

namespace EventHouse.Management.Application.Tests.Commands.Genres.Update;

public sealed class UpdateGenreValidatorTests : ValidatorTestBase<UpdateGenreCommand>
{
    public UpdateGenreValidatorTests() : base(new UpdateGenreCommandValidator()) { }

    [Fact]
    public void Should_HaveError_When_Id_Is_Empty()
    {
        var command = new UpdateGenreCommand(Guid.Empty, "Rock");

        ShouldHaveValidationError(command, x => x.Id, "Id must be a non-empty GUID.");
    }

    [Fact]
    public void Should_HaveError_When_Name_Is_Empty()
    {
        var command = new UpdateGenreCommand(Guid.NewGuid(), "");

        ShouldHaveValidationError(command, x => x.Name, "Name is required.");
    }

    [Fact]
    public void Should_Pass_When_Command_Is_Valid()
    {
        var command = new UpdateGenreCommand(Guid.NewGuid(), "Jazz");

        ShouldPassValidation(command);
    }
}