using FluentValidation;

namespace Application.CQRS.Hashtags.Commands.UpdateHashtag;

public class UpdateHashtagCommandValidator : AbstractValidator<UpdateHashtagCommand>
{
    public UpdateHashtagCommandValidator()
    {
        RuleFor(v => v.Title)
            .MaximumLength(50)
            .NotEmpty();
    }
}
