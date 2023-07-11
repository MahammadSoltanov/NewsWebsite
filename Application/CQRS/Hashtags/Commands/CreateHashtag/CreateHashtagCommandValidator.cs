using FluentValidation;

namespace Application.CQRS.Hashtags.Commands.CreateHashtag;

public class CreateHashtagCommandValidator : AbstractValidator<CreateHashtagCommand>
{
    public CreateHashtagCommandValidator()
    {
        RuleFor(v => v.Title)
            .MaximumLength(50)
            .NotEmpty();
    }
}
