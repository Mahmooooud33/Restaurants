using Xunit;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Users;
using Microsoft.AspNetCore.Authorization;
using FluentAssertions;
using Restaurants.Infrastructure.Authorization.Requirements.MinimumAge;

namespace Restaurants.Infrastructure.Tests.Authorization.Requirements.MinimumAge;

public class MinimumAgeRequirementHandlerTests
{
    private readonly Mock<ILogger<MinimumAgeRequirementHandler>> _loggerMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly MinimumAgeRequirementHandler _handler;

    public MinimumAgeRequirementHandlerTests()
    {
        _loggerMock = new Mock<ILogger<MinimumAgeRequirementHandler>>();
        _userContextMock = new Mock<IUserContext>();

        _handler = new MinimumAgeRequirementHandler(
            _loggerMock.Object,
            _userContextMock.Object
        );
    }

    [Fact]
    public async Task HandleRequirementAsync_DateOfBirthIsNull_ShouldFail()
    {
        // Arrange
        var currentUser = new CurrentUser("1", "test@test.com", null!, null, null);

        _userContextMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);

        var requirement = new MinimumAgeRequirement(18);

        var context = new AuthorizationHandlerContext([requirement], null!, null);

        // Act
        await _handler.HandleAsync(context);

        // Assert
        context.HasFailed.Should().BeTrue();
    }

    [Fact]
    public async Task HandleRequirementAsync_UserMeetsMinimumAge_ShouldSucceed()
    {
        // Arrange
        var currentUser = new CurrentUser("1", "test@test.com", null!, null, DateOnly.FromDateTime(DateTime.Today.AddYears(-20)));

        _userContextMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);

        var requirement = new MinimumAgeRequirement(18);

        var context = new AuthorizationHandlerContext([requirement], null!, null);

        // Act
        await _handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeTrue();
    }

    [Fact]
    public async Task HandleRequirementAsync_UserDoesNotMeetMinimumAge_ShouldFail()
    {
        // Arrange
        var currentUser = new CurrentUser("1", "test@test.com", null!, null, DateOnly.FromDateTime(DateTime.Today.AddYears(-16)));

        _userContextMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);

        var requirement = new MinimumAgeRequirement(18);

        var context = new AuthorizationHandlerContext([requirement], null!, null);

        // Act
        await _handler.HandleAsync(context);

        // Assert
        context.HasFailed.Should().BeTrue();
    }
}