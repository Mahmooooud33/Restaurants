using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.API.Middlewares;
using System.Diagnostics;
using Xunit;

namespace Restaurants.API.Tests.Middlewares;

public class RequestTimeLoggingMiddlewareTests
{
    private readonly Mock<ILogger<RequestTimeLoggingMiddleware>> _loggerMock;
    private readonly RequestTimeLoggingMiddleware _middleware;

    public RequestTimeLoggingMiddlewareTests()
    {
        _loggerMock = new Mock<ILogger<RequestTimeLoggingMiddleware>>();
        _middleware = new RequestTimeLoggingMiddleware(_loggerMock.Object);
    }

    [Fact]
    public async Task InvokeAsync_WhenRequestTimeExceeds4Seconds_ShouldLog()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Method = "GET";
        context.Request.Path = "/test";
        var stopwatch = Stopwatch.StartNew();

        var next = new RequestDelegate(async _ =>
        {
            await Task.Delay(4100);
        });

        // Act
        await _middleware.InvokeAsync(context, next);
        stopwatch.Stop();

        // Assert
        _loggerMock.Verify(l => l.LogInformation("Request [{Verb}] at {Path} took {Time} ms", "GET", "/test-path", stopwatch.ElapsedMilliseconds), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_WhenRequestTimeNotExceeds4Seconds_ShouldCallNextMiddleware()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var wasCalled = false;

        var next = new RequestDelegate(_ =>
        {
            wasCalled = true;
            return Task.CompletedTask;
        });

        // Act
        await _middleware.InvokeAsync(context, next);

        // Assert
        wasCalled.Should().BeTrue();
    }
}