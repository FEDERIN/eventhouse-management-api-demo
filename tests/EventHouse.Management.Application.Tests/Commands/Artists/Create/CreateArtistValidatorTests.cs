using EventHouse.Management.Application.Commands.Artists.Create;
using EventHouse.Management.Application.Common.Enums;

namespace EventHouse.Management.Application.Tests.Commands.Artists.Create;

public sealed class CreateArtistValidatorTests : ValidatorTestBase<CreateArtistCommand>
{
    public CreateArtistValidatorTests() : base(new CreateArtistCommandValidator()) { }

    [Fact]
    public void Should_Pass_When_Command_Is_Valid()
    {
        var command = new CreateArtistCommand("The Rolling Stones", ArtistCategoryDto.Band);

        ShouldPassValidation(command);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Should_HaveError_When_Name_Is_Empty_Or_Whitespace(string invalidName)
    {
        var command = new CreateArtistCommand(invalidName, ArtistCategoryDto.Singer);

        ShouldHaveValidationError(command, x => x.Name, "Name is required.");
    }

    [Fact]
    public void Should_HaveError_When_Name_Exceeds_Max_Length()
    {
        var command = new CreateArtistCommand(new string('a', 201), ArtistCategoryDto.Band);

        ShouldHaveValidationError(command, x => x.Name);
    }

    [Fact]
    public void Should_HaveError_When_Category_Is_Invalid()
    {
        var command = new CreateArtistCommand("Test Artist", unchecked((ArtistCategoryDto)999));

        ShouldHaveValidationError(command, x => x.Category, "'Category' has a range of values which does not include the provided value.");
    }
}