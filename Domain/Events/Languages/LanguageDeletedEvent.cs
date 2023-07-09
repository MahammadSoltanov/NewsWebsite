using Domain.Common;

namespace Domain.Events.Languages;

public class LanguageDeletedEvent : BaseEvent
{
    public LanguageDeletedEvent(Language language)
	{
        Language = language;
	}

    public Language Language { get; set; }
}
