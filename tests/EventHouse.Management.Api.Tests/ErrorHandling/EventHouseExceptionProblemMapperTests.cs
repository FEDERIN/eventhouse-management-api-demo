using EventHouse.Management.Api.ErrorHandling;
using EventHouse.Management.Application.Common.Interfaces;

namespace EventHouse.Management.Api.Tests.ErrorHandling;

public sealed class EventHouseExceptionProblemMapperTests
{
    [Fact]
    public void Map_WhenApplicationMapperDoesNotProvideType_UsesEventHouseType()
    {
        var mapper = new EventHouseExceptionProblemMapper(new FakeExceptionMapper());

        var result = mapper.Map(new ArgumentException("Invalid input"));

        Assert.Equal(400, result.Status);
        Assert.Equal("BAD_REQUEST", result.ErrorCode);
        Assert.Equal("urn:eventhouse:error:BAD_REQUEST", result.Type);
    }

    [Fact]
    public void Map_WhenApplicationMapperProvidesType_PreservesIt()
    {
        var mapper = new EventHouseExceptionProblemMapper(new FakeExceptionMapper());

        var result = mapper.Map(new InvalidOperationException("Conflict"));

        Assert.Equal("urn:eventhouse:error:conflict", result.Type);
    }

    private sealed class FakeExceptionMapper : IExceptionMapper
    {
        public (int StatusCode, string ErrorCode, string Title, string Detail, string Type) Map(Exception exception) =>
            exception switch
            {
                ArgumentException argumentException =>
                    (400, "BAD_REQUEST", "Bad request", argumentException.Message, string.Empty),
                _ => (409, "CONFLICT", "Conflict", exception.Message, "urn:eventhouse:error:conflict")
            };
    }
}
