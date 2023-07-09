namespace Domain.Events.CategoryTranslations
{
    public class CategoryTranslationEditedEvent : BaseEvent
    {
        public CategoryTranslationEditedEvent(CategoryTranslation oldVersion, CategoryTranslation newVersion) 
        {
            OldVersion = oldVersion; 
            NewVersion = newVersion;
        }

        public CategoryTranslation OldVersion { get; private set; }
        public CategoryTranslation NewVersion { get; private set; }
    }
}
