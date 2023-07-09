using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Admin.Pos
{
    public class PostTranslationDetailsModel : PageModel
    {
        public PostTranslationDetailsModel()
        {
            PostTranslation = new PostTranslation();
        }
        public PostTranslation PostTranslation { get; set; }
        public void OnGet()
        {
        }
    }
}
