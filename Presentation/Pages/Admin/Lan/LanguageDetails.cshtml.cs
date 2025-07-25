using Application.Common.Models;
using Application.CQRS.Languages.Queries.GetLanguage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Presentation.Constants;

namespace Presentation.Pages.Admin.Lan
{
    [Authorize(Roles = RoleAccessLevels.AllRoles)]
    public class LanguageDetailsModel : PageModel
    {
        public LanguageDto Language { get; set; }
        private readonly IMediator _mediator;
        public LanguageDetailsModel(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task OnGetAsync(int id)
        {
            GetLanguageByIdQuery getLanguageByIdQuery = new GetLanguageByIdQuery(id);
            Language = await _mediator.Send(getLanguageByIdQuery);
        }
    }
}
