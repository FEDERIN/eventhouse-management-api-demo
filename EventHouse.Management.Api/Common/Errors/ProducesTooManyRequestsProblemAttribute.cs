using Microsoft.AspNetCore.Mvc;

namespace EventHouse.Management.Api.Common.Errors;

public sealed class ProducesTooManyRequestsProblemAttribute : ProducesResponseTypeAttribute
{
    public ProducesTooManyRequestsProblemAttribute()
        : base(typeof(EventHouseProblemDetails), StatusCodes.Status429TooManyRequests)
    {
    }
}
