using FluentValidation;
namespace Application.CQRS.Categories.Commands.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(c => c.Description)
            .MaximumLength(200)
            .NotEmpty();
    }
}
