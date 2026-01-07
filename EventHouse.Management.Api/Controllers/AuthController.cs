using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventHouse.Management.Api.Controllers;

[ApiController]
[Route("auth")]
[AllowAnonymous]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    [HttpPost("token")]
    [AllowAnonymous]
    public IActionResult Token()
    {
        return StatusCode(StatusCodes.Status501NotImplemented);
    }
}


