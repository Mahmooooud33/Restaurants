using FluentValidation;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Validators;

public class CreateRestaurantDtoValidator : AbstractValidator<CreateRestaurantDto>
{
    private readonly IRestaurantsService _restaurantService;
    private readonly List<string> _validCategories = ["Italian", "Mexican", "Japanese", "Indian", "American"];

    public CreateRestaurantDtoValidator(IRestaurantsService restaurantService)
    {
        _restaurantService = restaurantService;

        RuleFor(dto => dto.Name)
            .Length(3, 50)
            .Must(name => !_restaurantService.IsRestaurantNameExists(name))
            .WithMessage("This restaurant name is already in use.");

        RuleFor(dto => dto.Description)
            .NotEmpty().WithMessage("Description is required.");

        RuleFor(dto => dto.Category)
            .Must(_validCategories.Contains)
            .WithMessage("Invalid category. Please choose from the valid categories.");

        RuleFor(dto => dto.ContactEmail)
            .EmailAddress().WithMessage("Please provide a valid Email Address.");

        RuleFor(dto => dto.ContactNumber)
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .WithMessage("Please provide a valid Phone Number.");

        RuleFor(dto => dto.PostalCode)
            .Matches(@"^\d{2}-\d{3}").WithMessage("Please provide a valid postal code {XX-XXX}.");
    }
}
