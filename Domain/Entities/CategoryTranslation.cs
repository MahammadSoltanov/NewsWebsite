namespace Domain.Entities
{
    public class CategoryTranslation : BaseEntity
    {
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int LanguageId { get; set; }
        public Language Language { get; set; }
        public string Title { get; set; }
        public string? Status { get; set; }
        public DateTime? PublishDate { get; set; }
        public DateTime? InsertDate { get; set; }
    }
}
