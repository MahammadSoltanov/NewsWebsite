using Application.Common.Models;
using Application.CQRS.Roles.Queries.GetRoles;
using Application.CQRS.Users.Commands.CreateUser;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Presentation.Constants;
using Serilog;

namespace Presentation.Pages.Admin.Usr
{
    [Authorize(Roles = RoleAccessLevels.AdminAndModerator)]
    public class AddUserModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IValidator<CreateUserCommand> _validator;

        public AddUserModel(IMediator mediator, IValidator<CreateUserCommand> validator)
        {
            _mediator = mediator;
            _validator = validator;
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
        [BindProperty]
        public string RoleName { get; set; }
        public List<RoleDto> Roles { get; set; }

        public async Task OnGetAsync()
        {
            Roles = await _mediator.Send(new GetRolesQuery());
        }

        public async Task<ActionResult> OnPostCreateAsync()
        {
            try
            {
                CreateUserCommand createUserCommand = new CreateUserCommand()
                {
                    Name = Name,
                    Surname = Surname,
                    Email = Email,
                    Password = Password,
                    RoleId = RoleId,
                    RoleName = RoleName
                };

                ValidationResult result = await _validator.ValidateAsync(createUserCommand);

                if (!result.IsValid)
                {
                    result.AddToModelState(this.ModelState);
                    Roles = await _mediator.Send(new GetRolesQuery());
                    return Page();
                }

                int id = await _mediator.Send(createUserCommand);
                string _message = $"User with Id = {id} was successfully created";
                return new RedirectToPageResult("/Admin/Succeed", new { message = _message, entityName = "Users" });
            }

            catch (Exception ex)
            {
                Log.Error(ex, "Something went wrong on creating user\n" + ex.StackTrace);
                return new RedirectToPageResult("/Admin/Error", new { message = "Something went wrong during the operation, please try again or contact the support team.", entityName = "Users" });
            }
        }
    }
}
