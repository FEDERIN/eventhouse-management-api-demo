using EventHouse.Management.Api.Contracts.Auth;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Auth;

[ExcludeFromCodeCoverage]
internal class TokenRequestExample
    : IExamplesProvider<TokenRequest>
{
    public TokenRequest GetExamples() => new()
    {
        Username = "demo",
        Password = "demo"
    };
}
