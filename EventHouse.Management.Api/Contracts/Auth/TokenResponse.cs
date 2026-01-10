using Swashbuckle.AspNetCore.Annotations;

namespace EventHouse.Management.Api.Contracts.Auth;

public sealed record TokenResponse
{
    /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...</example>
    [SwaggerSchema(Description = "JWT access token.")]
    public string AccessToken { get; init; } = default!;

    /// <example>Bearer</example>
    [SwaggerSchema(Description = "Token type.")]
    public string TokenType { get; init; } = default!;

    /// <example>7200</example>
    [SwaggerSchema(Description = "Token lifetime in seconds.")]
    public int ExpiresIn { get; init; }
}
