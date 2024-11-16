using MediatR;
using Restaurants.Application.Dishes.Dtos;

namespace Restaurants.Application.Dishes.Queries.GetDishByIdForRestaurant;

public record GetDishByIdForRestaurantQuery(int restuarantId, int dishId) : IRequest<DishDto>
{
    public int RestaurantId { get; set; } = restuarantId;
    public int DishId { get; set; } = dishId;
}