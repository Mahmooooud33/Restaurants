using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.DeleteDishes;

internal class DeleteDishesForRestaurantCommandHandler(ILogger<DeleteDishesForRestaurantCommand> logger,
    IRestaurantsRepository restaurantsRepository,
    IDishesRepository dishesRepository) : IRequestHandler<DeleteDishesForRestaurantCommand>
{
    public async Task Handle(DeleteDishesForRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogWarning("Removing all Dishes from Restaurant: {RestaurantId}", request.RestaurantId);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId)
            ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

        await dishesRepository.Delete(restaurant.Dishes);
    }
}