using Domain.Events.Users;

namespace Domain.Entities
{
    public class User : BaseAuditableEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public ICollection<PostTranslation> PostTranslations { get; set; }

    }
}
