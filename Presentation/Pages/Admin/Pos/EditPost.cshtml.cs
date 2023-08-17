using Application.Common.Models;
using Application.CQRS.CategoryTranslations.Queries.GetCategoryTranslationsByLanguageId;
using Application.CQRS.Hashtags.Queries.GetHashtags;
using Application.CQRS.Languages.Queries.GetLanguageByCode;
using Application.CQRS.Languages.Queries.GetLanguages;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Admin.Pos
{
    [Authorize(Roles = "Admin")]
    public class EditPostModel : PageModel
    {
        private int _languageId;

        private readonly IMediator _mediator;

        public EditPostModel(IMediator mediator)
        {
            _mediator = mediator;
        }
        public List<LanguageDto> Languages { get; set; }
        public List<CategoryTranslationDto> Categories { get; set; }
        public LanguageDto DefaultLanguage { get; set; }
        public List<HashtagDto> Hashtags { get; set; }
        //Post info
        [BindProperty]
        public int CategoryId { get; set; }
        [BindProperty]
        public string TitleImageUrl { get; set; }

        //Translation info
        [BindProperty]
        public string Title { get; set; }
        [BindProperty]       
        public int LanguageId
        {
            get { return _languageId; }
            set 
            { 
                _languageId = value; 
                ChangeContentLanguage();
            }
        }


        [BindProperty]
        public string TranslationContent { get; set; }

        public async Task OnGet()
        {
            DefaultLanguage = await _mediator.Send(new GetLanguageByCodeQuery(DefaultStrings.DefaultLanguageCode));
            LanguageId = DefaultLanguage.Id;
            Categories = await _mediator.Send(new GetCategoryTranslationsByLanguageIdQuery(LanguageId));
            Languages = await _mediator.Send(new GetLanguagesQuery());
            Hashtags = await _mediator.Send(new GetHashtagsQuery());
        }

        private async void ChangeContentLanguage()
        {
            Categories = await _mediator.Send(new GetCategoryTranslationsByLanguageIdQuery(_languageId));
        }
    }
}
