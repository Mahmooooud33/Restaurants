using MediatR;

namespace Restaurants.Application.Dishes.Commands.DeleteDish;

public record DeleteDishForRestaurantCommand(int restuarantId, int dishId) : IRequest
{
    public int RestaurantId { get; set; } = restuarantId;
    public int DishId { get; set; } = dishId;
}
