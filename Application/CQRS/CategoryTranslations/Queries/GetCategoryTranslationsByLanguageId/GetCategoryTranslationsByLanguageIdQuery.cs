using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.CategoryTranslations.Queries.GetCategoryTranslationsByLanguageId;

public record GetCategoryTranslationsByLanguageIdQuery(int LanguageId) : IRequest<List<CategoryTranslationDto>>;

public class GetCategoryTranslationsByLanguageIdQueryHandler : IRequestHandler<GetCategoryTranslationsByLanguageIdQuery, List<CategoryTranslationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCategoryTranslationsByLanguageIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CategoryTranslationDto>> Handle(GetCategoryTranslationsByLanguageIdQuery request,  CancellationToken cancellationToken)
    {
        var translations = await _context.CategoryTranslations
            .Where(ct => ct.LanguageId == request.LanguageId)
            .ToListAsync(cancellationToken);

        var translationDtos = _mapper.Map<List<CategoryTranslationDto>>(translations);

        return translationDtos;
    }
}

