using Microsoft.AspNetCore.Mvc;

namespace EventHouse.Management.Api.Common.Errors;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public sealed class ProducesConflictProblemAttribute : ProducesResponseTypeAttribute
{
    public ProducesConflictProblemAttribute()
        : base(typeof(EventHouseProblemDetails), StatusCodes.Status409Conflict)
    {
    }
}
