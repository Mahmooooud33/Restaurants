using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Application.Restaurants;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Extensions;

public static class ServiceCollectionExtenstions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IRestaurantsService, RestaurantsService>();

        services.AddAutoMapper(typeof(RestaurantsProfile).Assembly);

    }
}
