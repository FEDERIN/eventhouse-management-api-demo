using EventHouse.Management.Application.Commands.Artists.Create;
using EventHouse.Management.Application.Common.Enums;
using FluentAssertions;


namespace EventHouse.Management.Application.Tests.Commands.Artists.Create;

public sealed class CreateArtistValidatorTests
{
    private readonly CreateArtistCommandValidator _validator = new();

    [Fact]
    public void Should_fail_when_name_is_empty()
    {
        var command = new CreateArtistCommand(
            "", ArtistCategoryDto.Band
        );

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_fail_when_category_is_invalid()
    {
        var command = new CreateArtistCommand(
            "Test", unchecked((ArtistCategoryDto)999)
            );

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }
}
