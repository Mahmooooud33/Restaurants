using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Data;
using System.Linq.Expressions;

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

    public async Task<(IEnumerable<Restaurant>, int)> GetAllMatchingAsync(string? searchPhrase, 
        int pageSize, 
        int pageNumber,
        string? sortBy,
        SortDirection sortDirection)
    {
        var searchPharseLower = searchPhrase?.ToLower().Trim();

        var baseQuery = dbContext.Restaurants
            .Include(r => r.Dishes)
            .Where(r => searchPharseLower == null || r.Name.ToLower().Contains(searchPharseLower)
                                                  || r.Description.ToLower().Contains(searchPharseLower));

        var totalCount = await baseQuery.CountAsync();

        if(sortBy != null)
        {
            var columnSelector = new Dictionary<string, Expression<Func<Restaurant, object>>>
            {
                { nameof(Restaurant.Name), r => r.Name },
                { nameof(Restaurant.Category), r => r.Category },
            };

            var selectedColumn = columnSelector[sortBy];

            baseQuery = sortDirection == SortDirection.Ascending ? baseQuery.OrderBy(selectedColumn) 
                                                                 : baseQuery.OrderByDescending(selectedColumn);
        }

        var resturants = await baseQuery
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync();

        return (resturants, totalCount);
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