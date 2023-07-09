namespace Domain.Entities
{
    public class PostTranslation : BaseAuditableEntity
    {
        public int AuthorId { get; set; }   
        public User User { get; set; }
        public int LanguageId { get; set; }
        public Language Language { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public long? ViewCount { get; set; }
        public string Title { get; set; }
        public string? Content { get; set; }
        public string? Status { get; set; }
        public DateTime? PublishDate { get; set; }
        public DateTime? InsertDate { get; set; }
    }
}
