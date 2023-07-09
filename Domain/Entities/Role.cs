namespace Domain.Entities
{
    public class Role : BaseAuditableEntity
    {
        public string Title { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
