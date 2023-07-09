using Application.Common.Models;
using Application.CQRS.Languages.Queries.GetLanguage;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Admin.Lan
{
    public class LanguageDetailsModel : PageModel
    {
        public LanguageDto Language { get; set; }
        private readonly IMediator _mediator;
        public LanguageDetailsModel(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task OnGetAsync(int Id)
        {
            GetLanguageByIdQuery getLanguageByIdQuery = new GetLanguageByIdQuery() { Id = Id };
            Language = await _mediator.Send(getLanguageByIdQuery);
        }
    }
}
