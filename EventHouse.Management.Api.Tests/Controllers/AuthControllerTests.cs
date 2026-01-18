using System.IdentityModel.Tokens.Jwt;
using EventHouse.Management.Api.Common.Errors;
using EventHouse.Management.Api.Contracts.Auth;
using EventHouse.Management.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EventHouse.Management.Api.Tests.Controllers;

public sealed class AuthControllerTests
{
    [Fact]
    public void Token_WhenCredentialsAreInvalid_Returns401ProblemJson()
    {
        // Arrange
        var config = new ConfigurationBuilder().AddInMemoryCollection().Build();
        var controller = CreateController(config, path: "/auth/token");

        var request = new TokenRequest { Username = "wrong", Password = "demo" };

        // Act
        var result = controller.Token(request);

        // Assert
        var obj = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status401Unauthorized, obj.StatusCode);

        var problem = Assert.IsType<EventHouseProblemDetails>(obj.Value);
        Assert.Equal("UNAUTHORIZED", problem.ErrorCode);
        Assert.Equal("urn:eventhouse:error:UNAUTHORIZED", problem.Type);
        Assert.Equal("Unauthorized", problem.Title);
        Assert.Equal("Invalid credentials. Use demo/demo.", problem.Detail);
        Assert.Equal("/auth/token", problem.Instance);

        Assert.Contains("application/problem+json", obj.ContentTypes);
    }

    [Fact]
    public void Token_WhenAuthSettingsMissing_Returns500AuthNotConfiguredProblemJson()
    {
        // Arrange
        // Sin Auth:* configurado
        var config = new ConfigurationBuilder().AddInMemoryCollection().Build();
        var controller = CreateController(config, path: "/auth/token");

        var request = new TokenRequest { Username = "demo", Password = "demo" };

        // Act
        var result = controller.Token(request);

        // Assert
        var obj = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, obj.StatusCode);

        var problem = Assert.IsType<EventHouseProblemDetails>(obj.Value);
        Assert.Equal("AUTH_NOT_CONFIGURED", problem.ErrorCode);
        Assert.Equal("urn:eventhouse:error:AUTH_NOT_CONFIGURED", problem.Type);
        Assert.Equal("Auth not configured", problem.Title);
        Assert.Equal("JWT settings are missing (Auth:DevSecret/Issuer/Audience).", problem.Detail);
        Assert.Equal("/auth/token", problem.Instance);

        Assert.Contains("application/problem+json", obj.ContentTypes);
    }

    [Fact]
    public void Token_WhenValidRequest_Returns200WithJwtTokenResponse()
    {
        // Arrange
        var settings = new Dictionary<string, string?>
        {
            ["Auth:DevSecret"] = "EVENTHOUSE_LOCAL_DEV_SECRET_1234567890",
            ["Auth:Issuer"] = "eventhouse.local",
            ["Auth:Audience"] = "eventhouse.management"
        };

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(settings!)
            .Build();

        var controller = CreateController(config, path: "/auth/token");

        var request = new TokenRequest { Username = "demo", Password = "demo" };

        // Act
        var result = controller.Token(request);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<TokenResponse>(ok.Value);

        Assert.Equal("Bearer", response.TokenType);
        Assert.Equal(3600, response.ExpiresIn);
        Assert.False(string.IsNullOrWhiteSpace(response.AccessToken));

        // Validar que sea un JWT parseable
        var handler = new JwtSecurityTokenHandler();
        Assert.True(handler.CanReadToken(response.AccessToken));

        var jwt = handler.ReadJwtToken(response.AccessToken);

        // Issuer/Audience
        Assert.Equal("eventhouse.local", jwt.Issuer);
        Assert.Contains("eventhouse.management", jwt.Audiences);

        // Claims esperados
        Assert.Contains(jwt.Claims, c => c.Type == JwtRegisteredClaimNames.Sub && c.Value == "demo-user");
        Assert.Contains(jwt.Claims, c => c.Type == "scope" && c.Value == "management");
        Assert.Contains(jwt.Claims, c => c.Type == JwtRegisteredClaimNames.Jti && !string.IsNullOrWhiteSpace(c.Value));

        // Expiración aproximada (no exacta)
        Assert.True(jwt.ValidTo > DateTime.UtcNow);
    }

    private static AuthController CreateController(IConfiguration config, string path)
    {
        var controller = new AuthController(config)
        {
            // Setear HttpContext para TraceIdentifier y Request.Path
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };

        controller.HttpContext.TraceIdentifier = "trace-123";
        controller.HttpContext.Request.Path = path;

        return controller;
    }
}
