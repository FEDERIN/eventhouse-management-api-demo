using System.Text.Json;
using EventHouse.Management.Api.Common.Errors;

namespace EventHouse.Management.Api.Tests;

public sealed class EventHouseValidationProblemDetailsTests
{
    [Fact]
    public void Ctor_SetsErrors()
    {
        var errors = new Dictionary<string, string[]>
        {
            ["name"] = ["The Name field is required."]
        };

        var problem = new EventHouseValidationProblemDetails(errors);

        Assert.NotNull(problem.Errors);
        Assert.True(problem.Errors.ContainsKey("name"));
        Assert.Equal("The Name field is required.", problem.Errors["name"][0]);
    }

    [Fact]
    public void JsonSerialization_UsesCustomPropertyNames()
    {
        var errors = new Dictionary<string, string[]>
        {
            ["name"] = ["The Name field is required."]
        };

        var problem = new EventHouseValidationProblemDetails(errors)
        {
            ErrorCode = "VALIDATION_ERROR",
            TraceId = "trace-123"
        };

        var json = JsonSerializer.Serialize(problem);

        Assert.Contains("\"errorCode\":\"VALIDATION_ERROR\"", json);
        Assert.Contains("\"traceId\":\"trace-123\"", json);

        // y opcional: asegurarte de que NO aparezcan con PascalCase
        Assert.DoesNotContain("\"ErrorCode\"", json);
        Assert.DoesNotContain("\"TraceId\"", json);
    }
}

