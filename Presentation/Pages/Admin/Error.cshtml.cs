using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;

namespace Presentation.Pages.Admin
{
    [AllowAnonymous]
    public class ErrorModel : PageModel
    {
        public void OnGet(string message, string entityName)
        {
            this.ViewData["Message"] = message;
            this.ViewData["EntityName"] = entityName;
        }
    }
}
