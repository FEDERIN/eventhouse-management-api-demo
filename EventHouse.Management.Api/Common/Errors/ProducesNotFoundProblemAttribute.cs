using Microsoft.AspNetCore.Mvc;

namespace EventHouse.Management.Api.Common.Errors;

public sealed class ProducesNotFoundProblemAttribute : ProducesResponseTypeAttribute
{
    public ProducesNotFoundProblemAttribute()
        : base(typeof(EventHouseProblemDetails), StatusCodes.Status404NotFound) { }
}