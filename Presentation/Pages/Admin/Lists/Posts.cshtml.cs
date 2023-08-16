using Application.Common.Models;
using Application.CQRS.Posts.Commands.DeletePost;
using Application.CQRS.Posts.Queries.GetPosts;
using Application.CQRS.PostTranslations.Queries.GetPostTranslations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Admin.Lists
{
    [Authorize(Roles = "Admin")]
    public class PostsModel : PageModel
    {
        private readonly IMediator _mediator;

        public PostsModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<PostTranslationDto> Posts { get; set; }

        public async Task OnGetAsync()
        {
            Posts = await _mediator.Send(new GetPostTranslationsQuery());
        }

        public async Task<ActionResult> OnPostDeleteAsync(int Id)
        {
            try
            {
                DeletePostCommand deletePostCommand = new DeletePostCommand(Id);

                await _mediator.Send(deletePostCommand);
            }
            catch (Exception ex)
            {
                return new RedirectToPageResult("/Admin/Error", new { message = ex.Message, entityName = "Post" });
            }


            return new RedirectToPageResult("/Admin/Succeed", new { message = $"Entity with Id = {Id} and all related translations were successfully deleted", entityName = "Post" });
        }
    }
}
