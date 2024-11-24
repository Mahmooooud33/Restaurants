using AutoMapper;
using FluentAssertions;
using Restaurants.Application.Dishes.Commands.CreateDish;
using Restaurants.Application.Dishes.Commands.UpdateDish;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Entities;
using Xunit;

namespace Restaurants.Application.Tests.Dishes.Dtos;

public class DishesProfileTests
{
    private readonly IMapper _mapper;

    public DishesProfileTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<DishesProfile>();
        });

        _mapper = configuration.CreateMapper();
    }

    [Fact()]
    public void CreateMap_ForDishToDishDto_MapCorrectly()
    {
        // Arrange
        var dish = new Dish
        {
            Id = 1,
            Name = "Existing Dish",
            Description = "Existing Description",
            Price = 12.99m,
            KiloCalories = 400,
            RestaurantId = 101
        };

        // Act
        var dishDto = _mapper.Map<DishDto>(dish);

        // Assert
        dishDto.Should().NotBeNull();
        dishDto.Id.Should().Be(dish.Id);
        dishDto.Name.Should().Be(dish.Name);
        dishDto.Description.Should().Be(dish.Description);
        dishDto.Price.Should().Be(dish.Price);
        dishDto.KiloCalories.Should().Be(dish.KiloCalories);
    }

    [Fact()]
    public void CreateMap_ForCreateDishCommandToDish_MapCorrectly()
    {
        // Arrange
        var createCommand = new CreateDishCommand
        {
            Name = "New Dish",
            Description = "New Description",
            Price = 9.99m,
            KiloCalories = 300,
            RestaurantId = 102
        };

        // Act
        var dish = _mapper.Map<Dish>(createCommand);

        // Assert
        dish.Should().NotBeNull();
        dish.Name.Should().Be(createCommand.Name);
        dish.Description.Should().Be(createCommand.Description);
        dish.Price.Should().Be(createCommand.Price);
        dish.KiloCalories.Should().Be(createCommand.KiloCalories);
        dish.RestaurantId.Should().Be(createCommand.RestaurantId);
    }

    [Fact()]
    public void CreateMap_ForUpdateDishCommandToDish_MapCorrectly()
    {
        // Arrange
        var updateCommand = new UpdateDishCommand
        {
            Id = 1,
            Name = "Updated Dish",
            Description = "Updated Description",
            Price = 19.99m,
            KiloCalories = 500,
            RestaurantId = 101
        };

        // Act
        var dish = _mapper.Map<Dish>(updateCommand);

        // Assert
        dish.Should().NotBeNull();
        dish.Id.Should().Be(updateCommand.Id);
        dish.Name.Should().Be(updateCommand.Name);
        dish.Description.Should().Be(updateCommand.Description);
        dish.Price.Should().Be(updateCommand.Price);
        dish.KiloCalories.Should().Be(updateCommand.KiloCalories);
        dish.RestaurantId.Should().Be(updateCommand.RestaurantId);
    }
}