﻿using MediatR;

namespace Restaurants.Application.Dishes.Commands.UpdateDish;

public record UpdateDishCommand : IRequest
{
    public int DishId { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; }
    public int? KiloCalories { get; set; }

    public int RestaurantId { get; set; }
}