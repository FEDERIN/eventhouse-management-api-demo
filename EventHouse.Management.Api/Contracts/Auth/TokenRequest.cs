namespace EventHouse.Management.Api.Contracts.Auth;

public sealed class TokenRequest
{
    /// <summary>
    /// Username used to authenticate.
    /// </summary>
    public required string Username { get; init; }

    /// <summary>
    /// Password used to authenticate.
    /// </summary>
    public required string Password { get; init; }
}
