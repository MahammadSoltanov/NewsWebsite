using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.CategoryTranslations.Queries.GetCategoryTranslationByCategoryId;

public record GetCategoryTranslationsByCategoryIdQuery(int CategoryId) : IRequest<List<CategoryTranslationDto>>;

public class GetCategoryTranslationsByCategoryIdQueryHandler
    : IRequestHandler<GetCategoryTranslationsByCategoryIdQuery, List<CategoryTranslationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCategoryTranslationsByCategoryIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CategoryTranslationDto>> Handle(GetCategoryTranslationsByCategoryIdQuery request, CancellationToken cancellationToken)
    {
        var translations = await _context.CategoryTranslations
        .Where(ct => ct.CategoryId == request.CategoryId)
        .ToListAsync(cancellationToken);

        var translationDtos = _mapper.Map<List<CategoryTranslationDto>>(translations);

        return translationDtos;
    }
}
