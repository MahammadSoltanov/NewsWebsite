namespace Domain.Events.Languages;

public class LanguageCreatedEvent : BaseEvent
{
    public LanguageCreatedEvent(Language language) 
    {
        Language = language;    
    }
    public Language Language { get; }
}
