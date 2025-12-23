using Microsoft.AspNetCore.Mvc;
using Presentation.Models.Navigation;

namespace Presentation.ViewComponents;

public class AdminSidebarMenuItemViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(bool isAdmin, bool isModerator)
    {
        var items = new List<AdminSidebarMenuItem>
        {
            new()
            {
                Title = "Languages",
                Url = "/Admin/Languages",
                IconHtml = "<ion-icon class='nav-icon far' name='language-outline'></ion-icon>",
                IsVisible = () => isAdmin
            },
            new()
            {
                Title = "Posts",
                Url = "/Admin/Posts",
                IconHtml = "<i class='nav-icon far fa-image'></i>",
                IsVisible = () => true
            },
            new()
            {
                Title = "Users",
                Url = "/Admin/Users",
                IconHtml = "<i class='nav-icon fas ion-person-add'></i>",
                IsVisible = () => isAdmin || isModerator
            },
            new()
            {
                Title = "Hashtags",
                Url = "/Admin/Hashtags",
                IconHtml = "<ion-icon class='nav-icon fas' name='pricetag-outline'></ion-icon>",
                IsVisible = () => true
            },
            new()
            {
                Title = "Categories",
                Url = "/Admin/Categories",
                IconHtml = "<ion-icon class='nav-icon fas' name='grid-outline'></ion-icon>",
                IsVisible = () => true
            },
            new()
            {
                Title = "Category approval",
                Url = "/Admin/Approve/CategoryTranslationsStatus",
                IconHtml = "<ion-icon class='nav-icon fas' name='checkmark-done-outline'></ion-icon>",
                IsVisible = () => isAdmin || isModerator
            },
            new()
            {
                Title = "Post approval",
                Url = "/Admin/Approve/PostsStatus",
                IconHtml = "<ion-icon class='nav-icon fas' name='checkmark-done-outline'></ion-icon>",
                IsVisible = () => true
            },
            new()
            {
                Title = "Post translation approval",
                Url = "/Admin/Approve/PostTranslationsStatus",
                IconHtml = "<ion-icon class='nav-icon fas' name='checkmark-done-outline'></ion-icon>",
                IsVisible = () => true
            },

        };

        return View(items.Where(x => x.IsVisible()));
    }
}
