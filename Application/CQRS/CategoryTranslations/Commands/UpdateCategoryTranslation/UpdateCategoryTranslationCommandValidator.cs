using FluentValidation;

namespace Application.CQRS.CategoryTranslations.Commands.UpdateCategoryTranslation;

public class UpdateCategoryTranslationCommandValidator : AbstractValidator<UpdateCategoryTranslationCommand>
{
    public UpdateCategoryTranslationCommandValidator() 
    {
        RuleFor(c => c.Title)
            .MaximumLength(200)
            .NotEmpty();
}
}
