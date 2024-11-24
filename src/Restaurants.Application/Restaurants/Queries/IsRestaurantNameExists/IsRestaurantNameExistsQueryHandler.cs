namespace Restaurants.Application.Restaurants.Queries.IsRestaurantNameExists;

public class IsRestaurantNameExistsQueryHandler(ILogger<IsRestaurantNameExistsQueryHandler> logger,
    IRestaurantsRepository restaurantsRepository) : IRequestHandler<IsRestaurantNameExistsQuery, bool>
{
    public async Task<bool> Handle(IsRestaurantNameExistsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Checking if {RestaurantName} exists", request.Name);

        return await restaurantsRepository.IsRestaurantNameExistsAsync(request.Name);
    }
}