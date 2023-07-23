namespace Domain.Events.CategoryTranslations
{
    public class CategoryTranslationCreatedEvent : BaseEvent
    {
        public CategoryTranslationCreatedEvent(CategoryTranslation categoryTranslation) 
        {
            CategoryTranslation = categoryTranslation;
        }
        
        public CategoryTranslation CategoryTranslation{ get; }
    }
}
