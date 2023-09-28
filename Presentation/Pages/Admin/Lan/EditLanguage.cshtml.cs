using Application.Common.Models;
using Application.CQRS.Languages.Commands.CreateLanguage;
using Application.CQRS.Languages.Commands.UpdateLanguage;
using Application.CQRS.Languages.Queries.GetLanguage;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Admin.Lan
{
    [Authorize(Roles = "Admin, Moderator, Journalist")]
    public class EditLanguageModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IValidator<UpdateLanguageCommand> _validator;

        [BindProperty]
        public string Title { get; set; }        
        [BindProperty]
        public string Code { get; set; }        
        [BindProperty]
        public int Id{ get; set; }        
        
        public EditLanguageModel(IMediator mediator, IValidator<UpdateLanguageCommand> validator)
        {
            _mediator = mediator;
            _validator = validator;
        }

        public async Task OnGetAsync(int id)
        {
            GetLanguageByIdQuery getLanguageByIdQuery = new GetLanguageByIdQuery() { Id = id };
            LanguageDto language = await _mediator.Send(getLanguageByIdQuery);
            Title = language.Title;
            Code = language.Code;
            Id = language.Id;
        }

        public async Task<ActionResult> OnPostPutAsync()
        {
            UpdateLanguageCommand updateLanguageCommand = new UpdateLanguageCommand()
            {
                Id = Id,
                Code = Code,
                Title = Title
            };

            ValidationResult result = await _validator.ValidateAsync(updateLanguageCommand);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);

                return Page();
            }

            await _mediator.Send(updateLanguageCommand);
            string _message = $"Language with Id = {Id} was successfully updated";
            return new RedirectToPageResult("/Admin/Succeed",  new { message = _message, entityName = "Languages"});
        }


    }
}
