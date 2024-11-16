using AutoMapper;
using Restaurants.Application.Dishes.Commands.CreateDish;
using Restaurants.Application.Dishes.Commands.UpdateDish;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Dishes.Dtos;

public class DishesProfile : Profile
{
    public DishesProfile()
    {
        CreateMap<UpdateDishCommand, Dish>();
        CreateMap<CreateDishCommand, Dish>();
        CreateMap<Dish, DishDto>();
    }
}