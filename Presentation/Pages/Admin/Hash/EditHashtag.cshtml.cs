using Application.Common.Models;
using Application.CQRS.Hashtags.Commands.UpdateHashtag;
using Application.CQRS.Hashtags.Queries.GetHashtagById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Admin.Hash
{
    public class EditHashtagModel : PageModel
    {
        private readonly IMediator _mediator;

        public EditHashtagModel(IMediator mediator)
        {
            _mediator = mediator;
        }
        [BindProperty]
        public HashtagDto Hashtag { get; set; }
        
        public async Task OnGetAsync(int Id)
        {
            Hashtag = await _mediator.Send(new GetHashtagByIdQuery(Id));
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            UpdateHashtagCommand command = new UpdateHashtagCommand()
            {
                Id = Hashtag.Id,
                Title = Hashtag.Title
            };

            await _mediator.Send(command);
            string _message = $"Hashtag with Id = {command.Id} was updated successfully";
            return new RedirectToPageResult("/Admin/Succeed", new {message = _message, entityName = "Hashtag"});
        }
    }
}
