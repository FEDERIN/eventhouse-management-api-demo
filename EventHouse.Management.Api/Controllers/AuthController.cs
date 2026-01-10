using EventHouse.Management.Api.Common.Errors;
using EventHouse.Management.Api.Contracts.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EventHouse.Management.Api.Controllers;

[ApiController]
[Route("auth")]
[AllowAnonymous]
[Produces("application/json")]
public sealed class AuthController(IConfiguration configuration) : ControllerBase
{
    private readonly IConfiguration _configuration = configuration;

    [HttpPost("token")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EventHouseProblemDetails), StatusCodes.Status500InternalServerError)]
    public IActionResult Token()
    {
        var secret = _configuration["Auth:DevSecret"];
        var issuer = _configuration["Auth:Issuer"];
        var audience = _configuration["Auth:Audience"];

        if (string.IsNullOrWhiteSpace(secret) ||
            string.IsNullOrWhiteSpace(issuer) ||
            string.IsNullOrWhiteSpace(audience))
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new EventHouseProblemDetails
            {
                Title = "Auth not configured",
                Status = 500,
                Detail = "JWT settings are missing (Auth:DevSecret/Issuer/Audience).",
                ErrorCode = "AUTH_NOT_CONFIGURED",
                TraceId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Instance = HttpContext.Request.Path
            });
        }

        var claims = new[]
        {
        new Claim(JwtRegisteredClaimNames.Sub, "demo-user"),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("scope", "management")
    };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        const int expiresIn = 3600;

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddSeconds(expiresIn),
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new TokenResponse
        {
            AccessToken = jwt,
            TokenType = "Bearer",
            ExpiresIn = expiresIn
        });
    }
}
