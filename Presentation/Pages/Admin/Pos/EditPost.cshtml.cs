using Application.Common.Models;
using Application.CQRS.CategoryTranslations.Queries.GetCategoryTranslationsByLanguageId;
using Application.CQRS.Hashtags.Queries.GetHashtagsByPostId;
using Application.CQRS.Languages.Queries.GetLanguage;
using Application.CQRS.Languages.Queries.GetLanguageByCode;
using Application.CQRS.Languages.Queries.GetLanguages;
using Application.CQRS.PostHashtags.Commands.UpdatePostHashtags;
using Application.CQRS.Posts.Commands.UpdatePost;
using Application.CQRS.Posts.Queries.GetPostById;
using Application.CQRS.PostTranslations.Commands.UpdatePostTranslation;
using Application.CQRS.PostTranslations.Queries.GetPostTranslationsByPostId;
using Domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Presentation.Constants;
using Serilog;
using System.Transactions;

namespace Presentation.Pages.Admin.Pos
{
    [Authorize(Roles = RoleAccessLevels.AllRoles)]
    public class EditPostModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly UserManager<User> _userManager;
        private readonly IValidator<UpdatePostTranslationCommand> _validatorTranslation;
        private readonly IValidator<UpdatePostCommand> _validatorPost;

        public EditPostModel(IMediator mediator, UserManager<User> userManager, IValidator<UpdatePostTranslationCommand> validatorTranslation, IValidator<UpdatePostCommand> validatorPost)
        {
            _mediator = mediator;
            _userManager = userManager;
            _validatorTranslation = validatorTranslation;
            _validatorPost = validatorPost;
        }

        //Lists
        public List<LanguageDto> Languages { get; set; }
        public List<CategoryTranslationDto> Categories { get; set; }
        public LanguageDto DefaultLanguage { get; set; }
        public List<string> Tags { get; set; }

        //Post info
        [BindProperty]
        public int CategoryId { get; set; }
        [BindProperty]
        public int PostId { get; set; }
        [BindProperty]
        public string TitleImageUrl { get; set; }

        //Translation info
        [BindProperty]
        public int TranslationId { get; set; }
        [BindProperty]
        public string Title { get; set; }
        [BindProperty]
        public int LanguageId { get; set; }
        [BindProperty]
        public string TranslationContent { get; set; }


        public async Task OnGetAsync(int id)
        {
            var defaultLanguage = await _mediator.Send(new GetLanguageByCodeQuery(DefaultStrings.DefaultLanguageCode));
            await UpdateFormProperties(id, defaultLanguage.Id);
        }

        public async Task<IActionResult> OnPostLanguageAsync()
        {
            await UpdateFormProperties(PostId, LanguageId);
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateAsync(List<string> tags)
        {
            List<string> tagsList = JsonStringToList(tags[0]);
            string _message;
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    /*---Update Post---*/
                    UpdatePostCommand updatePostCommand = new UpdatePostCommand()
                    {
                        Id = PostId,
                        TitleImageUrl = TitleImageUrl,
                        CategoryId = CategoryId,
                    };

                    var postResult = await _validatorPost.ValidateAsync(updatePostCommand);

                    if (!postResult.IsValid)
                    {
                        postResult.AddToModelState(this.ModelState);
                        await UpdateFormProperties(PostId, LanguageId);
                        scope.Dispose();
                        return Page();
                    }

                    await _mediator.Send(updatePostCommand);

                    /*---Update Post Translation---*/
                    var currentUser = await _userManager.GetUserAsync(User);
                    UpdatePostTranslationCommand updatePostTranslationCommand = new UpdatePostTranslationCommand()
                    {
                        TranslationContent = TranslationContent,
                        Title = Title,
                        AuthorId = currentUser.Id,
                        LanguageId = LanguageId,
                        PostId = PostId,
                    };

                    var postTranslationResult = await _validatorTranslation.ValidateAsync(updatePostTranslationCommand);

                    if (!postTranslationResult.IsValid)
                    {
                        postTranslationResult.AddToModelState(this.ModelState);
                        await UpdateFormProperties(PostId, LanguageId);
                        scope.Dispose();
                        return Page();
                    }

                    _message = await _mediator.Send(updatePostTranslationCommand);

                    if (tagsList.Count > 0)
                    {
                        UpdatePostHashtagsCommand updatePostHashtagsCommand = new UpdatePostHashtagsCommand()
                        {
                            PostId = PostId,
                            Tags = tagsList
                        };

                        await _mediator.Send(updatePostHashtagsCommand);
                    }
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    Log.Error(ex, "Something went wrong on editing post information\n" + ex.StackTrace);
                    return new RedirectToPageResult("/Admin/Error", new { message = "Something went wrong during the operation. Please try again or contact the support team", entityName = "Posts" });
                }

                scope.Complete();
            }

            return new RedirectToPageResult("/Admin/Succeed", new { message = _message, entityName = "Posts" });
        }

        private async Task UpdateFormProperties(int id, int languageId)
        {
            PostDto post = await _mediator.Send(new GetPostByIdQuery(id));

            DefaultLanguage = await _mediator.Send(new GetLanguageByIdQuery(languageId));

            var translations = await _mediator.Send(new GetPostTranslationsByPostIdQuery(post.Id));
            var postTranslation = translations.FirstOrDefault(pt => pt.LanguageId == languageId);

            if (postTranslation == null)
            {
                postTranslation = new PostTranslationDto()
                {
                    LanguageId = languageId,
                };
            }

            Categories = await _mediator.Send(new GetCategoryTranslationsByLanguageIdQuery(languageId));
            Languages = await _mediator.Send(new GetLanguagesQuery());
            var Hashtags = await _mediator.Send(new GetHashtagsByPostIdQuery(post.Id));
            Tags = new List<string>();

            foreach (var hashtag in Hashtags)
            {
                Tags.Add(hashtag.Title);
            }

            PostId = post.Id;
            CategoryId = post.CategoryId;
            TitleImageUrl = post.TitleImageUrl;

            Title = postTranslation.Title;
            LanguageId = postTranslation.LanguageId;
            TranslationContent = postTranslation.Content;
        }

        private List<string> JsonStringToList(string json)
        {
            List<string> rawTags = JsonConvert.DeserializeObject<List<string>>(json);

            // Clean and store tags without "×"
            List<string> cleanedTags = new List<string>();
            foreach (string rawTag in rawTags)
            {
                string cleanedTag = rawTag.TrimEnd('×');
                cleanedTags.Add(cleanedTag);
            }

            return cleanedTags;
        }
    }
}
