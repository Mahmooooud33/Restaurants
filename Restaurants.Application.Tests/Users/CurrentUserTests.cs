using FluentAssertions;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Xunit;

namespace Restaurants.Application.Tests.Users;

public class CurrentUserTests
{
    // TestMethod_Scenario_ExpectedResult
    [Theory()]
    [InlineData(UserRoles.Admin)]
    [InlineData(UserRoles.User)]
    [InlineData("")]
    public void IsInRole_WithMatchingRole_ReturnsTrue(string roleName)
    {
        //arrange
        var currentUser = new CurrentUser("1", "test@test.com", [UserRoles.Admin, UserRoles.User], null, null);

        //act
        var isInRole = currentUser.IsInRole(UserRoles.Admin);

        //assert
        isInRole.Should().BeTrue();
    }

    [Fact()]
    public void IsInRole_WithNoMatchingRole_ReturnsFalse()
    {
        //arrange
        var currentUser = new CurrentUser("1", "test@test.com", [UserRoles.Admin, UserRoles.User], null, null);

        //act
        var isInRole = currentUser.IsInRole(UserRoles.Owner);

        //assert
        isInRole.Should().BeFalse();
    }

    [Fact()]
    public void IsInRole_WithNoMatchingRoleCase_ReturnsFalse()
    {
        //arrange
        var currentUser = new CurrentUser("1", "test@test.com", [UserRoles.Admin, UserRoles.User], null, null);

        //act
        var isInRole = currentUser.IsInRole(UserRoles.Admin.ToLower());

        //assert
        isInRole.Should().BeFalse();
    }

    [Fact]
    public void Constructor_ShouldAssignValuesCorrectly_ReturnTrue()
    {
        // Arrange
        var roles = new[] {UserRoles.Admin, UserRoles.User};
        var dateOfBirth = new DateOnly(1990, 1, 1);

        // Act
        var user = new CurrentUser(
            Id: "123",
            Email: "user@example.com",
            Roles: roles,
            Nationality: "US",
            DateOfBirth: dateOfBirth
        );

        // Assert
        user.Id.Should().Be("123");
        user.Email.Should().Be("user@example.com");
        user.Roles.Should().BeEquivalentTo(roles);
        user.Nationality.Should().Be("US");
        user.DateOfBirth.Should().Be(dateOfBirth);
    }

    [Fact]
    public void Constructor_AllowNullNationalityAndDateOfBirth_ReturnTrue()
    {
        // Arrange & Act
        var user = new CurrentUser(
            Id: "123",
            Email: "user@example.com",
            Roles: [UserRoles.Admin, UserRoles.User],
            Nationality: null,
            DateOfBirth: null
        );

        // Assert
        user.Nationality.Should().BeNull();
        user.DateOfBirth.Should().BeNull();
    }
}