

using EventHouse.Management.Application.Commands.Artists.Update;
using EventHouse.Management.Application.Common.Enums;
using FluentAssertions;

namespace EventHouse.Management.Application.Tests.Commands.Artists.Update
{
    public sealed class UpdateArtistValidatorTests
    {
        private readonly UpdateArtistCommandValidator _validator = new();

        [Fact]
        public void Should_fail_when_id_is_empty()
        {
            var command = new UpdateArtistCommand(
                Guid.Empty,
                "Test",
                ArtistCategoryDto.Band
            );

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Should_fail_when_name_is_empty()
        {
            var command = new UpdateArtistCommand(
                Guid.NewGuid(),
                "", 
                ArtistCategoryDto.Band
            );

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Should_fail_when_category_is_invalid()
        {

            var command = new UpdateArtistCommand(
                Guid.NewGuid(),
                "Test", 
                unchecked((ArtistCategoryDto)999)
                );

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
        }
    }
}
