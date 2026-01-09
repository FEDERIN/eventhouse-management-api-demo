using Microsoft.AspNetCore.Mvc;

namespace EventHouse.Management.Api.Swagger;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public sealed class ProducesOkAttribute<T> : ProducesResponseTypeAttribute
{
    public ProducesOkAttribute()
        : base(typeof(T), StatusCodes.Status200OK)
    {
    }
}
