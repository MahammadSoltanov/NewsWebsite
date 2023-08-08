using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Admin.Pos
{
    [Authorize(Roles = "Admin")]
    public class EditPostModel : PageModel
    {
        public Post Post { get; set; }
        public List<PostTranslation> PostTranslations { get; set; }
        public void OnGet()
        {
        }
    }
}
