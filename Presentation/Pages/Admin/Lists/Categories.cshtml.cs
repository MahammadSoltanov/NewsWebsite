using Application.Common.Models;
using Application.CQRS.Categories.Queries.GetCategories;
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

        public List<CategoryDto> Categories { get; set; }

        public async Task OnGetAsync()
        {
            Categories = await _mediator.Send(new GetCategoriesQuery());   
        }
    }
}
