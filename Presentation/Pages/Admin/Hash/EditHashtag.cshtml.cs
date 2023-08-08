using Application.Common.Models;
using Application.CQRS.Hashtags.Commands.UpdateHashtag;
using Application.CQRS.Hashtags.Queries.GetHashtagById;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Admin.Hash
{
    [Authorize(Roles = "Admin")]
    public class EditHashtagModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IValidator<UpdateHashtagCommand> _validator;

        public EditHashtagModel(IMediator mediator, IValidator<UpdateHashtagCommand> validator)
        {
            _mediator = mediator;
            _validator = validator;
        }
        [BindProperty]
        public int Id { get; set; }
        [BindProperty]
        public string Title { get; set; }

        
        public async Task OnGetAsync(int id)
        {
            HashtagDto hashtag = await _mediator.Send(new GetHashtagByIdQuery(id));
        }

        public async Task<ActionResult> OnPostUpdateAsync()
        {
            UpdateHashtagCommand command = new UpdateHashtagCommand()
            {
                Id = Id,
                Title = Title
            };

            ValidationResult result = await _validator.ValidateAsync(command);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);

                return Page();
            }

            await _mediator.Send(command);
            string _message = $"Hashtag with Id = {command.Id} was updated successfully";
            return new RedirectToPageResult("/Admin/Succeed", new {message = _message, entityName = "Hashtag"});
        }
    }
}
