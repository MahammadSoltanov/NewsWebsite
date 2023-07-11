using FluentValidation;

namespace Application.CQRS.Languages.Commands.CreateLanguage;

public class CreateLanguageCommandValidator : AbstractValidator<CreateLanguageCommand>
{
    public CreateLanguageCommandValidator()
    {
        RuleFor(v => v.Code)
            .MaximumLength(5)
            .NotEmpty();
        RuleFor(v => v.Title).
            MaximumLength(25);
    }
}
