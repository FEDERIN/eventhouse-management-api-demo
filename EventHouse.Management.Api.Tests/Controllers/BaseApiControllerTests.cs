using EventHouse.Management.Api.Common;
using EventHouse.Management.Api.Common.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventHouse.Management.Api.Tests.Controllers;

public sealed class BaseApiControllerTests
{
    [Fact]
    public void BadRequestProblem_WhenCalled_Returns400ProblemJson()
    {
        var c = CreateController(path: "/test");

        var result = c.CallBadRequestProblem(
            code: "BAD_REQUEST",
            title: "Bad request",
            detail: "Invalid input");

        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        Assert.Contains("application/problem+json", result.ContentTypes);

        var problem = Assert.IsType<EventHouseProblemDetails>(result.Value);
        Assert.Equal("BAD_REQUEST", problem.ErrorCode);
        Assert.Equal("urn:eventhouse:error:BAD_REQUEST", problem.Type);
        Assert.Equal("/test", problem.Instance);
        Assert.NotNull(problem.TraceId);
    }

    [Fact]
    public void ConflictProblem_WhenCalled_Returns409ProblemJson()
    {
        var c = CreateController(path: "/test");

        var result = c.CallConflictProblem(
            code: "CONFLICT",
            title: "Conflict",
            detail: "Already exists");

        Assert.Equal(StatusCodes.Status409Conflict, result.StatusCode);

        var problem = Assert.IsType<EventHouseProblemDetails>(result.Value);
        Assert.Equal("CONFLICT", problem.ErrorCode);
    }

    [Fact]
    public void CreateProblem_WhenExtensionsProvided_AddsThemToProblemDetails()
    {
        var c = CreateController(path: "/test");

        var ext = new Dictionary<string, object?>
        {
            ["entityId"] = "abc",
            ["extra"] = 123
        };

        var result = c.CallNotFoundProblem(
            code: "NOT_FOUND",
            title: "Not found",
            detail: "Missing",
            ext: ext);

        var problem = Assert.IsType<EventHouseProblemDetails>(result.Value);

        Assert.Equal("abc", problem.Extensions["entityId"]);
        Assert.Equal(123, problem.Extensions["extra"]);
    }

    [Fact]
    public void ValidationProblemWithCode_WhenCalled_Returns400WithValidationProblemDetailsAndExtensions()
    {
        var c = CreateController(path: "/test");

        var errors = new Dictionary<string, string[]>
        {
            ["name"] = new[] { "The Name field is required." }
        };

        var ext = new Dictionary<string, object?>
        {
            ["entity"] = "Artist"
        };

        var result = c.CallValidationProblemWithCode(
            code: "VALIDATION_ERROR",
            title: "Validation error",
            detail: "One or more validation errors occurred.",
            errors: errors,
            ext: ext);

        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);

        var problem = Assert.IsType<ValidationProblemDetails>(result.Value);
        Assert.Equal("urn:eventhouse:error:VALIDATION_ERROR", problem.Type);
        Assert.Equal("/test", problem.Instance);
        Assert.Equal("Validation error", problem.Title);

        // Extensions obligatorias
        Assert.Equal("VALIDATION_ERROR", problem.Extensions["errorCode"]);
        Assert.NotNull(problem.Extensions["traceId"]);
        Assert.NotNull(problem.Extensions["correlationId"]);

        // Ext custom
        Assert.Equal("Artist", problem.Extensions["entity"]);
    }

    [Fact]
    public void GetCorrelationId_WhenPresentInItems_ReturnsItemValue()
    {
        var c = CreateController(path: "/test");
        c.HttpContext.Items["X-Correlation-Id"] = "corr-items";

        var correlationId = c.CallGetCorrelationId();

        Assert.Equal("corr-items", correlationId);
    }

    [Fact]
    public void GetCorrelationId_WhenNotInItems_UsesHeaderValue()
    {
        var c = CreateController(path: "/test");
        c.HttpContext.Request.Headers["X-Correlation-Id"] = "corr-header";

        var correlationId = c.CallGetCorrelationId();

        Assert.Equal("corr-header", correlationId);
    }

    private static TestController CreateController(string path)
    {
        var controller = new TestController();

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        controller.HttpContext.TraceIdentifier = "trace-123";
        controller.HttpContext.Request.Path = path;

        return controller;
    }

    // Controller fake solo para exponer protected methods
    private sealed class TestController : BaseApiController
    {
        public ObjectResult CallNotFoundProblem(string code, string title, string detail, IDictionary<string, object?>? ext = null)
            => NotFoundProblem(code, title, detail, ext);

        public ObjectResult CallConflictProblem(string code, string title, string detail, IDictionary<string, object?>? ext = null)
            => ConflictProblem(code, title, detail, ext);

        public ObjectResult CallBadRequestProblem(string code, string title, string detail, IDictionary<string, object?>? ext = null)
            => BadRequestProblem(code, title, detail, ext);

        public ObjectResult CallValidationProblemWithCode(
            string code,
            string title,
            string detail,
            IDictionary<string, string[]> errors,
            IDictionary<string, object?>? ext = null)
            => ValidationProblemWithCode(code, title, detail, errors, ext);

        public string? CallGetCorrelationId()
            => GetCorrelationId();
    }
}
