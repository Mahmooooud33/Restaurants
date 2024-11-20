namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public record GetAllRestaurantsQuery : IRequest<IEnumerable<RestaurantDto>>
{
    public string? SearchPhrase { get; set; }
}
