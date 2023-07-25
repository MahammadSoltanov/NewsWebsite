using Application.Common.Models;
using Application.CQRS.Posts.Queries.GetPosts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Admin.Lists
{
    public class PostsModel : PageModel
    {
        private readonly IMediator _mediator;

        public PostsModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<PostDto> Posts{ get; set; }

        public async Task OnGetAsync()
        {
            Posts = await _mediator.Send(new GetPostsQuery());
        }

        public async Task<ActionResult> OnPostDeleteAsync()
        {
            return Page();
        }
    }
}
