using System.Net.Http.Json;
using EventHouse.Management.Api.Contracts.Auth;

namespace EventHouse.Management.Api.Tests;

public static class AuthTestExtensions
{
    public static async Task<string> GetBearerTokenAsync(this HttpClient client)
    {
        var res = await client.PostAsync("/auth/token", content: null);
        res.EnsureSuccessStatusCode();

        var token = await res.Content.ReadFromJsonAsync<TokenResponse>()
            ?? throw new InvalidOperationException("TokenResponse was null");

        return $"{token.TokenType} {token.AccessToken}";
    }
}
