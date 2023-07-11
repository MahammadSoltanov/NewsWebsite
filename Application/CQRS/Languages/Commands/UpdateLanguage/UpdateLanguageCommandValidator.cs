using FluentValidation;

namespace Application.CQRS.Languages.Commands.UpdateLanguage;

public class UpdateLanguageCommandValidator : AbstractValidator<UpdateLanguageCommand>
{
    public UpdateLanguageCommandValidator()
    {
        RuleFor(v => v.Code)
            .MaximumLength(5)
            .NotEmpty();
        RuleFor(v => v.Title).
            MaximumLength(25);
    }
}
