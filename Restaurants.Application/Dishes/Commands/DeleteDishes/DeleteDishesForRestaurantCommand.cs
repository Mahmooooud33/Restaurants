namespace Restaurants.Application.Dishes.Commands.DeleteDishes;

public record DeleteDishesForRestaurantCommand(int restaurantId) : IRequest
{
    public int RestaurantId { get; } = restaurantId;
}
