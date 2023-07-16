using Application.Common.Models;
using Application.CQRS.Categories.Commands.CreateCategory;
using Application.CQRS.CategoryTranslations.Queries.GetCategoryTranslationByCategoryId;
using Application.CQRS.Languages.Queries.GetLanguages;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Admin.Categ
{
    public class AddCategoryModel : PageModel
    {
        private readonly IMediator _mediator;
        public AddCategoryModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<LanguageDto> Languages { get; set; }
        [BindProperty]
        public string Description { get; set; }
        [BindProperty]
        public string TranslationTitle { get; set; }
        [BindProperty]
        public string TranslationStatus { get; set; }
        [BindProperty]
        public int TranslationLanguageId { get; set; }

        
        public async Task OnGetAsync()
        {
            Languages = await _mediator.Send(new GetLanguagesQuery());
        }

        public async Task<ActionResult> OnPostCreateAsync()
        {
            CreateCategoryCommand command = new CreateCategoryCommand()
            {
                Description = Description,
            };

            int id = await _mediator.Send(command);


            return Page();
        }
    }
}
