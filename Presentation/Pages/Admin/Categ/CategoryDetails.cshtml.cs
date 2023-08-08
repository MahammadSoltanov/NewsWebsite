using Application.Common.Models;
using Application.CQRS.CategoryTranslations.Queries.GetCategoryTranslationByCategoryId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Admin.Categ
{
    [Authorize(Roles = "Admin")]
    public class CategoryDetailsModel : PageModel
    {
        private readonly IMediator _mediator;

        public CategoryDetailsModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<CategoryTranslationDto> Translations{ get; set; }
        public int CategoryId { get; set; }

        public async Task OnGetAsync(int id)
        {
            Translations = await _mediator.Send(new GetCategoryTranslationsByCategoryIdQuery(id));
            CategoryId = id;
        }
    }
}
