using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Restaurants;

namespace Restaurants.Application.Extensions;

public static class ServiceCollectionExtenstions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IRestaurantsService, RestaurantsService>();
    }
}
