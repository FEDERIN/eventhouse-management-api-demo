using System.Text.Json;
using System.Text.Json.Serialization;

namespace EventHouse.Management.Api.Tests;

public static class JsonTestOptions
{
    public static readonly JsonSerializerOptions Default = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };
}
