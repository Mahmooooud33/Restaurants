﻿namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandValidator : AbstractValidator<UpdateRestaurantCommand>
{
    public UpdateRestaurantCommandValidator()
    {
        RuleFor(dto => dto.Name)
            .Length(3, 50);

        RuleFor(dto => dto.Description)
            .NotEmpty().WithMessage("Description is required.");
    }
}