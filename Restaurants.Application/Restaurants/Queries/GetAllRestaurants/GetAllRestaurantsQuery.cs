namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public record GetAllRestaurantsQuery : IRequest<IEnumerable<RestaurantDto>>
{
}
