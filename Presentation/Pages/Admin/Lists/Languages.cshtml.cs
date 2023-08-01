using Application.Common.Models;
using Application.CQRS.Languages.Commands.DeleteLanguage;
using Application.CQRS.Languages.Queries.GetLanguages;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Admin.Lists
{
    [AllowAnonymous]
    public class LanguagesModel : PageModel
    {
        public List<LanguageDto> Languages { get; set; }
        private readonly IMediator _mediator;
        public LanguagesModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task OnGetAsync()
        {
            Languages = await _mediator.Send(new GetLanguagesQuery());
        }

        public async Task<IActionResult> OnPostDeleteAsync(int Id)
        {           
            await _mediator.Send(new DeleteLanguageCommand(Id));

            string _message = $"Language with Id = {Id} was successfully deleted";

            return new RedirectToPageResult("/Admin/Succeed", new { message = _message, entityName = "Language" });
        }
    }
}
