using Application.Common.Models;
using Application.CQRS.CategoryTranslations.Commands.UpdateCategoryTranslation;
using Application.CQRS.Languages.Queries.GetLanguages;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Admin.Categ
{
    public class EditCategoryModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IValidator<UpdateCategoryTranslationCommand> _validator;

        public EditCategoryModel(IMediator mediator, IValidator<UpdateCategoryTranslationCommand> validator)
        {
            _mediator = mediator;
            _validator = validator;
        }

        [BindProperty]
        public string Title { get; set; }
        [BindProperty]
        public int LanguageId { get; set; }
        [BindProperty]
        public int CategoryId { get; set; }
        [BindProperty]
        public List<LanguageDto> Languages { get; set; }

        public async Task OnGetAsync(int id)
        {
            CategoryId = id;
            Languages = await _mediator.Send(new GetLanguagesQuery());
        }

        public async Task<ActionResult> OnPostUpdateAsync()
        {
            UpdateCategoryTranslationCommand updateCategoryTranslationCommand;
            try
            {
                updateCategoryTranslationCommand = new UpdateCategoryTranslationCommand()
                {
                    Title = Title,
                    LanguageId = LanguageId,
                    CategoryId = CategoryId
                };

                ValidationResult result = await _validator.ValidateAsync(updateCategoryTranslationCommand);
                
                if (!result.IsValid)
                {
                    result.AddToModelState(this.ModelState);
                    return Page();
                }
            }
            catch (Exception ex)
            {
                return new RedirectToPageResult("/Admin/Error", new { message = ex.Message, entityName = "Categorie" });
            }

            string _message = await _mediator.Send(updateCategoryTranslationCommand);
            return new RedirectToPageResult("/Admin/Succeed", new { message = _message, entityName = "Categorie" });
        }
    }
}
