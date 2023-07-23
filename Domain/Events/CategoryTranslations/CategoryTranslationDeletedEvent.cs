namespace Domain.Events.CategoryTranslations;

public class CategoryTranslationDeletedEvent : BaseEvent
{
    public CategoryTranslationDeletedEvent(CategoryTranslation categoryTranslation)
    {
        CategoryTranslation = categoryTranslation;
    }

    public CategoryTranslation CategoryTranslation { get; }
}
