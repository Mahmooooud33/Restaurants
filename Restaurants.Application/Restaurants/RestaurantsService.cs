using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.DTOs;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants;

internal class RestaurantsService(IRestaurantsRepository restaurantsRepository,
    ILogger<RestaurantsService> logger) : IRestaurantsService
{
    public async Task<IEnumerable<RestaurantDto>> GetAll()
    {
        logger.LogInformation("Getting all restaurants");
        var restaurants = await restaurantsRepository.GetAllAsync();

        var restaurantDtos = restaurants.Select(RestaurantDto.FromEntity);

        return restaurantDtos!;
    }

    public async Task<RestaurantDto?> GetById(int id)
    {
        logger.LogInformation($"Getting restaurant {id}");
        var restaurant = await restaurantsRepository.GetByIdAsync(id);

        var restaurantDto = RestaurantDto.FromEntity(restaurant);

        return restaurantDto;
    }
}
