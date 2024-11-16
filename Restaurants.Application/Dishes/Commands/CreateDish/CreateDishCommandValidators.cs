using FluentValidation;

namespace Restaurants.Application.Dishes.Commands.CreateDish;

public class CreateDishCommandValidators : AbstractValidator<CreateDishCommand>
{
    public CreateDishCommandValidators()
    {
        RuleFor(d => d.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Price must be positive number");

        RuleFor(d => d.KiloCalories)
            .GreaterThanOrEqualTo(0)
            .WithMessage("KiloCalories must be positive number");
    }
}
