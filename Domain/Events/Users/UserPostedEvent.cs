namespace Domain.Events.Users
{
    public class UserPostedEvent : BaseEvent
    {
        public UserPostedEvent(Post post, User user)
        {
            Post = post;
            User = user;
        }

        public Post Post { get; private set; }
        public User User { get; private set; }
    }
}
