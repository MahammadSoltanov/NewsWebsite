namespace Application.Common.Models;

public class PostTranslationDto
{
    public int Id { get; set; }
    public int AuthorId { get; set; }
    public int LanguageId { get; set; }
    public int PostId { get; set; }
    public long ViewCount { get; set; }
    public string Title { get; set; }
    public string? Content { get; set; }
    public string Status { get; set; }
    public DateTime? PublishDate { get; set; }
    public DateTime InsertDate { get; set; }
}
