using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace Presentation.Pages.Admin.Hash
{
    public class HashtagDetailsModel : PageModel
    {
        public HashtagDetailsModel()
        {
            Hashtag = new Hashtag();
        }
        public Hashtag Hashtag{ get; set; }
        public void OnGet()
        {
        }
    }
}
