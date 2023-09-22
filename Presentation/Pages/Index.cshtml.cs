using Application.Common.Models;
using Application.CQRS.CategoryTranslations.Queries.GetCategoryTranslationsByLanguageId;
using Application.CQRS.Languages.Queries.GetLanguageByCode;
using Application.CQRS.Posts.Queries.GetPostById;
using Application.CQRS.Posts.Queries.GetPublishedPosts;
using Application.CQRS.PostTranslations.Queries.GetPublishedPostTranslations;
using Application.CQRS.PostTranslations.Queries.GetRecentPostTranslations;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<CategoryTranslationDto> CategoryTranslations { get; set; }
        public List<PostTranslationDto> PostTranslations { get; set; }
        public List<PostTranslationDto> RecentPostTranslations { get; set; }
        public async Task OnGetAsync()
        {
            var defaultLanguage = await _mediator.Send(new GetLanguageByCodeQuery(DefaultStrings.DefaultLanguageCode));
            CategoryTranslations = await _mediator.Send(new GetCategoryTranslationsByLanguageIdQuery(defaultLanguage.Id));
            PostTranslations = (await _mediator.Send(new GetPublishedPostTranslationsQuery()))
                .Where(pt => pt.LanguageId == defaultLanguage.Id)
                .ToList();        
            RecentPostTranslations = await _mediator.Send(new GetRecentPostTranslationsQuery());
        }

        public async Task<string> GetPostTitleImageUrl(int postId)
        {
            var post = await _mediator.Send(new GetPostByIdQuery(postId));
            
            string imageUrl = post.TitleImageUrl;

            return imageUrl;
        }
    }
}