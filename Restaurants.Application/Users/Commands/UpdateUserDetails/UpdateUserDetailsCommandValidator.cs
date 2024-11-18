using FluentValidation;

namespace Restaurants.Application.Users.Commands.UpdateUserDetails;

public class UpdateUserDetailsCommandValidator : AbstractValidator<UpdateUserDetailsCommand>
{
    private readonly DateOnly specificDate = DateOnly.FromDateTime(DateTime.UtcNow);

    public UpdateUserDetailsCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .Length(3, 50)
            .WithMessage("First name is required and cannot less than 3 or exceed 50 characters.");

        RuleFor(x => x.LastName)
            .Length(3, 50)
            .WithMessage("Last name is required and cannot less than 3 or exceed 50 characters.");

        RuleFor(x => x.Nationality)
            .Length(3, 30)
            .WithMessage("Nationality is required and cannot less than 3 or exceed 30 characters.");

        RuleFor(x => x.DateOfBirth)
            .Custom((value, context) =>
            {
                if (specificDate.Equals(value) || value > specificDate || value < specificDate.AddYears(-90))
                    context.AddFailure("Enter Valid Date.");
            });
    }
}
