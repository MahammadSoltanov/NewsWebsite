namespace Domain.Events.Posts
{
    public class PostCreatedEvent : BaseEvent
    {
        public PostCreatedEvent(Post post)
        {
            Post = post;
        }

        public Post Post { get; private set; }
    }
}
