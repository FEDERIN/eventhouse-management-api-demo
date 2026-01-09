using Microsoft.AspNetCore.Mvc;

namespace EventHouse.Management.Api.Swagger;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public sealed class ProducesCreatedAttribute<T> : ProducesResponseTypeAttribute
{
    public ProducesCreatedAttribute()
        : base(typeof(T), StatusCodes.Status201Created)
    {
    }
}
