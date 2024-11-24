using Microsoft.AspNetCore.Authorization;

namespace Restaurants.Infrastructure.Authorization.Requirements.MaximumRestaurantsForOwner;

public class OwnerMaximumRestaurantsRequirement(int minimumRestaurantsCreated) : IAuthorizationRequirement
{
    public int MinimumRestaurantsCreated { get; } = minimumRestaurantsCreated;
}
