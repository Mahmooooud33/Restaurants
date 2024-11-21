using FluentValidation.TestHelper;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Xunit;

namespace Restaurants.Application.Tests.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandValidatorTests
{
    private readonly CreateRestaurantCommandValidator _validator;

    public CreateRestaurantCommandValidatorTests()
    {
        _validator = new CreateRestaurantCommandValidator();
    }

    [Fact()]
    public void Validator_ForValidCommand_NotHaveValidationErrors()
    {
        //Arrange
        var command = new CreateRestaurantCommand()
        {
            Name = "test",
            Description = "test",
            Category = "Italian",
            ContactEmail = "test@test.com",
            PostalCode = "12-345"
        };

        //Act
        var result = _validator.TestValidate(command);

        //Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact()]
    public void Validator_ForInvalidCommand_HaveValidationErrors()
    {
        //Arrange
        var command = new CreateRestaurantCommand()
        {
            Name = "te",
            Description = "",
            Category = "Ital",
            ContactEmail = "@test.com",
            PostalCode = "12345"
        };

        //Act
        var result = _validator.TestValidate(command);

        //Assert
        result.ShouldHaveValidationErrorFor(c => c.Name);
        result.ShouldHaveValidationErrorFor(c => c.Description);
        result.ShouldHaveValidationErrorFor(c => c.Category);
        result.ShouldHaveValidationErrorFor(c => c.ContactEmail);
        result.ShouldHaveValidationErrorFor(c => c.PostalCode);
    }

    [Theory()]
    [InlineData("Italian")]
    [InlineData("Mexican")]
    [InlineData("Japanese")]
    [InlineData("Indian")]
    [InlineData("American")]
    public void Validator_ForValidCategory_NotHaveValidationErrors(string category)
    {
        //Arrange
        var command = new CreateRestaurantCommand { Category = category };

        //Act
        var result = _validator.TestValidate(command);

        //Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Category);
    }

    [Theory()]
    [InlineData("51321")]
    [InlineData("125 53")]
    [InlineData("12-6 34")]
    [InlineData("10-2 54")]
    public void Validator_ForInvalidPostalCode_HaveValidationErrors(string postalCode)
    {
        //Arrange
        var command = new CreateRestaurantCommand { PostalCode = postalCode };

        //Act
        var result = _validator.TestValidate(command);

        //Assert
        result.ShouldHaveValidationErrorFor(c => c.PostalCode);
    }
}