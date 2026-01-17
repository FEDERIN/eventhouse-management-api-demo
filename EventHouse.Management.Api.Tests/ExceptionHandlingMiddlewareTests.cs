using EventHouse.Management.Api.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Nodes;

namespace EventHouse.Management.Api.Tests;

public sealed class ExceptionHandlingMiddlewareTests
{
    [Fact]
    public async Task Invoke_WhenNextThrowsArgumentException_Returns400ProblemJson()
    {
        // Arrange
        var context = CreateHttpContext(env: "Production");
        static Task next(HttpContext _) => throw new ArgumentException("Invalid input");

        var mw = new ExceptionHandlingMiddleware(next);

        // Act
        await mw.Invoke(context);

        // Assert
        Assert.Equal(StatusCodes.Status400BadRequest, context.Response.StatusCode);
        //Assert.Equal("application/problem+json", context.Response.ContentType);

        var json = await ReadBodyAsJson(context);
        Assert.Equal("BAD_REQUEST", json["errorCode"]!.ToString());
        Assert.Equal("Bad request", json["title"]!.ToString());
        Assert.Equal("Invalid input", json["detail"]!.ToString());
        Assert.Equal("urn:eventhouse:error:BAD_REQUEST", json["type"]!.ToString());
        Assert.Equal("/test", json["instance"]!.ToString());
        Assert.True(!string.IsNullOrWhiteSpace(json["traceId"]?.ToString()));

        // no dev extras
        Assert.Null(json["exceptionType"]);
        Assert.Null(json["exceptionMessage"]);
    }

    [Fact]
    public async Task Invoke_WhenNextThrowsKeyNotFoundException_Returns404()
    {
        var context = CreateHttpContext(env: "Production");
        static Task next(HttpContext _) => throw new KeyNotFoundException("Artist not found");

        var mw = new ExceptionHandlingMiddleware(next);

        await mw.Invoke(context);

        Assert.Equal(StatusCodes.Status404NotFound, context.Response.StatusCode);

        var json = await ReadBodyAsJson(context);
        Assert.Equal("NOT_FOUND", json["errorCode"]!.ToString());
        Assert.Equal("Not found", json["title"]!.ToString());
        Assert.Equal("Artist not found", json["detail"]!.ToString());
        Assert.Equal("urn:eventhouse:error:NOT_FOUND", json["type"]!.ToString());
    }

    [Fact]
    public async Task Invoke_WhenNextThrowsInvalidOperationException_Returns409()
    {
        var context = CreateHttpContext(env: "Production");
        static Task next(HttpContext _) => throw new InvalidOperationException("Conflict!");

        var mw = new ExceptionHandlingMiddleware(next);

        await mw.Invoke(context);

        Assert.Equal(StatusCodes.Status409Conflict, context.Response.StatusCode);

        var json = await ReadBodyAsJson(context);
        Assert.Equal("CONFLICT", json["errorCode"]!.ToString());
        Assert.Equal("Conflict", json["title"]!.ToString());
        Assert.Equal("Conflict!", json["detail"]!.ToString());
        Assert.Equal("urn:eventhouse:error:CONFLICT", json["type"]!.ToString());
    }

    [Fact]
    public async Task Invoke_WhenNextThrowsUnknownException_Returns500_WithGenericMessage()
    {
        var context = CreateHttpContext(env: "Production");
        static Task next(HttpContext _) => throw new Exception("Sensitive details");

        var mw = new ExceptionHandlingMiddleware(next);

        await mw.Invoke(context);

        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);

        var json = await ReadBodyAsJson(context);
        Assert.Equal("UNEXPECTED_ERROR", json["errorCode"]!.ToString());
        Assert.Equal("Unexpected error", json["title"]!.ToString());
        Assert.Equal("An unexpected error occurred.", json["detail"]!.ToString());

        // asegura que NO se filtren detalles en prod
        Assert.NotEqual("Sensitive details", json["detail"]!.ToString());
        Assert.Null(json["exceptionType"]);
        Assert.Null(json["exceptionMessage"]);
    }

    [Fact]
    public async Task Invoke_InDevelopment_AddsExceptionExtras()
    {
        var context = CreateHttpContext(env: "Development");
        static Task next(HttpContext _) => throw new ArgumentException("Invalid input");

        var mw = new ExceptionHandlingMiddleware(next);

        await mw.Invoke(context);

        var json = await ReadBodyAsJson(context);

        Assert.Equal("ArgumentException", json["exceptionType"]!.ToString());
        Assert.Equal("Invalid input", json["exceptionMessage"]!.ToString());
    }

    private static DefaultHttpContext CreateHttpContext(string env)
    {
        var services = new ServiceCollection();

        // IHostEnvironment para que tu middleware lo lea desde RequestServices
        services.AddSingleton<IHostEnvironment>(new FakeHostEnvironment { EnvironmentName = env });

        var provider = services.BuildServiceProvider();

        var context = new DefaultHttpContext
        {
            RequestServices = provider
        };

        context.Request.Path = "/test";
        context.Response.Body = new MemoryStream();
        return context;
    }

    private static async Task<JsonObject> ReadBodyAsJson(HttpContext context)
    {
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(context.Response.Body);
        var text = await reader.ReadToEndAsync();

        var node = JsonNode.Parse(text);
        Assert.NotNull(node);
        return node!.AsObject();
    }

    private sealed class FakeHostEnvironment : IHostEnvironment
    {
        public string EnvironmentName { get; set; } = Environments.Production;
        public string ApplicationName { get; set; } = "TestApp";
        public string ContentRootPath { get; set; } = AppContext.BaseDirectory;
        public IFileProvider ContentRootFileProvider { get; set; } = default!;
    }
}
