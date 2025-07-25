using Application.Common.Models;
using Application.CQRS.Hashtags.Queries.GetHashtagById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Presentation.Constants;
namespace Presentation.Pages.Admin.Hash
{
    [Authorize(Roles = RoleAccessLevels.AllRoles)]
    public class HashtagDetailsModel : PageModel
    {
        private readonly IMediator _mediator;

        public HashtagDetailsModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public HashtagDto Hashtag { get; set; }
        public async Task OnGetAsync(int Id)
        {
            Hashtag = await _mediator.Send(new GetHashtagByIdQuery(Id));
        }
    }
}
