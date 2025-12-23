namespace Presentation.Models.Navigation;
public sealed class AdminSidebarMenuItem
{
    public string Title { get; init; }
    public string Url { get; init; }
    public string IconHtml { get; init; }
    public Func<bool> IsVisible { get; init; }
}

