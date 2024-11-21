namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public record GetAllRestaurantsQuery : IRequest<PagedResult<RestaurantDto>>
{
    public string? SearchPhrase { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string? SortBy { get; set; }
    public SortDirection SortDirection { get; set; }
}