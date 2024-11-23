using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Authorization.Requirements.MaximumRestaurantsForOwner;
using Xunit;

namespace Restaurants.Infrastructure.Tests.Authorization.Requirements.MaximumRestaurantsForOwner;

public class OwnerMaximumRestaurantsRequirementHandlerTests
{
    private readonly Mock<ILogger<OwnerMaximumRestaurantsRequirementHandler>> _loggerMock;
    private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly OwnerMaximumRestaurantsRequirementHandler _handler;

    public OwnerMaximumRestaurantsRequirementHandlerTests()
    {
        _loggerMock = new Mock<ILogger<OwnerMaximumRestaurantsRequirementHandler>>();
        _restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        _userContextMock = new Mock<IUserContext>();

        _handler = new OwnerMaximumRestaurantsRequirementHandler(_loggerMock.Object,
            _restaurantsRepositoryMock.Object,
            _userContextMock.Object);
    }

    [Fact]
    public async Task HandleRequirementAsync_UserIsNull_ShouldFail()
    {
        // Arrange
        var requirement = new OwnerMaximumRestaurantsRequirement(2);

        var context = new AuthorizationHandlerContext([requirement], null, null);

        _userContextMock.Setup(x => x.GetCurrentUser()).Returns((CurrentUser?)null);

        // Act
        await _handler.HandleAsync(context);

        // Assert
        context.HasFailed.Should().BeTrue();
    }

    [Fact]
    public async Task HandleRequirementAsync_UserIsNotOwner_ShouldFail()
    {
        // Arrange
        var requirement = new OwnerMaximumRestaurantsRequirement(2);

        var context = new AuthorizationHandlerContext([requirement], null, null);

        var currentUser = new CurrentUser("1", "test@test.com", [], null, null);

        _userContextMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);

        // Act
        await _handler.HandleAsync(context);

        // Assert
        context.HasFailed.Should().BeTrue();
    }

    [Fact()]
    public async Task HandleRequirementAsync_UserHasCreatedMaximumRestaurants_ShouldSuccess()
    {
        // Arrange
        var currentUser = new CurrentUser("1", "test@test.com", [UserRoles.Owner], null, null);

        _userContextMock.Setup(m => m.GetCurrentUser()).Returns(currentUser);

        var restaurants = new List<Restaurant>()
        {
            new()
            {
                OwnerId = currentUser.Id
            },
            new()
            {
                OwnerId = currentUser.Id
            },
            new()
            {
                OwnerId = "2"
            }
        };

        _restaurantsRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(restaurants);

        _userContextMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);

        var requirement = new OwnerMaximumRestaurantsRequirement(2);

        var context = new AuthorizationHandlerContext([requirement], null, null);

        // Act
        await _handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeTrue();
    }

    [Fact()]
    public async Task HandleRequirementAsync_UserHasNotCreatedMaximumRestaurants_ShouldFail()
    {
        // Arrange
        var currentUser = new CurrentUser("1", "test@test.com", [], null, null);

        _userContextMock.Setup(m => m.GetCurrentUser()).Returns(currentUser);

        var restaurants = new List<Restaurant>()
        {
            new()
            {
                OwnerId = currentUser.Id
            },
            new()
            {
                OwnerId = "2"
            }
        };

        _restaurantsRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(restaurants);

        var requirement = new OwnerMaximumRestaurantsRequirement(2);

        var context = new AuthorizationHandlerContext([requirement], null, null);

        // Act
        await _handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeFalse();
        context.HasFailed.Should().BeTrue();
    }
}