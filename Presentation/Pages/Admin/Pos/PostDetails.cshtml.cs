using Application.Common.Models;
using Application.CQRS.Languages.Queries.GetLanguages;
using Application.CQRS.Posts.Queries.GetPostById;
using Application.CQRS.PostTranslations.Queries.GetPostTranslationsByPostId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Presentation.Constants;

namespace Presentation.Pages.Admin.Pos
{
    [Authorize(Roles = RoleAccessLevels.AllRoles)]
    public class PostDetailsModel : PageModel
    {
        private readonly IMediator _mediator;

        public PostDetailsModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<LanguageDto> Languages { get; set; }
        public List<PostTranslationDto> Translations { get; set; }
        public PostDto Post { get; set; }

        public async Task OnGetAsync(int id) //id = PostId
        {
            Languages = await _mediator.Send(new GetLanguagesQuery());
            Translations = await _mediator.Send(new GetPostTranslationsByPostIdQuery(id));
            Post = await _mediator.Send(new GetPostByIdQuery(id));
        }
    }
}
