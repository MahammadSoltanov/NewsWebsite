using Application.Common.Models;
using Application.CQRS.CategoryTranslations.Queries.GetCategoryTranslationsByLanguageId;
using Application.CQRS.Hashtags.Queries.GetHashtagsByPostId;
using Application.CQRS.Languages.Queries.GetLanguageByCode;
using Application.CQRS.Posts.Queries.GetPostById;
using Application.CQRS.PostTranslations.Commands.IncreaseViewCount;
using Application.CQRS.PostTranslations.Queries.GetPostTranslationById;
using Application.CQRS.Users.Queries.GetUserById;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages
{
    public class SingleModel : PageModel
    {
        private readonly IMediator _mediator;

        public SingleModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<CategoryTranslationDto> CategoryTranslations { get; set; }
        public UserDto Author { get; set; }
        public PostTranslationDto PostTranslation { get; set; }
        public PostDto Post{ get; set; }
        public List<HashtagDto> Hashtags { get; set; }
        public async Task OnGet(int id)
        {
            var defaultLanguage = await _mediator.Send(new GetLanguageByCodeQuery(DefaultStrings.DefaultLanguageCode));            
            CategoryTranslations = await _mediator.Send(new GetCategoryTranslationsByLanguageIdQuery(defaultLanguage.Id));
            PostTranslation = await _mediator.Send(new GetPostTranslationByIdQuery(id));
            Post = await _mediator.Send(new GetPostByIdQuery(PostTranslation.PostId));
            Author = await _mediator.Send(new GetUserByIdQuery(PostTranslation.AuthorId));
            Hashtags = await _mediator.Send(new GetHashtagsByPostIdQuery(Post.Id));

            bool hasViewedPost = Request.Cookies["ViewedPost_" + id] != null;

            if(!hasViewedPost) 
            {
                var increaseViewCountCommand = new IncreaseViewCountCommand(id);
                await _mediator.Send(increaseViewCountCommand);
                
                var options = new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddHours(24),
                };
                Response.Cookies.Append("ViewedPost_" + id, "true", options);
            }
        }
    }
}
