using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;

public interface IRestaurantsRepository
{
    Task<IEnumerable<Restaurant>> GetAllAsync();
    Task<Restaurant?> GetByIdAsync(int id);
    Task<int> Create(Restaurant entity);
    Task<bool> IsRestaurantNameExistsAsync(string name);
    Task Delete(Restaurant entity);
    Task Update();
    Task<IEnumerable<Restaurant>> GetAllMatchingAsync(string? searchPhrase);
}