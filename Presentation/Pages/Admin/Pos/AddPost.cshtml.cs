using Application.Common.Models;
using Application.CQRS.CategoryTranslations.Queries.GetCategoryTranslationsByLanguageId;
using Application.CQRS.Languages.Queries.GetLanguageByCode;
using Application.CQRS.Languages.Queries.GetLanguages;
using Application.CQRS.Posts.Commands.CreatePost;
using Application.CQRS.PostTranslations.Commands.CreatePostTranslation;
using Application.CQRS.Users.Queries.GetUsers;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Transactions;

namespace Presentation.Pages.Admin.Pos
{
    public class AddPostModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IValidator<CreatePostCommand> _validatorPost;
        private readonly IValidator<CreatePostTranslationCommand> _validatorPostTranslation;

        public AddPostModel(IMediator mediator, IValidator<CreatePostCommand> validatorPost, IValidator<CreatePostTranslationCommand> validatorPostTranslation)
        {
            _mediator = mediator;
            _validatorPost = validatorPost;
            _validatorPostTranslation = validatorPostTranslation;
        }

        public List<UserDto> Users { get; set; }
        public List<CategoryTranslationDto> Categories{ get; set; }
        public LanguageDto DefaultLanguage { get; set; }
        //Post info
        [BindProperty]
        public int CategoryId { get; set; }
        
        //Translation info
        [BindProperty]
        public string Title { get; set; }
        [BindProperty]
        public int AuthorId { get; set; }
        [BindProperty]
        public int LanguageId { get; set; }
        [BindProperty]
        public string TranslationContent { get; set; }
        public async Task OnGetAsync()
        {
            Users = await _mediator.Send(new GetUsersQuery());
            DefaultLanguage = await _mediator.Send(new GetLanguageByCodeQuery(DefaultStrings.DefaultLanguageCode));
            LanguageId = DefaultLanguage.Id;
            Categories = await _mediator.Send(new GetCategoryTranslationsByLanguageIdQuery(LanguageId));
        }

        public async Task<ActionResult> OnPostCreateAsync()
        {
            int postId;
            int postTranslationId;
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    CreatePostCommand createPostCommand = new CreatePostCommand()
                    {
                        CategoryId = CategoryId,
                    };

                    ValidationResult resultPost = await _validatorPost.ValidateAsync(createPostCommand);

                    if(!resultPost.IsValid)
                    {
                        resultPost.AddToModelState(this.ModelState);
                        Users = await _mediator.Send(new GetUsersQuery());
                        DefaultLanguage = await _mediator.Send(new GetLanguageByCodeQuery(DefaultStrings.DefaultLanguageCode));
                        LanguageId = DefaultLanguage.Id;
                        Categories = await _mediator.Send(new GetCategoryTranslationsByLanguageIdQuery(LanguageId));
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
                        AuthorId = AuthorId,
                    };

                    ValidationResult resultPostTranslation = await _validatorPostTranslation.ValidateAsync(createPostTranslationCommand);

                    if(!resultPostTranslation.IsValid) 
                    {
                        resultPostTranslation.AddToModelState(this.ModelState);
                        Users = await _mediator.Send(new GetUsersQuery());
                        DefaultLanguage = await _mediator.Send(new GetLanguageByCodeQuery(DefaultStrings.DefaultLanguageCode));
                        LanguageId = DefaultLanguage.Id;
                        Categories = await _mediator.Send(new GetCategoryTranslationsByLanguageIdQuery(LanguageId));
                        scope.Dispose();
                        return Page();
                    }

                    postTranslationId = await _mediator.Send(createPostTranslationCommand);
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    return new RedirectToPageResult("/Admin/Error", new { message = ex.Message, entityName = "Post" });
                }

                scope.Complete();
            }
            DefaultLanguage = await _mediator.Send(new GetLanguageByCodeQuery(DefaultStrings.DefaultLanguageCode));
            string _message = $"Post with Id = {postId} and " +
                $"default {DefaultLanguage.Code} translation with " +
                $"Id = {postTranslationId} were successfully created";
            return new RedirectToPageResult("/Admin/Succeed", new { message = _message, entityName = "Post" });
        }
    }
}
