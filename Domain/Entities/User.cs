using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User : IdentityUser<int>
    {
        public override int Id { get; set; }
        public string Name { get; set; }
        public override string UserName { get => Email; set => Email = value; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public ICollection<PostTranslation> PostTranslations { get; set; }
    }
}
