using Application.Common.Models;
using Application.CQRS.Roles.Queries.GetRoles;
using Application.CQRS.Users.Commands.UpdateUser;
using Application.CQRS.Users.Queries.GetUserById;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Admin.Usr
{
    [Authorize(Roles = "Admin")]
    public class EditUserModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IValidator<UpdateUserCommand> _validator;

        public EditUserModel(IMediator mediator, IValidator<UpdateUserCommand> validator)
        {
            _mediator = mediator;
            _validator = validator;
        }

        [BindProperty]
        public int Id { get; set; }
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
        public async Task OnGetAsync(int id)
        {
            UserDto user = await _mediator.Send(new GetUserByIdQuery(id));
            Id = user.Id;
            Name = user.Name;
            Surname = user.Surname;
            Password = user.Password;
            Email = user.Email;

            Roles = await _mediator.Send(new GetRolesQuery());
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            UpdateUserCommand command = new UpdateUserCommand()
            {
                Id = Id,
                Name = Name,
                Surname = Surname,
                Password = Password,
                Email = Email,
                RoleId = RoleId,
            };

            ValidationResult result = await _validator.ValidateAsync(command);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                Roles = await _mediator.Send(new GetRolesQuery());

                return Page();
            }

            await _mediator.Send(command);

            string _message = $"User details with Id = {Id} were successfully updated";

            return new RedirectToPageResult("/Admin/Succeed", new { message = _message, entityName = "User" });
        }
    }
}
