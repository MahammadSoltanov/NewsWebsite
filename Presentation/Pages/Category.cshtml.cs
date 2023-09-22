using Application.Common.Models;

using Application.CQRS.CategoryTranslations.Queries.GetCategoryTranslationByCategoryId;
using Application.CQRS.CategoryTranslations.Queries.GetCategoryTranslationsByLanguageId;
using Application.CQRS.Languages.Queries.GetLanguageByCode;
using Application.CQRS.Posts.Queries.GetPostById;
using Application.CQRS.Posts.Queries.GetPostsByCategoryId;
using Application.CQRS.Posts.Queries.GetPublishedPosts;
using Application.CQRS.PostTranslations.Queries.GetPostTranslations;
using Application.CQRS.PostTranslations.Queries.GetPublishedPostTranslations;
using Application.CQRS.PostTranslations.Queries.GetRecentPostTranslations;
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
        public List<PostTranslationDto> RecentPostTranslations { get; set; }
        public List<PostDto> Posts { get; set; }
        public CategoryTranslationDto Category { get; set; }
        public async Task OnGetAsync(int id)
        {
            var defaultLanguage = await _mediator.Send(new GetLanguageByCodeQuery(DefaultStrings.DefaultLanguageCode));
            
            CategoryTranslations = await _mediator.Send(new GetCategoryTranslationsByLanguageIdQuery(defaultLanguage.Id));
            Category = CategoryTranslations.FirstOrDefault(ct => ct.CategoryId == id);
            
            var posts = await _mediator.Send(new GetPublishedPostsQuery());
            Posts = posts.Where(p => p.CategoryId == Category.CategoryId).ToList();
            
            var translations = await _mediator.Send(new GetPublishedPostTranslationsQuery());

            PostTranslations = new List<PostTranslationDto>();
            RecentPostTranslations = await _mediator.Send(new GetRecentPostTranslationsQuery());

            foreach (var translation in translations)
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

        public async Task<string> GetPostTitleImageUrl(int postId)
        {
            var post = await _mediator.Send(new GetPostByIdQuery(postId));

            string imageUrl = post.TitleImageUrl;

            return imageUrl;
        }
    }
}