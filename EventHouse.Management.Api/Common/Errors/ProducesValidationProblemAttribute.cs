using Microsoft.AspNetCore.Mvc;

namespace EventHouse.Management.Api.Common.Errors;

public sealed class ProducesValidationProblemAttribute : ProducesResponseTypeAttribute
{
    public ProducesValidationProblemAttribute()
        : base(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest) { }
}