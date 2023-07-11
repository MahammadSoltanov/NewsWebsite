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
            .NotEmpty();
    }
}
