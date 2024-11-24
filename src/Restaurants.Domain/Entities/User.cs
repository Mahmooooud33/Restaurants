using Microsoft.AspNetCore.Identity;

namespace Restaurants.Domain.Entities;

public class User : IdentityUser
{
    public string? FirstName { get; set; } = default!;
    public string? LastName { get; set; } = default!;
    public string? FullName { get; set; } = default!;
    public DateOnly? DateOfBirth { get; set; }
    public string? Nationality { get; set; }

    public List<Restaurant> OwnedRestaurants { get; set; } = [];
}