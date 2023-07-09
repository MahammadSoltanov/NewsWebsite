using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Admin
{
    public class SucceedModel : PageModel
    {
        public void OnGet(string message, string entityName)
        {
            this.ViewData["Message"] = message;
            this.ViewData["EntityName"] = entityName;
        }
    }
}
