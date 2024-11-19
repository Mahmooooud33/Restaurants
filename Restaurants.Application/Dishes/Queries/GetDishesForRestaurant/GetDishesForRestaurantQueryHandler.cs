namespace Restaurants.Application.Dishes.Queries.GetDishesForRestaurant;

internal class GetDishesForRestaurantQueryHandler(ILogger<GetDishesForRestaurantQueryHandler> logger,
    IRestaurantsRepository restaurantsRepository,
    IMapper mapper) : IRequestHandler<GetDishesForRestaurantQuery, IEnumerable<DishDto>>
{
    public async Task<IEnumerable<DishDto>> Handle(GetDishesForRestaurantQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving dishes for restaurant with Id: {RestaurantId}", request.RestaurantId);

        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId)
               ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

        var dishes = mapper.Map<IEnumerable<DishDto>>(restaurant.Dishes);

        return dishes;
    }
}
