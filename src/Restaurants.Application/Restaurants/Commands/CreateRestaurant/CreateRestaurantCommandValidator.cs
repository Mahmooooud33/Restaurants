namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandValidator : AbstractValidator<CreateRestaurantCommand>
{
    private readonly List<string> _validCategories = ["Italian", "Mexican", "Japanese", "Indian", "American"];
    private readonly List<string> _allowedExtensions = [".jpg", ".jpeg", ".png", ".webp"];
    private const long MaxAllowedFileSize = 1048576; // 1MB

    public CreateRestaurantCommandValidator()
    {
        RuleFor(dto => dto.Name)
            .Length(3, 50);

        RuleFor(dto => dto.Description)
            .NotEmpty()
            .WithMessage("Description is required.");

        RuleFor(dto => dto.Category)
            .Must(_validCategories.Contains)
            .WithMessage("Invalid category. Please choose from the valid categories.");

        RuleFor(dto => dto.ContactEmail)
            .EmailAddress()
            .WithMessage("Please provide a valid Email Address.");

        RuleFor(dto => dto.ContactNumber)
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .WithMessage("Please provide a valid Phone Number.");

        RuleFor(dto => dto.PostalCode)
            .Matches(@"^\d{2}-\d{3}")
            .WithMessage("Please provide a valid postal code {XX-XXX}.");

        RuleFor(dto => dto.RestaurantLogo)
            .Custom((value, context) =>
            {
                if(value != null)
                {
                    if (!_allowedExtensions.Contains(Path.GetExtension(value.FileName)))
                        context.AddFailure($"File must be one of the following types: {string.Join(", ", _allowedExtensions)}.");

                    if(value.Length > MaxAllowedFileSize)
                        context.AddFailure($"File size cannot exceed {MaxAllowedFileSize / 1024} KB.");

                    if (!value.ContentType.Contains("image"))
                        context.AddFailure("File must be an image.");

                    if (value.Length == 0)
                        context.AddFailure("File cannot be empty.");
                }
            });
    }
}
