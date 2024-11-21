using FluentValidation.TestHelper;
using Restaurants.Application.Dishes.Commands.UpdateDish;
using Xunit;

namespace Restaurants.Application.Tests.Dishes.Commands.UpdateDish;

public class UpdateDishCommandValidatorsTests
{
    private readonly UpdateDishCommandValidators _validator;

    public UpdateDishCommandValidatorsTests()
    {
        _validator = new UpdateDishCommandValidators();
    }

    [Fact]
    public void Validator_PriceIsNegative_HaveValidationErrors()
    {
        // Arrange
        var command = new UpdateDishCommand { Price = -1 };

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
        var command = new UpdateDishCommand { KiloCalories = -1 };

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
        var command = new UpdateDishCommand { Price = 0 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Price);
    }

    [Fact]
    public void Validator_KiloCaloriesIsZeroOrPositive_NotHaveValidationErrors()
    {
        // Arrange
        var command = new UpdateDishCommand { KiloCalories = 0 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.KiloCalories);
    }
}