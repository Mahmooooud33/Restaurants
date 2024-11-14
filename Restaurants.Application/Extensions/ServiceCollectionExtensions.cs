using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Restaurants;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Validators;

namespace Restaurants.Application.Extensions;

public static class ServiceCollectionExtenstions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IRestaurantsService, RestaurantsService>();

        services.AddAutoMapper(typeof(RestaurantsProfile).Assembly);

        services.AddValidatorsFromAssembly(typeof(CreateRestaurantDtoValidator).Assembly)
            .AddFluentValidationAutoValidation();
    }
}
