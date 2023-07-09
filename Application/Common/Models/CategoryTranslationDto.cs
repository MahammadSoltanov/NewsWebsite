namespace Application.Common.Models;

public class CategoryTranslationDto
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public int LanguageId { get; set; }
    public string Title { get; set; }
    public string? Status { get; set; }
    public DateTime? PublishDate { get; set; }
    public DateTime? InsertDate { get; set; }
}
