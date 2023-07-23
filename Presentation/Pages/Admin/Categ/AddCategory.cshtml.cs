using Application.Common.Interfaces;
using Application.Common.Models;
using Application.CQRS.Categories.Commands.CreateCategory;
using Application.CQRS.CategoryTranslations.Commands.CreateCategoryTranslation;
using Application.CQRS.CategoryTranslations.Queries.GetCategoryTranslationByCategoryId;
using Application.CQRS.Languages.Queries.GetLanguageByCode;
using Application.CQRS.Languages.Queries.GetLanguages;
using Domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Transactions;

namespace Presentation.Pages.Admin.Categ
{
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
                    return new RedirectToPageResult("/Admin/Error", new {message =  ex.Message, entityName = "Categorie"});
                }

                scope.Complete();
            }

            string _message = $"Category with Id = {id} was successfully created with default {DefaultLanguage.Code} transaltion";
            return new RedirectToPageResult("/Admin/Succeed", new { message = _message, entityName = "Categorie" });
        }
    }
}
