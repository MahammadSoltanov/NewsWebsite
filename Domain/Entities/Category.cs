namespace Domain.Entities
{
    public class Category : BaseEntity
    {
        public string? Description { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<CategoryTranslation> CategoryTranslations { get; set; }
    }
}
