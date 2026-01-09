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
    [AllowAnonymous]
    public IActionResult Token()
    {
        var secret = _configuration["Auth:DevSecret"];
        var issuer = _configuration["Auth:Issuer"];
        var audience = _configuration["Auth:Audience"];

        if (string.IsNullOrWhiteSpace(secret))
            return Problem(
                title: "Auth not configured",
                detail: "JWT secret is missing",
                statusCode: StatusCodes.Status500InternalServerError
            );

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, "demo-user"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("scope", "management")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new
        {
            accessToken = jwt,
            tokenType = "Bearer",
            expiresIn = 3600
        });
    }
}
