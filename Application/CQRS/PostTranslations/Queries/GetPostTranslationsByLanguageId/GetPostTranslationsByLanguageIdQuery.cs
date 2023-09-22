using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.PostTranslations.Queries.GetPostTranslationsByLanguageId;

public record GetPostTranslationsByLanguageIdQuery(int LanguageId) : IRequest<List<PostTranslationDto>>;

public class GetPostTranslationsByLanguageIdQueryHandler : IRequestHandler<GetPostTranslationsByLanguageIdQuery, List<PostTranslationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPostTranslationsByLanguageIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<PostTranslationDto>> Handle(GetPostTranslationsByLanguageIdQuery request, CancellationToken cancellationToken)
    {
        var postTranslations = await _context.PostTranslations
            .Where(pt => pt.LanguageId == request.LanguageId && pt.Status != "Deleted")
            .ToListAsync(cancellationToken);

        if(!postTranslations.Any())
        {
            throw new NotFoundException(nameof(PostTranslation), request.LanguageId);
        }

        var postTranslationDtos = _mapper.Map<List<PostTranslationDto>>(postTranslations);

        return postTranslationDtos;
    }
}


