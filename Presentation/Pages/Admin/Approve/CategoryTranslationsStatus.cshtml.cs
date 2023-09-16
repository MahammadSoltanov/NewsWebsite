using Application.Common.Models;
using Application.CQRS.CategoryTranslations.Commands.ChangeCategoryStatuses;
using Application.CQRS.CategoryTranslations.Queries.GetCategoryTranslations;
using Application.CQRS.PostTranslations.Commands.ChangePostTranslationsStatuses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Presentation.Pages.Admin.Approve
{
    public class CategoryTranslationsStatusModel : PageModel
    {
        private readonly IMediator _mediator;

        public CategoryTranslationsStatusModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<CategoryTranslationDto> Translations { get; set; }

        public async Task OnGetAsync()
        {
            await UpdateProperties();
        }

        public async Task OnPostChangeStatusAsync(string selectedvalues)
        {
            List<CategoryTranslationStatusObj> selectedCategories = JsonStringToList(selectedvalues);

            ChangeCategoryTranslationStatusesCommand command = new ChangeCategoryTranslationStatusesCommand()
            {
                ChangedCategories = selectedCategories
            };

            await _mediator.Send(command);
            await UpdateProperties();
        }

        private List<CategoryTranslationStatusObj> JsonStringToList(string json)
        {
            List<CategoryTranslationStatusObj> list = JsonConvert.DeserializeObject<List<CategoryTranslationStatusObj>>(json);
            return list;
        }

        private async Task UpdateProperties()
        {
            Translations = await _mediator.Send(new GetCategoryTranslationsQuery());
        }
        

    }
}
