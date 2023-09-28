using Application.Common.Models;
using Application.CQRS.Hashtags.Commands.DeleteHashtag;
using Application.CQRS.Hashtags.Queries.GetHashtags;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Admin.Lists
{
    [Authorize(Roles = "Admin, Moderator, Journalist")]
    public class HashtagsModel : PageModel
    {
        private readonly IMediator _mediator;

        public HashtagsModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<HashtagDto> Hashtags { get; set; }
        public async Task OnGetAsync()
        {
            Hashtags = await _mediator.Send(new GetHashtagsQuery());
        }

        public async Task<IActionResult> OnPostDeleteAsync(int Id)
        {
            await _mediator.Send(new DeleteHashtagCommand(Id));
            string _message = $"Hashtag with Id = {Id} was successfully deleted";
            
            return new RedirectToPageResult("/Admin/Succeed", new {message = _message, entityName = "Hashtags"});
        }
    }
}
