using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Application.Tests.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryHandlerTests
{
    private readonly Mock<ILogger<GetAllRestaurantsQueryHandler>> _mockLogger;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IRestaurantsRepository> _mockRepository;
    private readonly GetAllRestaurantsQueryHandler _handler;

    public GetAllRestaurantsQueryHandlerTests()
    {
        _mockLogger = new Mock<ILogger<GetAllRestaurantsQueryHandler>>();
        _mockMapper = new Mock<IMapper>();
        _mockRepository = new Mock<IRestaurantsRepository>();

        _handler = new GetAllRestaurantsQueryHandler(
            _mockLogger.Object,
            _mockMapper.Object,
            _mockRepository.Object);
    }

    [Fact]
    public async Task Handle_RestaurantsExist_ReturnPagedResult()
    {
        // Arrange
        var query = new GetAllRestaurantsQuery
        {
            SearchPhrase = "Italian",
            PageSize = 5,
            PageNumber = 1,
            SortBy = "Name",
            SortDirection = SortDirection.Ascending
        };

        var restaurants = new List<Restaurant>
        {
            new() { Id = 1, Name = "Restaurant 1" },
            new() { Id = 2, Name = "Restaurant 2" }
        };

        var totalCount = 2;
        var restaurantDtos = new List<RestaurantDto>
        {
            new() { Id = 1, Name = "Restaurant 1" },
            new() { Id = 2, Name = "Restaurant 2" }
        };

        _mockRepository.Setup(r => r.GetAllMatchingAsync(
                query.SearchPhrase,
                query.PageSize,
                query.PageNumber,
                query.SortBy,
                query.SortDirection))
            .ReturnsAsync((restaurants, totalCount));

        _mockMapper.Setup(m => m.Map<IEnumerable<RestaurantDto>>(restaurants))
            .Returns(restaurantDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.TotalItemsCount.Should().Be(totalCount);
        result.Items.Should().BeEquivalentTo(restaurantDtos);
    }

    [Fact]
    public async Task Handle_NoRestaurantsMatch_ReturnEmptyPagedResult()
    {
        // Arrange
        var query = new GetAllRestaurantsQuery
        {
            SearchPhrase = "NonExistent",
            PageSize = 5,
            PageNumber = 1,
            SortBy = "Name",
            SortDirection = SortDirection.Ascending
        };

        var restaurants = new List<Restaurant>();
        var totalCount = 0;
        var restaurantDtos = new List<RestaurantDto>();

        _mockRepository.Setup(r => r.GetAllMatchingAsync(
                query.SearchPhrase,
                query.PageSize,
                query.PageNumber,
                query.SortBy,
                query.SortDirection))
            .ReturnsAsync((restaurants, totalCount));

        _mockMapper.Setup(m => m.Map<IEnumerable<RestaurantDto>>(restaurants))
            .Returns(restaurantDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
        result.TotalItemsCount.Should().Be(totalCount);
    }

    [Fact]
    public async Task Handle_RepositoryThrows_ReturnThrowException()
    {
        // Arrange
        var query = new GetAllRestaurantsQuery
        {
            SearchPhrase = "Error",
            PageSize = 5,
            PageNumber = 1,
            SortBy = "Name",
            SortDirection = SortDirection.Ascending
        };

        _mockRepository.Setup(r => r.GetAllMatchingAsync(
                query.SearchPhrase,
                query.PageSize,
                query.PageNumber,
                query.SortBy,
                query.SortDirection))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Database error");
    }
}