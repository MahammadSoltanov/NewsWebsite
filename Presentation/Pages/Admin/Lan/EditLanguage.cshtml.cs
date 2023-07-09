using Application.Common.Models;
using Application.CQRS.Languages.Commands.UpdateLanguage;
using Application.CQRS.Languages.Queries.GetLanguage;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Admin.Lan
{
    public class EditLanguageModel : PageModel
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public LanguageDto Language { get; set; }
        
        public EditLanguageModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task OnGetAsync(int Id)
        {
            GetLanguageByIdQuery getLanguageByIdQuery = new GetLanguageByIdQuery() { Id = Id };
            Language = await _mediator.Send(getLanguageByIdQuery);
        }

        public async Task<IActionResult> OnPostPutAsync()
        {
            UpdateLanguageCommand updateLanguageCommand = new UpdateLanguageCommand()
            {
                Id = Language.Id,
                Code = Language.Code,
                Title = Language.Title
            };

            await _mediator.Send(updateLanguageCommand);
            string _message = $"Language with Id = {Language.Id} was successfully updated";
            return new RedirectToPageResult("/Admin/Succeed",  new { message = _message, entityName = "Language"});
        }


    }
}
