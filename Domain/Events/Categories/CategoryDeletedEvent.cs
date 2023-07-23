using Domain.Common;

namespace Domain.Events.Categories;

public class CategoryDeletedEvent : BaseEvent
{
    public CategoryDeletedEvent(Category category)
    {
        Category = category;
    }

    public Category Category { get; set; }
}
