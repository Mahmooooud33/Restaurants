namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant;

public class DeleteRestaurantCommandHandler(ILogger<DeleteRestaurantCommandHandler> logger,
    IRestaurantsRepository restaurantsRepository,
    IRestaurantAuthorizationService restaurantAuthorizationService) : IRequestHandler<DeleteRestaurantCommand>
{
    public async Task Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleteing Restaurant with ID: {@RestaurantId}", request.Id);

        var restaurant = await restaurantsRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(Restaurant), request.Id.ToString());

        if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Delete))
            throw new ForbiddenException();

        await restaurantsRepository.Delete(restaurant);
    }
}
