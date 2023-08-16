using Application.Common.Behaviours.SignIn;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Admin.Authentication
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly SignInManager<User> _signInManager;

        public LoginModel(IMediator mediator, SignInManager<User> signInManager)
        {
            _mediator = mediator;
            _signInManager = signInManager;
        }

        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Password { get; set; }

        public void OnGet()
        {
        }

        public async Task<ActionResult> OnPostSignInAsync()
        {
            try
            {
                SignInCommand signInCommand = new SignInCommand()
                {
                    Email = Email,
                    Password = Password
                };

                var result = await _mediator.Send(signInCommand);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "You have entered an invalid username or password");
                    return Page();
                }

                return new RedirectToPageResult("/Admin/Lists/Posts");
            }
            catch (Exception ex)
            {
                return new RedirectToPageResult("/Admin/Succeed", new { message = ex.Message });
            }
        }
    }
}
