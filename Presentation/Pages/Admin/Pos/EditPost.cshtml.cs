using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Admin.Pos
{
    public class EditPostModel : PageModel
    {
        public Post Post { get; set; }
        public List<PostTranslation> PostTranslations { get; set; }
        public void OnGet()
        {
        }
    }
}
