namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandValidator : AbstractValidator<UpdateRestaurantCommand>
{
    private readonly List<string> _allowedExtensions = [".jpg", ".jpeg", ".png", ".webp"];
    private const long MaxAllowedFileSize = 1048576; // 1MB
    public UpdateRestaurantCommandValidator()
    {
        RuleFor(dto => dto.Name)
            .Length(3, 50);

        RuleFor(dto => dto.Description)
            .NotEmpty().WithMessage("Description is required.");

        RuleFor(dto => dto.RestaurantLogo)
            .Custom((value, context) =>
            {
                if (value != null)
                {
                    if (!_allowedExtensions.Contains(Path.GetExtension(value.FileName)))
                        context.AddFailure($"File must be one of the following types: {string.Join(", ", _allowedExtensions)}.");

                    if (value.Length > MaxAllowedFileSize)
                        context.AddFailure($"File size cannot exceed {MaxAllowedFileSize / 1024} KB.");

                    if (!value.ContentType.Contains("image"))
                        context.AddFailure("File must be an image.");

                    if (value.Length == 0)
                        context.AddFailure("File cannot be empty.");
                }
            });
    }
}