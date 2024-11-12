using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Data;

namespace Restaurants.Infrastructure.Repositories;

internal class RestaurantsRepository(RestaurantsDbContext dbContext) 
    : IRestaurantsRepository
{
    public async Task<IEnumerable<Restaurant>> GetAllAsync()
    {
        var resturants = await dbContext.Restaurants
            .Include(r => r.Dishes)
            .ToListAsync();
        return resturants;
    }

    public async Task<Restaurant?> GetByIdAsync(int id)
    {
        var restaurants = await dbContext.Restaurants
            .Include(r => r.Dishes)
            .FirstOrDefaultAsync(x => x.Id == id);
        return restaurants;
    }
}
