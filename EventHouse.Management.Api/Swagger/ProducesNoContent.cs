using Microsoft.AspNetCore.Mvc;

namespace EventHouse.Management.Api.Swagger;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public sealed class ProducesNoContentAttribute : ProducesResponseTypeAttribute
{
    public ProducesNoContentAttribute()
        : base(StatusCodes.Status204NoContent)
    {
    }
}
