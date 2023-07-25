namespace Domain.Events.PostTranslations
{
    public class PostTranslationCreatedEvent : BaseEvent
    {
        public PostTranslationCreatedEvent(PostTranslation postTranslation)
        {
            PostTranslation = postTranslation;
        }

        public PostTranslation PostTranslation { get; private set; }
    }
}
