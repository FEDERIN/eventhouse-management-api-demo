using Swashbuckle.AspNetCore.Annotations;

namespace EventHouse.Management.Api.Contracts.Auth;

public sealed record TokenResponse
{
    /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...</example>
    [SwaggerSchema(Description = "JWT access token.")]
    public required string AccessToken { get; init; }

    /// <example>Bearer</example>
    [SwaggerSchema(Description = "Token type.")]
    public required string TokenType { get; init; }

    /// <example>7200</example>
    [SwaggerSchema(Description = "Token lifetime in seconds.")]
    public required int ExpiresIn { get; init; }
}
