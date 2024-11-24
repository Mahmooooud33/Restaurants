using Microsoft.AspNetCore.Identity;

namespace Restaurants.Application.Users.Commands.UpdateUserDetails;

internal class UpdateUserDetailsCommandHandler(ILogger<UpdateUserDetailsCommandHandler> logger,
    IUserContext userContext,
    IUserStore<User> userStore) : IRequestHandler<UpdateUserDetailsCommand>
{
    public async Task Handle(UpdateUserDetailsCommand request, CancellationToken cancellationToken)
    {
        var user = userContext.GetCurrentUser();

        logger.LogInformation("Updating user: {UserId}, with {@Request}", user!.Id, request);

        var dbUser = await userStore.FindByIdAsync(user!.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(User), user!.Id);

        dbUser.FirstName = request.FirstName;
        dbUser.LastName = request.LastName;
        dbUser.FullName = $"{dbUser.FirstName} {dbUser.LastName}";
        dbUser.Nationality = request.Nationality;
        dbUser.DateOfBirth = request.DateOfBirth;

        await userStore.UpdateAsync(dbUser, cancellationToken);
    }
}
