namespace Restaurants.Domain.Entities;

public class DishImage
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = default!;

    public int DishId { get; set; }
}