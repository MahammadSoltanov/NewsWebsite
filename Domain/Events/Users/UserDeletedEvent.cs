using Domain.Common;

namespace Domain.Events.Users;

public class UserDeletedEvent : BaseEvent
{
    public UserDeletedEvent(User user)
    {
        User = user;
    }

    public User User { get; set; }
}
