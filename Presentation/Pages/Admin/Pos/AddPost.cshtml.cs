using Application.Common.Models;
using Application.CQRS.CategoryTranslations.Queries.GetCategoryTranslationsByLanguageId;
using Application.CQRS.Languages.Queries.GetLanguageByCode;
using Application.CQRS.PostHashtags.Commands.AddPostHashtags;
using Application.CQRS.Posts.Commands.CreatePost;
using Application.CQRS.PostTranslations.Commands.CreatePostTranslation;
using Domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
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
    public class AddPostModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly UserManager<User> _userManager;
        private readonly IValidator<CreatePostCommand> _validatorPost;
        private readonly IValidator<CreatePostTranslationCommand> _validatorPostTranslation;

        public AddPostModel(IMediator mediator,
                IValidator<CreatePostCommand> validatorPost,
                IValidator<CreatePostTranslationCommand> validatorPostTranslation,
                UserManager<User> userManager)
        {
            _mediator = mediator;
            _validatorPost = validatorPost;
            _validatorPostTranslation = validatorPostTranslation;
            _userManager = userManager;
        }

        public List<CategoryTranslationDto> Categories { get; set; }
        public LanguageDto DefaultLanguage { get; set; }
        //Post info
        [BindProperty]
        public int CategoryId { get; set; }
        [BindProperty]
        public string TitleImageUrl { get; set; }

        //Translation info
        [BindProperty]
        public string Title { get; set; }
        [BindProperty]
        public int LanguageId { get; set; }
        [BindProperty]
        public string TranslationContent { get; set; }
        public async Task OnGetAsync()
        {
            await UpdateProperties();
        }

        public async Task<ActionResult> OnPostCreateAsync(List<string> tags)
        {
            List<string> tagsList = JsonStringToList(tags[0]);
            int postId;
            int postTranslationId;
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var currentUser = await _userManager.GetUserAsync(User);
                    CreatePostCommand createPostCommand = new CreatePostCommand()
                    {
                        CategoryId = CategoryId,
                        TitleImageUrl = TitleImageUrl,
                    };

                    ValidationResult resultPost = await _validatorPost.ValidateAsync(createPostCommand);

                    if (!resultPost.IsValid)
                    {
                        resultPost.AddToModelState(this.ModelState);
                        await UpdateProperties();
                        scope.Dispose();
                        return Page();
                    }

                    postId = await _mediator.Send(createPostCommand);

                    CreatePostTranslationCommand createPostTranslationCommand = new CreatePostTranslationCommand()
                    {
                        Title = Title,
                        PostId = postId,
                        Content = TranslationContent,
                        LanguageId = LanguageId,
                        AuthorId = currentUser.Id
                    };

                    ValidationResult resultPostTranslation = await _validatorPostTranslation.ValidateAsync(createPostTranslationCommand);

                    if (!resultPostTranslation.IsValid)
                    {
                        resultPostTranslation.AddToModelState(this.ModelState);
                        await UpdateProperties();
                        scope.Dispose();
                        return Page();
                    }

                    postTranslationId = await _mediator.Send(createPostTranslationCommand);

                    if (tagsList.Count > 0)
                    {
                        AddPostHashtagsCommand addHashtagsCommand = new AddPostHashtagsCommand()
                        {
                            PostId = postId,
                            Tags = tagsList
                        };

                        await _mediator.Send(addHashtagsCommand);
                    }
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    Log.Error(ex, "An unexpected error occurred on post creation" + ex.StackTrace);
                    return new RedirectToPageResult("/Admin/Error", new { message = "Something went wrong during the operation. Please try again or contact the support team.", entityName = "Posts" });
                }

                scope.Complete();
            }

            DefaultLanguage = await _mediator.Send(new GetLanguageByCodeQuery(DefaultStrings.DefaultLanguageCode));
            string _message = $"Post with Id = {postId} and " +
                $"default {DefaultLanguage.Code} translation with " +
                $"Id = {postTranslationId} were successfully created";
            return new RedirectToPageResult("/Admin/Succeed", new { message = _message, entityName = "Posts" });
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

        private async Task UpdateProperties()
        {
            DefaultLanguage = await _mediator.Send(new GetLanguageByCodeQuery(DefaultStrings.DefaultLanguageCode));
            LanguageId = DefaultLanguage.Id;
            Categories = await _mediator.Send(new GetCategoryTranslationsByLanguageIdQuery(LanguageId));
        }
    }
}
