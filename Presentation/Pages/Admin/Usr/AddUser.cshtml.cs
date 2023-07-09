using Application.Common.Models;
using Application.CQRS.Roles.Queries.GetRoles;
using Application.CQRS.Users.Commands.CreateUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Admin.Usr
{
    public class AddUserModel : PageModel
    {
        private readonly IMediator _mediator;

        public AddUserModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public string Name { get; set; }
        [BindProperty]
        public string Surname { get; set; }
        [BindProperty]
        public string Password { get; set; }
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public int RoleId { get; set; }
        public List<RoleDto> Roles { get; set; }

        public async Task OnGetAsync()
        {
            Roles = await _mediator.Send(new GetRolesQuery());
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            CreateUserCommand createUserCommand = new CreateUserCommand()
            {
                Name = Name,
                Surname = Surname,
                Email = Email,
                Password = Password,
                RoleId = RoleId
            };

            int id = await _mediator.Send(createUserCommand);
            string _message = $"User with Id = {id} was successfully created";
            return new RedirectToPageResult("/Admin/Succeed", new { message = _message, entityName = "User" });
        }
    }
}
