using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Admin.Pos
{
    [Authorize(Roles = "Admin")]
    public class PostDetailsModel : PageModel
    {
        public PostDetailsModel()
        {
            Post = new Post();
        }
        public Post Post { get; set; }
        public void OnGet()
        {
        }
    }
}
