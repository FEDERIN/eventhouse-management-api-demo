using System.Net;
using System.Net.Http.Json;
using EventHouse.Management.Api.Common.Errors; // Donde vive EventHouseProblemDetails
using FluentAssertions;

namespace EventHouse.Management.Api.Tests.Common;

public static class HttpResponseExtensions
{
     public static Task ShouldBeProblemJson(this HttpResponseMessage response, HttpStatusCode expectedStatus)
    {
        response.StatusCode.Should().Be(expectedStatus);
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/problem+json");
        return Task.CompletedTask;
    }

     public static async Task ShouldHaveErrorCode(this HttpResponseMessage response, HttpStatusCode expectedStatus, string expectedErrorCode)
    {
        await response.ShouldBeProblemJson(expectedStatus);

        var problem = await response.Content.ReadFromJsonAsync<EventHouseProblemDetails>(JsonTestOptions.Default);

        problem.Should().NotBeNull();
        problem!.ErrorCode.Should().Be(expectedErrorCode);
    }

     public static async Task<T> ReadContentAsync<T>(this HttpResponseMessage response)
    {
        var content = await response.Content.ReadFromJsonAsync<T>(JsonTestOptions.Default);
        content.Should().NotBeNull();
        return content!;
    }
}