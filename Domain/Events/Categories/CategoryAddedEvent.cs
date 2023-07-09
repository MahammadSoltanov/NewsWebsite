namespace Domain.Events.Categories
{
    public class CategoryAddedEvent : BaseEvent
    {
        public CategoryAddedEvent(Category category)
        {
            Category = category;
        }

        public Category Category { get; private set; }
    }
}
