namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryValidator : AbstractValidator<GetAllRestaurantsQuery>
{   
    private readonly int[] allowPageSizes = [ 5, 10, 15, 30 ];
    private readonly string[] allowSortingByColumnNames = [nameof(RestaurantDto.Name), nameof(RestaurantDto.Category)];

    public GetAllRestaurantsQueryValidator()
    {
        RuleFor(r => r.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("PageNumber at least greater than or equal to 1.");

        RuleFor(r => r.PageSize)
            .Must(value => allowPageSizes.Contains(value))
            .WithMessage($"Page Size must be in [{string.Join(" - ", allowPageSizes)}]");

        RuleFor(r => r.SortBy)
            .Must(value => allowSortingByColumnNames.Contains(value))
            .When(q => q.SortBy != null)
            .WithMessage($"Sort by is optional, or must be by [{string.Join(" - ", allowSortingByColumnNames)}]");
    }
}