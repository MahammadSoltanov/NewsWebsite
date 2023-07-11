using Application.CQRS.Languages.Commands.CreateLanguage;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Admin.Lan
{
    public class AddLanguageModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IValidator<CreateLanguageCommand> _validator;

        public AddLanguageModel(IMediator mediator, IValidator<CreateLanguageCommand> validator)
        {
            _mediator = mediator;
            _validator = validator;
        }

        [BindProperty]
        public string Title { get; init; }
        [BindProperty]
        public string Code { get; init; }

        public void OnGet()
        {

        }

        public async Task<ActionResult> OnPostAsync()
        {
            CreateLanguageCommand createLanguageCommand = new CreateLanguageCommand()
            {
                Code = Code,
                Title = Title
            };

            ValidationResult result = await _validator.ValidateAsync(createLanguageCommand);

            if(!result.IsValid) 
            {
                result.AddToModelState(this.ModelState);

                return Page();
            }


            int id = await _mediator.Send(createLanguageCommand);
            string _message = $"Language with ID = {id} was successfully created";
            return RedirectToPage("/Admin/Succeed", new { message = _message, entityName = "Language" });
        }
    }
}
