using Application.Common.Models;
using Application.CQRS.Users.Queries.GetUserById;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Admin.Usr
{
    [Authorize(Roles = "Admin, Moderator, Journalist")]
    public class UserDetailsModel : PageModel
    {
        private readonly IMediator _mediator;
        public UserDto UserP{ get; set; }
       
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
