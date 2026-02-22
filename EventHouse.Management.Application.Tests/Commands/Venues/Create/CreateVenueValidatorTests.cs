using EventHouse.Management.Application.Commands.Venues.Create;
using FluentAssertions;

namespace EventHouse.Management.Application.Tests.Commands.Venues.Create;

public sealed class CreateVenueValidatorTests
{
    private readonly CreateVenueCommandValidator _validator = new();

    private static CreateVenueCommand ValidCommand() =>
        new(
            Name: "Palace Theatre",
            Address: "123 Main St",
            City: "Valletta",
            Region: "Malta",
            CountryCode: "MT",
            Latitude: 35.8989m,
            Longitude: 14.5146m,
            TimeZoneId: "Europe/Malta",
            Capacity: 500,
            true
        );

    [Fact]
    public void Should_fail_when_name_is_empty()
    {
        var command = ValidCommand() with { Name = "" };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_fail_when_name_is_whitespace()
    {
        var command = ValidCommand() with { Name = "   " };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorCode.Contains("NotEmptyValidator") && e.ErrorMessage.Length > 0);
    }

    [Fact]
    public void Should_fail_when_country_code_is_invalid()
    {
        var command = ValidCommand() with { CountryCode = "ZZZ" };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("ISO-3166-1 alpha-2"));
    }

    [Theory]
    [InlineData(-90.0001)]
    [InlineData(90.0001)]
    public void Should_fail_when_latitude_is_out_of_range(decimal latitude)
    {
        var command = ValidCommand() with { Latitude = latitude };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(-180.0001)]
    [InlineData(180.0001)]
    public void Should_fail_when_longitude_is_out_of_range(decimal longitude)
    {
        var command = ValidCommand() with { Longitude = longitude };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_fail_when_time_zone_is_invalid()
    {
        var command = ValidCommand() with { TimeZoneId = "Europe/NotAPlace" };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("valid IANA time zone"));
    }

    [Fact]
    public void Should_fail_when_capacity_is_negative()
    {
        var command = ValidCommand() with { Capacity = -1 };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_pass_when_optional_fields_are_null()
    {
        var command = ValidCommand() with
        {
            Latitude = null,
            Longitude = null,
            TimeZoneId = null,
            Capacity = null
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }
}