namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryHandler(ILogger<GetAllRestaurantsQueryHandler> logger,
    IMapper mapper,
    IRestaurantsRepository restaurantsRepository) : IRequestHandler<GetAllRestaurantsQuery, PagedResult<RestaurantDto>>
{
    public async Task<PagedResult<RestaurantDto>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting {Count} restaurants", request.PageSize);

        var (restaurants, totalCount) = await restaurantsRepository.GetAllMatchingAsync(request.SearchPhrase, 
            request.PageSize, 
            request.PageNumber,
            request.SortBy,
            request.SortDirection);

        var restaurantDtos = mapper.Map<IEnumerable<RestaurantDto>>(restaurants);

        var result = new PagedResult<RestaurantDto>(restaurantDtos, totalCount, request.PageSize, request.PageNumber);
        return result;
    }
}
