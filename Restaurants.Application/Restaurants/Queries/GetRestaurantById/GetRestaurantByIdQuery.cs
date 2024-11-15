using MediatR;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Queries.GetRestaurantById;

public record GetRestaurantByIdQuery(int id) : IRequest<RestaurantDto>
{
    public int Id { get; set; } = id;
}
