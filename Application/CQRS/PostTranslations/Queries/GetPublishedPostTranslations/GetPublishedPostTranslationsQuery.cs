using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.PostTranslations.Queries.GetPublishedPostTranslations;

public record GetPublishedPostTranslationsQuery : IRequest<List<PostTranslationDto>>;

public class GetPublishedPostTranslationsQueryHandler : IRequestHandler<GetPublishedPostTranslationsQuery, List<PostTranslationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPublishedPostTranslationsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<PostTranslationDto>> Handle(GetPublishedPostTranslationsQuery request, CancellationToken cancellationToken)
    {
        var posts = await _context.PostTranslations
            .Where(pt => pt.Status == "Published")
            .ToListAsync(cancellationToken);

        var postDtos = _mapper.Map<List<PostTranslationDto>>(posts);

        if(postDtos == null)
        {
            return new List<PostTranslationDto>();
        }

        return postDtos;
    }
}
