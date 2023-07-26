using FluentValidation;

namespace Application.CQRS.Posts.Commands.CreatePost;

public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostCommandValidator()
    {
        RuleFor(p => p.CategoryId)
            .NotEmpty();
    }
}
