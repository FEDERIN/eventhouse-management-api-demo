using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace EventHouse.Management.Api.Common.Errors;

public sealed class EventHouseProblemDetails : ProblemDetails
{
    [JsonPropertyName("errorCode")]
    public string? ErrorCode { get; set; }

    [JsonPropertyName("traceId")]
    public string? TraceId { get; set; }
}
