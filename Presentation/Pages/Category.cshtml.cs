using Application.Common.Models;
using Application.CQRS.CategoryTranslations.Queries.GetCategoryTranslationByCategoryId;
using Application.CQRS.CategoryTranslations.Queries.GetCategoryTranslationsByLanguageId;
using Application.CQRS.Languages.Queries.GetLanguageByCode;
using Application.CQRS.Posts.Queries.GetPostsByCategoryId;
using Application.CQRS.PostTranslations.Queries.GetPostTranslations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages
{
    public class CategoryModel : PageModel
    {
        private readonly IMediator _mediator;

        public CategoryModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<CategoryTranslationDto> CategoryTranslations { get; set; }
        public List<PostTranslationDto> PostTranslations { get; set; }
        public List<PostDto> Posts { get; set; }
        public CategoryTranslationDto Category { get; set; }
        public async Task OnGetAsync(int id)
        {
            var defaultLanguage = await _mediator.Send(new GetLanguageByCodeQuery(DefaultStrings.DefaultLanguageCode));
            CategoryTranslations = await _mediator.Send(new GetCategoryTranslationsByLanguageIdQuery(defaultLanguage.Id));
            Category = CategoryTranslations.FirstOrDefault(ct => ct.CategoryId == id);
            Posts = await _mediator.Send(new GetPostsByCategoryIdQuery(id));
            var translations = await _mediator.Send(new GetPostTranslationsQuery());

            PostTranslations = new List<PostTranslationDto>();
            
            foreach(var translation in translations)
            {
                foreach (var post in Posts)
                {

                    if(post.Id == translation.PostId && translation.LanguageId == defaultLanguage.Id)
                    {
                        PostTranslations.Add(translation);
                    }
                }
            }

        }
    }
}