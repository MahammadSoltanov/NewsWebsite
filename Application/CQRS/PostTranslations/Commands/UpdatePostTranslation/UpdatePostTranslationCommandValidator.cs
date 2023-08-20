using FluentValidation;

namespace Application.CQRS.PostTranslations.Commands.UpdatePostTranslation;

public class UpdatePostTranslationCommandValidator : AbstractValidator<UpdatePostTranslationCommand>
{
    public UpdatePostTranslationCommandValidator()
    {
        RuleFor(pt => pt.Title)
        .MaximumLength(200)
        .NotEmpty();
        RuleFor(pt => pt.TranslationContent)
            .NotEmpty();
        RuleFor(pt => pt.LanguageId)
            .NotEmpty();
        RuleFor(pt => pt.PostId)
            .NotEmpty();
        RuleFor(pt => pt.AuthorId)
            .NotEmpty();
    }
}
