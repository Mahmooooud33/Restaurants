using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using System.Security.Claims;
using Xunit;

namespace Restaurants.Application.Tests.Users;

public class UserContextTests
{
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;

    public UserContextTests()
    {
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
    }

    [Fact()]
    public void GetCurrentUser_WhenUserAuthenticatedAndClaimsArePresent_ReturnCurrentUser()
    {
        //Arrange
        var dateOfBirth = new DateOnly(1990, 1, 1);
        
        var claims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier, "1"),
            new(ClaimTypes.Email, "test@test.com"),
            new(ClaimTypes.Role, UserRoles.Admin),
            new(ClaimTypes.Role, UserRoles.User),
            new("Nationality", "German"),
            new("DateOfBirth", dateOfBirth.ToString("yyyy-MM-dd"))
        };

        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));

        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext() 
        { 
            User = user 
        });

        var userContext = new UserContext(_httpContextAccessorMock.Object);

        //Act
        var currentUser = userContext.GetCurrentUser();

        //Assert
        currentUser.Should().NotBeNull();
        currentUser.Id.Should().Be("1");
        currentUser.Email.Should().Be("test@test.com");
        currentUser.Roles.Should().ContainInOrder(UserRoles.Admin, UserRoles.User);
        currentUser.Nationality.Should().Be("German");
        currentUser.DateOfBirth.Should().Be(dateOfBirth);
    }

    [Fact()]
    public void GetCurrentUser_WhenUserNotAuthenticated_ReturnNull()
    {
        //Arrange
        var identity = new ClaimsIdentity(); // Unauthenticated user
        var user = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext { User = user };
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        var userContext = new UserContext(_httpContextAccessorMock.Object);

        //Act
        var currentUser = userContext.GetCurrentUser();

        //Assert
        currentUser.Should().BeNull();
    }

    [Fact()]
    public void GetCurrentUser_WithUserContextNotPresent_ThrowsInvalidOperationException()
    {
        //Arrange
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext)null);

        var userContext = new UserContext(_httpContextAccessorMock.Object);

        //Act
        Action action = () => userContext.GetCurrentUser();

        //Assert
        action.Should().Throw<InvalidOperationException>().WithMessage("User context is not present");
    }

    [Fact()]
    public void GetCurrentUser_WithNullOptionalClaims_ReturnsCurrentUser()
    {
        // Arrange
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(ClaimTypes.Email, "test@example.com"),
            new Claim(ClaimTypes.Role, UserRoles.Admin),
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var user = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext { User = user };
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        var userContext = new UserContext(_httpContextAccessorMock.Object);

        // Act
        var currentUser = userContext.GetCurrentUser();

        //Assert
        currentUser.Should().NotBeNull();
        currentUser.Id.Should().Be("1");
        currentUser.Email.Should().Be("test@example.com");
        currentUser.Roles.Should().Contain(UserRoles.Admin);
        currentUser.Nationality.Should().BeNull();
        currentUser.DateOfBirth.Should().BeNull();
    }
}