using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.PostTranslations.Queries.GetPostTranslationsByPostId;

public record GetPostTranslationsByPostIdQuery(int PostId) : IRequest<List<PostTranslationDto>>;

public class GetPostTranslationsByPostIdQueryHandler : IRequestHandler<GetPostTranslationsByPostIdQuery, List<PostTranslationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPostTranslationsByPostIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<PostTranslationDto>> Handle(GetPostTranslationsByPostIdQuery request, CancellationToken cancellationToken)
    {
        var postTranslations = await _context.PostTranslations
            .Where(pt => pt.PostId == request.PostId)
            .ToListAsync(cancellationToken);

        var postTranslationDtos = _mapper.Map<List<PostTranslationDto>>(postTranslations);

        return postTranslationDtos;
    }
}

