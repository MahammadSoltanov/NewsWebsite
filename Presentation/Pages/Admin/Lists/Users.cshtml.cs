using Application.Common.Models;
using Application.CQRS.Users.Commands.DeleteUser;
using Application.CQRS.Users.Queries.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Admin.Lists
{
    [Authorize(Roles = "Admin")]
    public class UsersModel : PageModel
    {
        private readonly IMediator _mediator;
        public List<UserDto> Users { get; set; }

        public UsersModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task OnGetAsync()
        {
            Users = await _mediator.Send(new GetUsersQuery());
        }

        public async Task<IActionResult> OnPostDeleteAsync(int Id)
        {
            await _mediator.Send(new DeleteUserCommand(Id));
            string message = $"User with Id = {Id} was successfully deleted";
            return new RedirectToPageResult("/Admin/Succeed", new { message = message, entityName = "User" });
        }
    }
}
