using EventHouse.Management.Api.Common.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventHouse.Management.Api.Controllers;

[Authorize]
[ProducesResponseType(typeof(EventHouseProblemDetails), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(EventHouseProblemDetails), StatusCodes.Status403Forbidden)]
[ProducesResponseType(typeof(EventHouseProblemDetails), StatusCodes.Status500InternalServerError)]
public abstract class BaseApiController : ControllerBase
{}
