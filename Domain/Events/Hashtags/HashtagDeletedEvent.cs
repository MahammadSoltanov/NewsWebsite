namespace Domain.Events.Hashtags;

public class HashtagDeletedEvent : BaseEvent
{
    public HashtagDeletedEvent(Hashtag hashtag)
    {
        Hashtag = hashtag;
    }

    public Hashtag Hashtag { get; set; }
}
