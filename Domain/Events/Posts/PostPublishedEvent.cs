namespace Domain.Events.Posts
{
    public class PostPublishedEvent : BaseEvent
    {
        public PostPublishedEvent(Post post)
        {
            Post = post;
        }

        public Post Post { get; private set; }
    }
}
