using FluentValidation;

namespace Application.CQRS.Users.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(v => v.Name)
            .MaximumLength(25)
            .NotEmpty();
        RuleFor(v => v.Surname)
            .MaximumLength(25)
            .NotEmpty();
        RuleFor(v => v.Email)
            .MaximumLength(50)
            .NotEmpty()
            .EmailAddress().WithMessage("Invalid Email adress");
        RuleFor(v => v.Password)
            .MaximumLength(16)
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit")
            .Matches("[!@#$%^&*(),.?\":{}|<>]").WithMessage("Password must contain at least one special character")
            .NotEmpty();
    }
}
