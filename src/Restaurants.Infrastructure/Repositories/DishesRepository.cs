using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Data;

namespace Restaurants.Infrastructure.Repositories;

internal class DishesRepository(RestaurantsDbContext dbContext) : IDishesRepository
{
    public async Task<int> Create(Dish entity)
    {
        dbContext.Dishes.Add(entity);
        await dbContext.SaveChangesAsync();
        return entity.Id;
    }

    public async Task Delete(IEnumerable<Dish> entities)
    {
        dbContext.Dishes.RemoveRange(entities);
        await dbContext.SaveChangesAsync();
    }

    public async Task Delete(Dish entity)
    {
        dbContext.Remove(entity);
        await dbContext.SaveChangesAsync();
    }
}
