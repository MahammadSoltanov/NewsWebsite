using Application.Common.Models;
using Application.CQRS.Categories.Commands.DeleteCategory;
using Application.CQRS.Categories.Queries.GetCategories;
using Application.CQRS.CategoryTranslations.Queries.GetCategoryTranslations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Admin.Lists
{
    public class CategoriesModel : PageModel
    {
        private readonly IMediator _mediator;

        public CategoriesModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<CategoryTranslationDto> Categories { get; set; }

        public async Task OnGetAsync()
        {
            Categories = await _mediator.Send(new GetCategoryTranslationsQuery());
        }

        public async Task<ActionResult> OnPostDeleteAsync(int id)
        {
            DeleteCategoryCommand command = new DeleteCategoryCommand(id);

            try
            {
                await _mediator.Send(command);
            }
            catch (Exception ex)
            {
                return new RedirectToPageResult("/Admin/Error", new { message = ex.Message, entityName = "Categorie" });
            }

            string _message = $"Category with Id = {id} and all related translations were deleted";

            return new RedirectToPageResult("/Admin/Succeed", new { message = _message, entityName = "Categorie" });
        }
    }
}
