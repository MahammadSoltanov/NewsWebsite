using FluentValidation;

namespace Application.CQRS.PostTranslations.Commands.CreatePostTranslation;

public class CreatePostTranslationCommandValidator : AbstractValidator<CreatePostTranslationCommand>
{
    public CreatePostTranslationCommandValidator()
    {
        RuleFor(pt => pt.Title)
            .MaximumLength(200)
            .NotEmpty();
    }
}
