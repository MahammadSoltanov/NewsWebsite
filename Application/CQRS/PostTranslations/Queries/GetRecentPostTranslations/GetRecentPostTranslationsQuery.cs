using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.PostTranslations.Queries.GetRecentPostTranslations;

public record GetRecentPostTranslationsQuery : IRequest<List<PostTranslationDto>>;

public class GetRecentPostTranslationsQueryHandler : IRequestHandler<GetRecentPostTranslationsQuery, List<PostTranslationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private DefaultContainer _container;

    public GetRecentPostTranslationsQueryHandler(IApplicationDbContext context, IMapper mapper, IMediator mediator)
    {
        _context = context;
        _mapper = mapper;
        _container = new DefaultContainer(mediator);
    }

    public async Task<List<PostTranslationDto>> Handle(GetRecentPostTranslationsQuery request, CancellationToken cancellationToken)
    {
        var defaultLanguageId = await _container.GetDefaultPostLanguageId();
        var translations = (await _context.PostTranslations            
            .OrderByDescending(pt => pt.PublishDate)
            .Where(pt => pt.LanguageId == defaultLanguageId)
            .ToListAsync(cancellationToken)).Take(3);

        var translationDtos = _mapper.Map<List<PostTranslationDto>>(translations);

        return translationDtos;
    }
}