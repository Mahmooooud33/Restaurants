namespace Restaurants.Application.Dishes.Commands.DeleteDish;

internal class DeleteDishForRestaurantCommandHandler(ILogger<DeleteDishForRestaurantCommand> logger,
    IRestaurantsRepository restaurantsRepository,
    IDishesRepository dishesRepository) : IRequestHandler<DeleteDishForRestaurantCommand>
{
    public async Task Handle(DeleteDishForRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Removing dish with Id: {DishId}, for restaurant with Id: {RestaurantId}",
            request.DishId,
            request.RestaurantId);

        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId)
            ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

        var dish = restaurant.Dishes.FirstOrDefault(d => d.Id == request.DishId)
            ?? throw new NotFoundException(nameof(Dish), request.DishId.ToString());

        await dishesRepository.Delete(dish);
    }
}