using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Restaurants.Queries.IsRestaurantNameExists;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Application.Tests.Restaurants.Queries.IsRestaurantNameExists;

public class IsRestaurantNameExistsQueryHandlerTests
{
    private readonly Mock<ILogger<IsRestaurantNameExistsQueryHandler>> _mockLogger;
    private readonly Mock<IRestaurantsRepository> _mockRepository;
    private readonly IsRestaurantNameExistsQueryHandler _handler;

    public IsRestaurantNameExistsQueryHandlerTests()
    {
        _mockLogger = new Mock<ILogger<IsRestaurantNameExistsQueryHandler>>();
        _mockRepository = new Mock<IRestaurantsRepository>();

        _handler = new IsRestaurantNameExistsQueryHandler(
            _mockLogger.Object,
            _mockRepository.Object);
    }

    [Fact()]
    public async Task Handle_RestaurantNameExists_ReturnsTrue()
    {
        // Arrange
        var restaurantName = "Pizza Place";
        var query = new IsRestaurantNameExistsQuery(restaurantName)
        { 
            Name = restaurantName
        };

        _mockRepository.Setup(r => r.IsRestaurantNameExistsAsync(query.Name)).ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeTrue();

        _mockRepository.Verify(r => r.IsRestaurantNameExistsAsync(query.Name), Times.Once);
    }

    [Fact]
    public async Task Handle_RestaurantNameDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var restaurantName = "Pizza Place";
        var query = new IsRestaurantNameExistsQuery(restaurantName)
        {
            Name = restaurantName
        };

        _mockRepository.Setup(r => r.IsRestaurantNameExistsAsync(query.Name))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeFalse();

        _mockRepository.Verify(r => r.IsRestaurantNameExistsAsync(query.Name), Times.Once);
    }
}