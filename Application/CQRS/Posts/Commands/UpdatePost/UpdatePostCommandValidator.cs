using FluentValidation;

namespace Application.CQRS.Posts.Commands.UpdatePost;

public class UpdatePostCommandValidator : AbstractValidator<UpdatePostCommand>
{
    public UpdatePostCommandValidator()
    {
        RuleFor(p => p.CategoryId).NotEmpty();        
    }
}
