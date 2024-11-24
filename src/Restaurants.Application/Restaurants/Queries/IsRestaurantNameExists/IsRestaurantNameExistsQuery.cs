namespace Restaurants.Application.Restaurants.Queries.IsRestaurantNameExists;

public record IsRestaurantNameExistsQuery(string Name) : IRequest<bool>
{
    public string Name { get; set; } = Name;
}
