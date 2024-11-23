using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.API.Middlewares;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using System.Net;
using Xunit;

namespace Restaurants.API.Tests.Middlewares;

public class ErrorHandlingMiddlewareTests
{
    private readonly Mock<ILogger<ErrorHandlingMiddleware>> _loggerMock;
    private readonly ErrorHandlingMiddleware _middleware;
    private readonly DefaultHttpContext _context;

    public ErrorHandlingMiddlewareTests()
    {
        _loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        _middleware = new ErrorHandlingMiddleware(_loggerMock.Object);

        _context = new DefaultHttpContext();
    }

    [Fact()]
    public async Task InvokeAsync_NoExceptionThrown_CallNextDelegate()
    {
        // Arrange
        var nextDelegateMock = new Mock<RequestDelegate>();

        // Act
        await _middleware.InvokeAsync(_context, nextDelegateMock.Object);

        // Assert
        nextDelegateMock.Verify(x => x(_context), Times.Once);
    }

    [Fact()]
    public async Task InvokeAsync_NotFoundExceptionThrown_ReturnsStatus404()
    {
        // Arrange
        var notFoundException = new NotFoundException(nameof(Restaurant), "1");

        // Act
        await _middleware.InvokeAsync(_context, _ => throw notFoundException);

        // Assert
        _context.Response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Fact()]
    public async Task InvokeAsync_ForbiddenExceptionThrown_ReturnsStatus403()
    {
        // Arrange
        var exception = new ForbiddenException();

        // Act
        await _middleware.InvokeAsync(_context, _ => throw exception);

        // Assert
        _context.Response.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
    }

    [Fact()]
    public async Task InvokeAsync_UnauthorizedAccessExceptionThrown_ReturnsStatus401()
    {
        // Arrange
        var exception = new UnauthorizedAccessException();

        // Act
        await _middleware.InvokeAsync(_context, _ => throw exception);

        // Assert
        _context.Response.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
    }

    [Fact()]
    public async Task InvokeAsync_GenericlExceptionThrown_ReturnsStatus500()
    {
        // Arrange
        var exception = new Exception();

        // Act
        await _middleware.InvokeAsync(_context, _ => throw exception);

        // Assert
        _context.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}