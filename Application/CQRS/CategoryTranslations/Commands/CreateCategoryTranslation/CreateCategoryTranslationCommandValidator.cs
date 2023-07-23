using FluentValidation;

namespace Application.CQRS.CategoryTranslations.Commands.CreateCategoryTranslation;

public class CreateCategoryTranslationCommandValidator : AbstractValidator<CreateCategoryTranslationCommand>
{
    public CreateCategoryTranslationCommandValidator()
    {
        RuleFor(c => c.Title)
            .MaximumLength(200)
            .NotEmpty();
    }
}
