using Application.Common.Models;
using Application.CQRS.Users.Queries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Presentation.Constants;

namespace Presentation.Pages.Admin.Usr
{
    [Authorize(Roles = RoleAccessLevels.AllRoles)]
    public class UserDetailsModel : PageModel
    {
        private readonly IMediator _mediator;
        public UserDto UserP { get; set; }

        public UserDetailsModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task OnGetAsync(int id)
        {
            UserP = await _mediator.Send(new GetUserByIdQuery(id));
        }
    }
}
