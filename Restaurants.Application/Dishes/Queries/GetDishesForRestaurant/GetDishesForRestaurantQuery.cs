namespace Restaurants.Application.Dishes.Queries.GetDishesForRestaurant;

public record GetDishesForRestaurantQuery(int RestaurantId) : IRequest<IEnumerable<DishDto>>
{
    public int RestaurantId { get; set; } = RestaurantId;
}
