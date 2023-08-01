using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class Role : IdentityRole<int>
    {
        public string Title { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
