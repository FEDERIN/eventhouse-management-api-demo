using EventHouse.Management.Api.Contracts.Auth;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Auth;

[ExcludeFromCodeCoverage]
internal sealed class TokenResponseExample
    : IExamplesProvider<TokenResponse>
{
    public TokenResponse GetExamples() => new()
    {
        AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..",
        TokenType = "Bearer",
        ExpiresIn = 7200
    };
}
