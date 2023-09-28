using Application.Common.Models;
using Application.CQRS.CategoryTranslations.Commands.UpdateCategoryTranslation;
using Application.CQRS.CategoryTranslations.Queries.GetCategoryTranslationByCategoryId;
using Application.CQRS.CategoryTranslations.Queries.GetCategoryTranslationById;
using Application.CQRS.Languages.Queries.GetLanguages;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;

namespace Presentation.Pages.Admin.Categ
{
    [Authorize(Roles = "Admin, Moderator, Journalist")]
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
        public int Id { get; set; }
        [BindProperty]
        public List<LanguageDto> Languages { get; set; }

        public async Task OnGetAsync(int id)
        {
            await UpdateProperties(id);
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
                    await UpdateProperties(Id);
                    return Page();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Something went wrong on updating category information\n" + ex.StackTrace);
                return new RedirectToPageResult("/Admin/Error", new { message = "Something went wrong during the operation, please try again or contact the support team.", entityName = "Categories" });
            }

            string _message = await _mediator.Send(updateCategoryTranslationCommand);
            return new RedirectToPageResult("/Admin/Succeed", new { message = _message, entityName = "Categories" });
        }

        private async Task<IActionResult> UpdateProperties(int id)
        {
            try
            {
                var categoryTranslation = await _mediator.Send(new GetCategoryTranslationByIdQuery(id));
                CategoryId = categoryTranslation.CategoryId;
                LanguageId = categoryTranslation.LanguageId;
                Title = categoryTranslation.Title;
                Id = id;

                Languages = await _mediator.Send(new GetLanguagesQuery());
            }
            catch (Exception ex) 
            {
                Log.Error(ex, "Something went wrong on updating page properties\n" + ex.StackTrace);
                return new RedirectToPageResult("/Admin/Error", new { message = "Something went wrong during the operation, please try again or contact the support team.", entityName = "Categories" });
            }

            return Page();
        }
    }
}
