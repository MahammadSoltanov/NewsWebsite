using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Admin.Categ
{
    public class CategoryTranslationDetailsModel : PageModel
    {
        public CategoryTranslation CategoryTranslation { get; set; }
        public CategoryTranslationDetailsModel()
        {
            CategoryTranslation = new CategoryTranslation();
        }
        public void OnGet()
        {
        }
    }
}
