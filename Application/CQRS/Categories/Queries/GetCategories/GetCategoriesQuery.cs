using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Categories.Queries.GetCategories;

public record GetCategoriesQuery : IRequest<List<CategoryDto>>;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, List<CategoryDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCategoriesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        List<Category> categories = await _context.Categories.ToListAsync(cancellationToken);
        List<CategoryDto> categoryDtos = _mapper.Map<List<CategoryDto>>(categories);

        List<CategoryTranslation> categoryTranslations = await _context.CategoryTranslations.ToListAsync(cancellationToken);

        foreach(CategoryDto categoryDto in categoryDtos) 
        {
            var transaltionsForCategory = categoryTranslations
                .Where(t => t.Id == categoryDto.Id)
                .ToList();

            var translationDtos = _mapper.Map<List<CategoryTranslationDto>>(transaltionsForCategory);
            
            categoryDto.CategoryTranslationDtos = translationDtos;
        }

    }

}

