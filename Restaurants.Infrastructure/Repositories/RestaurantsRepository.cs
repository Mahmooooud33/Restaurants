using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Data;

namespace Restaurants.Infrastructure.Repositories;

internal class RestaurantsRepository(RestaurantsDbContext dbContext) 
    : IRestaurantsRepository
{
    public async Task<int> Create(Restaurant entity)
    {
        dbContext.Restaurants.Add(entity);
        await dbContext.SaveChangesAsync();
        return entity.Id;
    }

    public async Task Delete(Restaurant entity)
    {
        dbContext.Remove(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Restaurant>> GetAllAsync()
    {
        var resturants = await dbContext.Restaurants
            .Include(r => r.Dishes)
            .ToListAsync();
        return resturants;
    }

    public async Task<IEnumerable<Restaurant>> GetAllMatchingAsync(string? searchPhrase)
    {
        var searchPharseLower = searchPhrase?.ToLower().Trim();

        var resturants = await dbContext.Restaurants
            .Include(r => r.Dishes)
            .Where(r => searchPharseLower == null || r.Name.ToLower().Contains(searchPharseLower)
                                                  || r.Description.ToLower().Contains(searchPharseLower))
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

    public async Task<bool> IsRestaurantNameExistsAsync(string name)
    {
        return await dbContext.Restaurants.AnyAsync(x => x.Name == name);
    }

    public async Task Update()
    {
        await dbContext.SaveChangesAsync();
    }
}