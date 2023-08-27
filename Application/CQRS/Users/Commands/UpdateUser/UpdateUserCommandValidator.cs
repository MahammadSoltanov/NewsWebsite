using FluentValidation;

namespace Application.CQRS.Users.Commands.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
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
        RuleFor(u => u.RoleId)
            .NotEmpty();
        RuleFor(v => v.Password)
            .MaximumLength(16)
            .NotEmpty();
    }
}
