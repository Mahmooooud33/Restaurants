using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Application.Tests.Restaurants.Queries.GetRestaurantById;

public class GetRestaurantByIdQueryHandlerTests
{
    private readonly Mock<ILogger<GetRestaurantByIdQueryHandler>> _mockLogger;
    private readonly Mock<IRestaurantsRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetRestaurantByIdQueryHandler _handler;

    public GetRestaurantByIdQueryHandlerTests()
    {
        _mockLogger = new Mock<ILogger<GetRestaurantByIdQueryHandler>>();
        _mockRepository = new Mock<IRestaurantsRepository>();
        _mockMapper = new Mock<IMapper>();

        _handler = new GetRestaurantByIdQueryHandler(
            _mockLogger.Object,
            _mockRepository.Object,
            _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_RestaurantExists_ReturnRestaurantDto()
    {
        // Arrange
        int restaurantId = 1;
        var query = new GetRestaurantByIdQuery(restaurantId)
        {
            Id = restaurantId
        };

        var restaurant = new Restaurant 
        { 
            Id = restaurantId, 
            Name = "Restaurant 1" 
        };

        var restaurantDto = new RestaurantDto 
        { 
            Id = restaurantId,
            Name = "Restaurant 1" 
        };

        _mockRepository.Setup(r => r.GetByIdAsync(query.Id))
            .ReturnsAsync(restaurant);

        _mockMapper.Setup(m => m.Map<RestaurantDto>(restaurant))
            .Returns(restaurantDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(restaurantDto);
    }

    [Fact]
    public async Task Handle_RestaurantDoesNotExist_ThrowNotFoundException()
    {
        // Arrange
        int restaurantId = 1;
        var query = new GetRestaurantByIdQuery(restaurantId)
        {
            Id = restaurantId
        };

        _mockRepository.Setup(r => r.GetByIdAsync(query.Id))
            .ReturnsAsync((Restaurant?)null);

        // Act
        var act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage($"Restaurant with Id: {restaurantId} not found");
    }
}