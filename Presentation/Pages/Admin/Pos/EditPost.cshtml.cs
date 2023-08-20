using Application.Common.Models;
using Application.CQRS.CategoryTranslations.Queries.GetCategoryTranslationsByLanguageId;
using Application.CQRS.Hashtags.Queries.GetHashtags;
using Application.CQRS.Languages.Queries.GetLanguageByCode;
using Application.CQRS.Languages.Queries.GetLanguages;
using Application.CQRS.Posts.Commands.UpdatePost;
using Application.CQRS.Posts.Queries.GetPostById;
using Application.CQRS.PostTranslations.Commands.UpdatePostTranslation;
using Application.CQRS.PostTranslations.Queries.GetPostTranslationById;
using Domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Transactions;

namespace Presentation.Pages.Admin.Pos
{
    [Authorize(Roles = "Admin")]
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
        public List<HashtagDto> Hashtags { get; set; }

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
            await UpdateFormProperties(id);
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
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
                        await UpdateFormProperties(TranslationId);
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
                        await UpdateFormProperties(TranslationId);
                        scope.Dispose();
                        return Page();
                    }

                    _message = await _mediator.Send(updatePostTranslationCommand);
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    return new RedirectToPageResult("/Admin/Error", new { message = ex.Message, entityName = "Post" });
                }

                scope.Complete();
            }

            return new RedirectToPageResult("/Admin/Succeed", new { message = _message, entityName = "Post" });
        }

        public async Task UpdateFormProperties(int id)
        {
            PostTranslationDto postTranslation = await _mediator.Send(new GetPostTranslationByIdQuery(id));
            PostDto post = await _mediator.Send(new GetPostByIdQuery(postTranslation.PostId));

            DefaultLanguage = await _mediator.Send(new GetLanguageByCodeQuery(DefaultStrings.DefaultLanguageCode));
            LanguageId = DefaultLanguage.Id;
            Categories = await _mediator.Send(new GetCategoryTranslationsByLanguageIdQuery(LanguageId));
            Languages = await _mediator.Send(new GetLanguagesQuery());
            Hashtags = await _mediator.Send(new GetHashtagsQuery());

            PostId = post.Id;
            CategoryId = post.CategoryId;
            TitleImageUrl = post.TitleImageUrl;

            TranslationId = postTranslation.Id;
            Title = postTranslation.Title;
            LanguageId = postTranslation.LanguageId;
            TranslationContent = postTranslation.Content;
        }
    }
}
