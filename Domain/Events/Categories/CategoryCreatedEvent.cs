namespace Domain.Events.Categories
{
    public class CategoryCreatedEvent : BaseEvent
    {
        public CategoryCreatedEvent(Category category)
        {
            Category = category;
        }

        public Category Category { get; }
    }
}
