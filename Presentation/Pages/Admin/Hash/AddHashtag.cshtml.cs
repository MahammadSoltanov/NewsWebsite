using Application.CQRS.Hashtags.Commands.CreateHashtag;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Admin.Hash
{
    [Authorize(Roles = "Admin, Moderator, Journalist")]
    public class AddHashtagModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IValidator<CreateHashtagCommand> _validator;

        public AddHashtagModel(IMediator mediator, IValidator<CreateHashtagCommand> validator)
        {
            _validator = validator;
            _mediator = mediator;
        }

        [BindProperty]
        public string Title { get; set; }

        public async Task<ActionResult> OnPostCreateAsync()
        {
            CreateHashtagCommand command = new CreateHashtagCommand()
            {
                Title = Title
            };

            ValidationResult result = await _validator.ValidateAsync(command);

            if(!result.IsValid) 
            {
                result.AddToModelState(this.ModelState);

                return Page();
            }

            int id = await _mediator.Send(command);

            string _message = $"Hashtag with Id = {id} was successfully created";

            return new RedirectToPageResult("/Admin/Succeed", new { message = _message, entityName = "Hashtag" });

        }
    }
}
