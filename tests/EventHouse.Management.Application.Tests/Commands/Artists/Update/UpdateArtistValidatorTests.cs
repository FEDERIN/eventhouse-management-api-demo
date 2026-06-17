using EventHouse.Management.Application.Commands.Artists.Update;
using EventHouse.Management.Application.Common.Enums;

namespace EventHouse.Management.Application.Tests.Commands.Artists.Update;

public sealed class UpdateArtistValidatorTests : ValidatorTestBase<UpdateArtistCommand>
{
    public UpdateArtistValidatorTests() : base(new UpdateArtistCommandValidator()) { }

    [Fact]
    public void Should_HaveError_When_Id_Is_Empty()
    {
        var command = new UpdateArtistCommand(Guid.Empty, "Test Artist", ArtistCategoryDto.Band);

        ShouldHaveValidationError(command, x => x.Id, "Id must be a non-empty GUID.");
    }

    [Fact]
    public void Should_HaveError_When_Name_Is_Empty()
    {
        var command = new UpdateArtistCommand(Guid.NewGuid(), "", ArtistCategoryDto.Band);

        ShouldHaveValidationError(command, x => x.Name, "Name is required.");
    }

    [Fact]
    public void Should_HaveError_When_Category_Is_Invalid()
    {
        var command = new UpdateArtistCommand(Guid.NewGuid(), "Test Artist", unchecked((ArtistCategoryDto)999));

        ShouldHaveValidationError(command, x => x.Category, "'Category' has a range of values which does not include the provided value.");
    }

    [Fact]
    public void Should_Pass_When_Command_Is_Valid()
    {
        var command = new UpdateArtistCommand(Guid.NewGuid(), "Valid Artist Name", ArtistCategoryDto.Singer);

        ShouldPassValidation(command);
    }
}