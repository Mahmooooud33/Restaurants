namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandHandler(ILogger<CreateRestaurantCommandHandler> logger,
    IMapper mapper,
    IRestaurantsRepository restaurantsRepository,
    IUserContext userContext,
    IFileService fileService) : IRequestHandler<CreateRestaurantCommand, int>
{
    public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();

        logger.LogInformation("{UserEmail} [{UserId}] is creating a new restaurant {@Restaurant}",
            currentUser!.Email,
            currentUser.Id,
        request);

        var restaurant = mapper.Map<Restaurant>(request);

        restaurant.OwnerId = currentUser.Id;
        restaurant.LogoUrl = await fileService.UploadFileAsync(request.RestaurantLogo);

        int id = await restaurantsRepository.Create(restaurant);
        return id;
    }
}