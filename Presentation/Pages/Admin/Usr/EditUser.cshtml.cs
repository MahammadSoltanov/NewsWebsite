using Application.Common.Models;
using Application.CQRS.Roles.Queries.GetRoles;
using Application.CQRS.Users.Commands.UpdateUser;
using Application.CQRS.Users.Queries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Admin.Usr
{
    public class EditUserModel : PageModel
    {
        private readonly IMediator _mediator;

        public EditUserModel(IMediator mediator)
        {
            _mediator = mediator;
        }
        [BindProperty]
        public UserDto UserP { get; set; }
        public List<RoleDto> Roles { get; set; }
        public async Task OnGetAsync(int Id)
        {
            UserP = await _mediator.Send(new GetUserByIdQuery(Id));
            Roles = await _mediator.Send(new GetRolesQuery());
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            UpdateUserCommand command = new UpdateUserCommand()
            {
                Id = UserP.Id,
                Name = UserP.Name,
                Surname = UserP.Surname,
                Email = UserP.Email,
                RoleId = UserP.RoleId,
            };

            await _mediator.Send(command);

            string _message = $"User details with Id = {UserP.Id} were successfully updated";

            return new RedirectToPageResult("/Admin/Succeed", new { message = _message, entityName = "User" });
        }
    }
}
