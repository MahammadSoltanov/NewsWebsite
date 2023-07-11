using Application.Common.Models;
using Application.CQRS.CategoryTranslations.Queries.GetCategoryTranslationByCategoryId;
using Application.CQRS.Languages.Queries.GetLanguages;
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
        public List<CategoryTranslationDto> Translations { get; set; }

        public async Task OnGetAsync()
        {
            Languages = await _mediator.Send(new GetLanguagesQuery());

            //Creating translationDto objects in number of languages in db
            Translations = Languages.Select(language => new CategoryTranslationDto()).ToList(); 
        }
    }
}
