using EventHouse.Management.Application.Commands.Venues.Update;

namespace EventHouse.Management.Application.Tests.Commands.Venues.Update;

public sealed class UpdateVenueValidatorTests : ValidatorTestBase<UpdateVenueCommand>
{
    public UpdateVenueValidatorTests() : base(new UpdateVenueCommandValidator()) { }

    [Fact]
    public void Should_HaveError_When_Id_Is_Empty()
    {
        var command = ValidCommand() with { Id = Guid.Empty };

        ShouldHaveValidationError(command, x => x.Id);
    }

    [Fact]
    public void Should_HaveError_When_Name_Is_Empty()
    {
        var command = ValidCommand() with { Name = "" };

        ShouldHaveValidationError(command, x => x.Name);
    }

    [Fact]
    public void Should_HaveError_When_Name_Is_Whitespace()
    {
        var command = ValidCommand() with { Name = "   " };

        ShouldHaveValidationError(command, x => x.Name);
    }

    [Fact]
    public void Should_HaveError_When_Country_Code_Is_Invalid()
    {
        var command = ValidCommand() with { CountryCode = "ZZZ" };

        ShouldHaveValidationError(command, x => x.CountryCode, "CountryCode must be a valid ISO-3166-1 alpha-2 code (e.g. 'ES').");
    }

    [Theory]
    [InlineData(-90.0001)]
    [InlineData(90.0001)]
    public void Should_HaveError_When_Latitude_Is_Out_Of_Range(decimal latitude)
    {
        var command = ValidCommand() with { Latitude = latitude };

        ShouldHaveValidationError(command, x => x.Latitude);
    }

    [Theory]
    [InlineData(-180.0001)]
    [InlineData(180.0001)]
    public void Should_HaveError_When_Longitude_Is_Out_Of_Range(decimal longitude)
    {
        var command = ValidCommand() with { Longitude = longitude };

        ShouldHaveValidationError(command, x => x.Longitude);
    }

    [Fact]
    public void Should_HaveError_When_Time_Zone_Is_Invalid()
    {
        var command = ValidCommand() with { TimeZoneId = "Europe/NotAPlace" };

        ShouldHaveValidationError(command, x => x.TimeZoneId, "TimeZoneId must be a valid IANA time zone (e.g. 'Europe/Malta').");
    }

    [Fact]
    public void Should_HaveError_When_Capacity_Is_Negative()
    {
        var command = ValidCommand() with { Capacity = -1 };

        ShouldHaveValidationError(command, x => x.Capacity);
    }

    [Fact]
    public void Should_Pass_When_Optional_Fields_Are_Null()
    {
        var command = ValidCommand() with
        {
            Capacity = null
        };

        ShouldPassValidation(command);
    }

    private static UpdateVenueCommand ValidCommand() =>
    new(
        Id: Guid.NewGuid(),
        Name: "Palace Theatre",
        Address: "123 Main St",
        City: "Valletta",
        Region: "Malta",
        CountryCode: "MT",
        Latitude: 35.8989m,
        Longitude: 14.5146m,
        TimeZoneId: "Europe/Malta",
        Capacity: 500,
        IsActive: true
    );
}