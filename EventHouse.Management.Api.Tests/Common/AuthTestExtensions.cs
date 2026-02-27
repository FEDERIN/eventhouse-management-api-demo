using EventHouse.Management.Api.Contracts.Auth;
using EventHouse.Management.Api.Tests.Abstractions;
using System.Net.Http.Json;

namespace EventHouse.Management.Api.Tests.Common;

public static class AuthTestExtensions
{
    public static async Task<string> GetBearerTokenAsync(this HttpClient client)
    {
        var res = await client.PostAsJsonAsync("/auth/token", new TokenRequest
        {
            Username = "demo",
            Password = "demo"
        });

        if (!res.IsSuccessStatusCode)
        {
            var body = await res.Content.ReadAsStringAsync();
            throw new InvalidOperationException(
                $"Failed to obtain token. Status={(int)res.StatusCode} {res.StatusCode}. Body={body}");
        }

        var token = await res.Content.ReadFromJsonAsync<TokenResponse>()
            ?? throw new InvalidOperationException("TokenResponse was null");

        if (string.IsNullOrWhiteSpace(token.AccessToken))
            throw new InvalidOperationException("AccessToken was empty");

        return $"{token.TokenType} {token.AccessToken}";
    }


    public static HttpRequestMessage WithoutAuthentication(this HttpRequestMessage request)
    {
        request.Options.Set(AuthedHandler.SkipAuth, true);
        return request;
    }
}