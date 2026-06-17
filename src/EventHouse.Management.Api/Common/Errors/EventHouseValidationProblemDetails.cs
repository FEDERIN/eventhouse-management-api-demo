using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace EventHouse.Management.Api.Common.Errors;

public sealed class EventHouseValidationProblemDetails(IDictionary<string, string[]> errors) : ValidationProblemDetails(errors)
{
    [JsonPropertyName("errorCode")]
    public string? ErrorCode { get; set; }

    [JsonPropertyName("traceId")]
    public string? TraceId { get; set; }
}
