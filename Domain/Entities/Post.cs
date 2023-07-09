namespace Domain.Entities
{
    public class Post : BaseAuditableEntity
    {
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string Title { get; set; }
        public string? Status { get; set; }
        public DateTime? PublishDate { get; set; }
        public DateTime? InsertDate { get; set; }
        public ICollection<PostHashtag> PostHashtags { get; set; }
        public ICollection<PostTranslation> PostTranslations { get; set; }
    }
}
