using Application.Common.Models;
using Application.CQRS.CategoryTranslations.Queries.GetCategoryTranslationsByLanguageId;
using Application.CQRS.Languages.Queries.GetLanguageByCode;
using Application.CQRS.Posts.Queries.GetPostById;
using Application.CQRS.Posts.Queries.GetPosts;
using Application.CQRS.PostTranslations.Queries.GetPostTranslations;
using Application.CQRS.PostTranslations.Queries.GetPostTranslationsByLanguageId;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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
        public List<PostDto> Posts { get; set; }
        public async Task OnGetAsync()
        {
            var defaultLanguage = await _mediator.Send(new GetLanguageByCodeQuery(DefaultStrings.DefaultLanguageCode));
            CategoryTranslations = await _mediator.Send(new GetCategoryTranslationsByLanguageIdQuery(defaultLanguage.Id));
            PostTranslations = await _mediator.Send(new GetPostTranslationsByLanguageIdQuery(defaultLanguage.Id));
            Posts = await _mediator.Send(new GetPostsQuery());
        }
    }
}