using Application.CQRS.Languages.Commands.CreateLanguage;
using MediatR;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Admin.Lan
{
    public class AddLanguageModel : PageModel
    {
        [BindProperty]
        public string Title { get; init; }
        [BindProperty]
        public string Code { get; init; }
        private readonly IMediator _mediator;
        public AddLanguageModel(IMediator mediator)
        {
            _mediator = mediator;
        }
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            CreateLanguageCommand createLanguageCommand = new CreateLanguageCommand()
            {
                Code = Code,
                Title = Title
            };

            int id = await _mediator.Send(createLanguageCommand);
            string _message = $"Language with ID = {id} was successfully created";
            return new RedirectToPageResult("/Admin/Succeed", new { message = _message, entityName = "Language" });
        }
    }
}
