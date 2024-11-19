namespace Restaurants.Application.Restaurants.Queries.IsRestaurantNameExists;

public record IsRestaurantNameExistsQuery(string name) : IRequest<bool>
{
    public string Name { get; set; } = name;
}
