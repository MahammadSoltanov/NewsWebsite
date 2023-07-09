using Domain.Entities;

namespace Application.Common.Models;

public class CategoryDto
{
    public int Id { get; set; } 
    public string Description { get; set; }
    public List<CategoryTranslationDto> CategoryTranslationDtos { get; set; }
}
