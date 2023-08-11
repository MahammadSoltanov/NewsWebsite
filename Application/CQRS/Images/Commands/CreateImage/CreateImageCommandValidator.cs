using FluentValidation;

namespace Application.CQRS.Images.Commands.CreateImage
{
    public class CreateImageCommandValidator : AbstractValidator<CreateImageCommand>
    {
        public CreateImageCommandValidator() 
        {
            RuleFor(i => i.Url)
                .NotEmpty();
        }
    }
}
