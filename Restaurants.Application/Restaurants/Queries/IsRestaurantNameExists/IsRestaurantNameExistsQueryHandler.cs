using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.IsRestaurantNameExists;

internal class IsRestaurantNameExistsQueryHandler(ILogger<IsRestaurantNameExistsQueryHandler> logger,
    IRestaurantsRepository restaurantsRepository) : IRequestHandler<IsRestaurantNameExistsQuery, bool>
{
    public async Task<bool> Handle(IsRestaurantNameExistsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Checking if {RestaurantName} exists", request.Name);

        return await restaurantsRepository.IsRestaurantNameExistsAsync(request.Name);
    }
}