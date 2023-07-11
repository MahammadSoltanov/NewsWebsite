using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.CategoryTranslations.Queries.GetCategoryTranslations;

public record GetCategoryTranslationsQuery : IRequest<List<CategoryTranslationDto>>;

public class GetCategoryTranslationsQueryHandler : IRequestHandler<GetCategoryTranslationsQuery, List<CategoryTranslationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCategoryTranslationsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CategoryTranslationDto>> Handle(GetCategoryTranslationsQuery request, CancellationToken cancellationToken)
    {
        var translations = await _context.CategoryTranslations.ToListAsync(cancellationToken);
        var transaltionDtos = _mapper.Map<List<CategoryTranslationDto>>(translations);
        return transaltionDtos;
    }
}
