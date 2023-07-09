namespace Domain.Events.PostTranslations
{
    public class PostTranslationEditedEvent : BaseEvent
    {
        public PostTranslationEditedEvent(PostTranslation oldVersion, PostTranslation newVersion) 
        { 
            OldVersion = oldVersion;
            NewVersion = newVersion;
        }

        public PostTranslation OldVersion { get; private set; }
        public PostTranslation NewVersion { get; private set;}
    }
}
