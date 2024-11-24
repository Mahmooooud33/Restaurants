namespace Restaurants.Application.Dishes.Commands.UpdateDish;

internal class UpdateDishCommandHandler(ILogger<UpdateDishCommand> logger,
    IRestaurantsRepository restaurantsRepository,
    IMapper mapper) : IRequestHandler<UpdateDishCommand>
{
    public async Task Handle(UpdateDishCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating Dish with ID: {DishId} for Restaurant with ID: {RestaurantId}",
            request.Id,
            request.RestaurantId);

        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId)
            ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

        var dish = restaurant.Dishes.FirstOrDefault(d => d.Id == request.Id)
            ?? throw new NotFoundException(nameof(Dish), request.Id.ToString());

        mapper.Map(request, dish);

        await restaurantsRepository.Update();
    }
}
