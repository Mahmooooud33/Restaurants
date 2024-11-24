using Xunit;
using FluentValidation.TestHelper;
using Restaurants.Application.Dishes.Commands.CreateDish;

namespace Restaurants.Application.Tests.Dishes.Commands.CreateDish;

public class CreateDishCommandValidatorsTests
{
    private readonly CreateDishCommandValidators _validator;

    public CreateDishCommandValidatorsTests()
    {
        _validator = new CreateDishCommandValidators();
    }

    [Fact]
    public void Validator_PriceIsNegative_HaveValidationErrors()
    {
        // Arrange
        var command = new CreateDishCommand { Price = -1 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Price)
              .WithErrorMessage("Price must be positive number");
    }

    [Fact]
    public void Validator_KiloCaloriesIsNegative_HaveValidationErrors()
    {
        // Arrange
        var command = new CreateDishCommand { KiloCalories = -1 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.KiloCalories)
              .WithErrorMessage("KiloCalories must be positive number");
    }

    [Fact]
    public void Validator_PriceIsZeroOrPositive_NotHaveValidationErrors()
    {
        // Arrange
        var command = new CreateDishCommand { Price = 0 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Price);
    }

    [Fact]
    public void Validator_KiloCaloriesIsZeroOrPositive_NotHaveValidationErrors()
    {
        // Arrange
        var command = new CreateDishCommand { KiloCalories = 0 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.KiloCalories);
    }
}