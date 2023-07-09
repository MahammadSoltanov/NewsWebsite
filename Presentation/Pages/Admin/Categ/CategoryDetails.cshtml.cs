using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Admin.Categ
{
    public class CategoryDetailsModel : PageModel
    {
        public CategoryDetailsModel()
        {
            Category = new Category();
        }
        public Category Category { get; set; }
        public void OnGet()
        {
        }
    }
}
