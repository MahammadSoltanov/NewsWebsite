using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.PostTranslations.Queries.GetPostTranslations;

public record GetPostTranslationsQuery : IRequest<List<PostTranslationDto>>;

public class GetPostTranslationsQueryHandler : IRequestHandler<GetPostTranslationsQuery, List<PostTranslationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPostTranslationsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<PostTranslationDto>> Handle(GetPostTranslationsQuery request, CancellationToken cancellationToken)
    {
        var postTranslations = await _context.PostTranslations
            .Where(pt => pt.Status != "Deleted")
            .ToListAsync(cancellationToken);
        var postTranslationDtos = _mapper.Map<List<PostTranslationDto>>(postTranslations);
        return postTranslationDtos;
    }
}