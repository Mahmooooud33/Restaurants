using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Repositories;

namespace Restaurants.Infrastructure.Authorization.Requirements.MaximumRestaurantsForOwner;

internal class OwnerMaximumRestaurantsRequirementHandler(ILogger<OwnerMaximumRestaurantsRequirementHandler> logger,
    IRestaurantsRepository restaurantsRepository,
    IUserContext userContext) : AuthorizationHandler<OwnerMaximumRestaurantsRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        OwnerMaximumRestaurantsRequirement requirement)
    {
        var currentUser = userContext.GetCurrentUser();

        var resturants = await restaurantsRepository.GetAllAsync();

        if (currentUser is null || !currentUser.IsInRole(UserRoles.Owner))
        {
            context.Fail();
            return;
        }

        var userRestaurantsCreated = resturants.Count(x => x.OwnerId == currentUser.Id);

        logger.LogInformation("User: {Email}, Restaurants Created: {Restaurants} = Handling OwnerMaximumRestaurantsRequirement",
        currentUser!.Email,
        userRestaurantsCreated);

        if (userRestaurantsCreated >= requirement.MinimumRestaurantsCreated)
        {
            context.Succeed(requirement);
            return;
        }

        context.Fail();
    }
}
