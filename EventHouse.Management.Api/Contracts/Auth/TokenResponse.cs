using Swashbuckle.AspNetCore.Annotations;

namespace EventHouse.Management.Api.Contracts.Auth;

public sealed record TokenResponse
{
    [SwaggerSchema(Description = "JWT access token.")]
    public required string AccessToken { get; init; }

    [SwaggerSchema(Description = "Token type.")]
    public required string TokenType { get; init; }

    [SwaggerSchema(Description = "Token lifetime in seconds.")]
    public required int ExpiresIn { get; init; }
}
