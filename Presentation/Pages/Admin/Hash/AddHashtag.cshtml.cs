using Application.CQRS.Hashtags.Commands.CreateHashtag;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Admin.Hash
{
    public class AddHashtagModel : PageModel
    {
        private readonly IMediator _mediator;

        public AddHashtagModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public string Title { get; set; }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            CreateHashtagCommand command = new CreateHashtagCommand()
            {
                Title = Title
            };

            int id = await _mediator.Send(command);

            string _message = $"Hashtag with Id = {id} was successfully created";

            return new RedirectToPageResult("/Admin/Succeed", new { message = _message, entityName = "Hashtag" });

        }
    }
}
