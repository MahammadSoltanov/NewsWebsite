using Application.Common.Models;
using Application.CQRS.Categories.Commands.CreateCategory;
using Application.CQRS.CategoryTranslations.Commands.CreateCategoryTranslation;
using Application.CQRS.Languages.Queries.GetLanguageByCode;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Presentation.Constants;
using Serilog;
using System.Transactions;

namespace Presentation.Pages.Admin.Categ
{
    [Authorize(Roles = RoleAccessLevels.AdminAndModerator)]
    public class AddCategoryModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IValidator<CreateCategoryCommand> _validatorCategory;
        private readonly IValidator<CreateCategoryTranslationCommand> _validatorCategoryTranslation;

        public AddCategoryModel(IMediator mediator, IValidator<CreateCategoryCommand> validatorCategory, IValidator<CreateCategoryTranslationCommand> validatorCategoryTranslation)
        {
            _mediator = mediator;
            _validatorCategory = validatorCategory;
            _validatorCategoryTranslation = validatorCategoryTranslation;
        }

        [BindProperty]
        public LanguageDto DefaultLanguage { get; set; }
        [BindProperty]
        public string TranslationTitle { get; set; }
        [BindProperty]
        public int LanguageId { get; set; }

        public async Task OnGetAsync()
        {
            DefaultLanguage = await _mediator.Send(new GetLanguageByCodeQuery(DefaultStrings.DefaultLanguageCode));
            LanguageId = DefaultLanguage.Id;
        }

        public async Task<ActionResult> OnPostCreateAsync()
        {
            int id;
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    CreateCategoryCommand createCategoryCommand = new CreateCategoryCommand();

                    ValidationResult resultCategory = await _validatorCategory.ValidateAsync(createCategoryCommand);

                    if (!resultCategory.IsValid)
                    {
                        resultCategory.AddToModelState(this.ModelState);
                        DefaultLanguage = await _mediator.Send(new GetLanguageByCodeQuery(DefaultStrings.DefaultLanguageCode));
                        LanguageId = DefaultLanguage.Id;
                        scope.Dispose();
                        return Page();
                    }

                    id = await _mediator.Send(createCategoryCommand);

                    CreateCategoryTranslationCommand createCategoryTranslationCommand = new CreateCategoryTranslationCommand()
                    {
                        Title = TranslationTitle,
                        CategoryId = id,
                        LanguageId = LanguageId
                    };

                    await _mediator.Send(createCategoryTranslationCommand);
                    ValidationResult resultTranslation = await _validatorCategoryTranslation.ValidateAsync(createCategoryTranslationCommand);

                    if (!resultTranslation.IsValid)
                    {
                        resultCategory.AddToModelState(this.ModelState);
                        DefaultLanguage = await _mediator.Send(new GetLanguageByCodeQuery(DefaultStrings.DefaultLanguageCode));
                        LanguageId = DefaultLanguage.Id;
                        scope.Dispose();
                        return Page();
                    }
                }

                catch (Exception ex)
                {
                    scope.Dispose();
                    Log.Error(ex, "Something went wrong on category creation\n" + ex.StackTrace);
                    return new RedirectToPageResult("/Admin/Error", new { message = "Something went wrong during the operation, please try again or contact the support team.", entityName = "Categories" });
                }

                scope.Complete();
            }

            string _message = $"Category with Id = {id} was successfully created with default {DefaultLanguage.Title} transaltion";
            return new RedirectToPageResult("/Admin/Succeed", new { message = _message, entityName = "Categories" });
        }
    }
}
