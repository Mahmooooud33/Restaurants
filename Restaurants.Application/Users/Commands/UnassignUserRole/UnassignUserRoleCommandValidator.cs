using FluentValidation;
using Restaurants.Domain.Constants;

namespace Restaurants.Application.Users.Commands.UnassignUserRole;

internal class UnassignUserRoleCommandValidator : AbstractValidator<UnassignUserRoleCommand>
{
    private readonly List<string> _validRoles = [UserRoles.User, UserRoles.Admin, UserRoles.Owner];

    public UnassignUserRoleCommandValidator()
    {
        RuleFor(x => x.UserEmail)
        .EmailAddress()
        .WithMessage("Please provide a valid Email Address.");

        RuleFor(x => x.RoleName)
            .Must(_validRoles.Contains)
            .WithMessage("Invalid role. Should be one from the valid roles.");
    }
}
