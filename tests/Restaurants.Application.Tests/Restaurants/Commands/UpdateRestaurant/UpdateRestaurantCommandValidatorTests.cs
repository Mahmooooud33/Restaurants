using FluentValidation.TestHelper;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Xunit;

namespace Restaurants.Application.Tests.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandValidatorTests
{
    private readonly UpdateRestaurantCommandValidator _validator;

    public UpdateRestaurantCommandValidatorTests()
    {
        _validator = new UpdateRestaurantCommandValidator();
    }

    [Fact]
    public void Validator_WhenNameIsTooShort_ValidationError()
    {
        // Arrange
        var command = new UpdateRestaurantCommand { Name = "AB" };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Name);
    }

    [Fact]
    public void Validator_WhenNameIsTooLong_ValidationError()
    {
        // Arrange
        var command = new UpdateRestaurantCommand { Name = new string('A', 51) };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Name);
    }

    [Fact]
    public void Validator_WhenDescriptionIsEmpty_ValidationError()
    {
        // Arrange
        var command = new UpdateRestaurantCommand { Description = string.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Description)
              .WithErrorMessage("Description is required.");
    }

    [Fact]
    public void Validator_WhenNameAndDescriptionAreValid_NotHaveValidationError()
    {
        // Arrange
        var command = new UpdateRestaurantCommand
        {
            Name = "Valid Name",
            Description = "A valid description."
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Name);
        result.ShouldNotHaveValidationErrorFor(c => c.Description);
    }
}