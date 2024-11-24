using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Restaurants.Domain.Constants;
using System.Security.Claims;

namespace Restaurants.API.Tests;

public class FakePolicyEvaluator : IPolicyEvaluator
{
    public Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
    {
        var claimPrinciple = new ClaimsPrincipal();

        claimPrinciple.AddIdentity(new ClaimsIdentity(
            [
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Role, UserRoles.Admin)
            ]));

        var ticket = new AuthenticationTicket(claimPrinciple, "Test");
        var result = AuthenticateResult.Success(ticket);
        return Task.FromResult(result);
    }

    public Task<PolicyAuthorizationResult> AuthorizeAsync(AuthorizationPolicy policy, AuthenticateResult authenticationResult, HttpContext context, object? resource)
    {
        var result = PolicyAuthorizationResult.Success();
        return Task.FromResult(result);
    }
}