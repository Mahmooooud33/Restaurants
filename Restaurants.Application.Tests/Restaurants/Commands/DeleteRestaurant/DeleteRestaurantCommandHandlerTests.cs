using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Application.Tests.Restaurants.Commands.DeleteRestaurant;

public class DeleteRestaurantCommandHandlerTests
{
    private readonly Mock<ILogger<DeleteRestaurantCommandHandler>> _loggerMock;
    private readonly Mock<IRestaurantAuthorizationService> _restaurantAuthorizationServiceMock;
    private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock;

    private readonly DeleteRestaurantCommandHandler _handler;

    public DeleteRestaurantCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<DeleteRestaurantCommandHandler>>();
        _restaurantAuthorizationServiceMock = new Mock<IRestaurantAuthorizationService>();
        _restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();

        _handler = new DeleteRestaurantCommandHandler(_loggerMock.Object,
            _restaurantsRepositoryMock.Object,
            _restaurantAuthorizationServiceMock.Object);
    }

    [Fact()]
    public async Task Handle_ValidRequest_DeleteRestaurant()
    {
        // Arrange
        var restaurantId = 1;
        var command = new DeleteRestaurantCommand(restaurantId)
        {
            Id = restaurantId
        };

        var restaurant = new Restaurant()
        {
            Id = restaurantId,
        };

        _restaurantsRepositoryMock.Setup(r => r.GetByIdAsync(restaurantId)).ReturnsAsync(restaurant);

        _restaurantAuthorizationServiceMock.Setup(m => m.Authorize(restaurant, ResourceOperation.Delete)).Returns(true);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _restaurantsRepositoryMock.Verify(r => r.Delete(restaurant), Times.Once);
    }

    [Fact()]
    public async Task Handle_WithoutExistingRestaurant_ThrowNotFoundException()
    {
        // Arrange
        var restaurantId = 2;
        var command = new DeleteRestaurantCommand(restaurantId)
        {
            Id = restaurantId
        };

        _restaurantsRepositoryMock.Setup(r => r.GetByIdAsync(restaurantId)).ReturnsAsync((Restaurant?)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage($"Restaurant with Id: {restaurantId} not found");
    }

    [Fact()]
    public async Task Handle_UnauthorizedUser_ThrowForbiddenException()
    {
        // Arrange
        var restaurantId = 3;
        var command = new DeleteRestaurantCommand(restaurantId)
        {
            Id = restaurantId
        };

        var restaurant = new Restaurant()
        {
            Id = restaurantId,
        };

        _restaurantsRepositoryMock.Setup(r => r.GetByIdAsync(restaurantId)).ReturnsAsync(restaurant);

        _restaurantAuthorizationServiceMock.Setup(m => m.Authorize(restaurant, ResourceOperation.Delete)).Returns(false);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ForbiddenException>();
    }
}