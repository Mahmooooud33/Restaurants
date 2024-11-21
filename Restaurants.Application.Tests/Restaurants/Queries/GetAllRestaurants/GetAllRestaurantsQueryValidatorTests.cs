using FluentValidation.TestHelper;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Domain.Entities;
using Xunit;

namespace Restaurants.Application.Tests.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryValidatorTests
{
    private readonly GetAllRestaurantsQueryValidator _validator;

    public GetAllRestaurantsQueryValidatorTests()
    {
        _validator = new GetAllRestaurantsQueryValidator();
    }

    [Theory()]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validator_WhenPageNumberIsLessThanOne_HaveValidationError(int pageNumber)
    {
        // Arrange
        var query = new GetAllRestaurantsQuery { PageNumber = pageNumber };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(q => q.PageNumber)
              .WithErrorMessage("PageNumber at least greater than or equal to 1.");
    }

    [Fact]
    public void Validator_WhenPageSizeIsNotAllowed_HaveValidationError()
    {
        // Arrange
        var query = new GetAllRestaurantsQuery { PageSize = 7 };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(q => q.PageSize)
              .WithErrorMessage("Page Size must be in [5 - 10 - 15 - 30]");
    }

    [Theory()]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(15)]
    [InlineData(30)]
    public void Validator_WhenPageSizeIsAllowed_HaveValidationError(int pageSize)
    {
        // Arrange
        var query = new GetAllRestaurantsQuery { PageSize = pageSize };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveValidationErrorFor(q => q.PageSize);
    }

    [Theory()]
    [InlineData(nameof(Restaurant.Description))]
    [InlineData(nameof(Restaurant.ContactEmail))]
    [InlineData("InvalidColumn")]
    public void Validator_WhenSortByIsNotAllowed_HaveValidationError(string sortBy)
    {
        // Arrange
        var query = new GetAllRestaurantsQuery { SortBy = sortBy };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(q => q.SortBy)
              .WithErrorMessage("Sort by is optional, or must be by [Name - Category]");
    }

    [Fact]
    public void Validator_WhenPageNumberAndPageSizeAreValid_NotHaveValidationError()
    {
        // Arrange
        var query = new GetAllRestaurantsQuery
        {
            PageNumber = 1,
            PageSize = 10
        };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveValidationErrorFor(q => q.PageNumber);
        result.ShouldNotHaveValidationErrorFor(q => q.PageSize);
    }

    [Theory()]
    [InlineData(nameof(Restaurant.Name))]
    [InlineData(nameof(Restaurant.Category))]
    public void Validator_WhenSortByIsAllowed_NotHaveValidationError(string sortBy)
    {
        // Arrange
        var query = new GetAllRestaurantsQuery { SortBy = sortBy };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveValidationErrorFor(q => q.SortBy);
    }

    [Fact]
    public void Validator_WhenSortByIsNull_NotHaveValidationError()
    {
        // Arrange
        var query = new GetAllRestaurantsQuery { SortBy = null };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveValidationErrorFor(q => q.SortBy);
    }
}