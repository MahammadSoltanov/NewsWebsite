using Application.Common.Behaviours.SignIn;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;

namespace Presentation.Pages.Admin.Authentication
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly IMediator _mediator;

        public LoginModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public IActionResult OnGet()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToPage("/Admin/Posts");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostSignInAsync()
        {
            try
            {
                var command = new SignInCommand
                {
                    Email = Email,
                    Password = Password
                };

                var result = await _mediator.Send(command);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "You have entered an invalid username or password");
                    return Page();
                }

                return RedirectToPage("/Admin/Posts");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error during login", ex);
                ModelState.AddModelError(string.Empty, "Something went wrong. Please try again.");
                return Page();
            }
        }
    }
}
