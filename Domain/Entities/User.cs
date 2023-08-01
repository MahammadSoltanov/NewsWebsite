using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public ICollection<PostTranslation> PostTranslations { get; set; }

    }
}
