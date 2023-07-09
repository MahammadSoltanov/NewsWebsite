namespace Domain.Events.Hashtags;

public class HashtagCreatedEvent : BaseEvent
{
    public HashtagCreatedEvent(Hashtag hashtag)
    {
        Hashtag = hashtag;
    }
    public Hashtag Hashtag { get; }
}
