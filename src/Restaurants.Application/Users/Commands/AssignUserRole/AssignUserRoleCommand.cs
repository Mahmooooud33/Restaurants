namespace Restaurants.Application.Users.Commands.AssignUserRole;

public record AssignUserRoleCommand : IRequest
{
    public string UserEmail { get; set; } = default!;
    public string RoleName { get; set; } = default!;
}
