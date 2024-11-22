namespace Restaurants.Application.Restaurants.Queries.GetRestaurantById;

public record GetRestaurantByIdQuery(int Id) : IRequest<RestaurantDto>
{
    public int Id { get; set; } = Id;
}
